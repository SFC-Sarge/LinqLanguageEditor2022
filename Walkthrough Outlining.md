# Walkthrough: Outlining


<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Prerequisites](#Prerequisites)
   - [Create a Managed Extensibility Framework (MEF) project](#Create-a-Managed-Extensibility-Framework-(MEF)-project)
   - [Implement an outlining tagger](#Implement-an-outlining-tagger)
   - [Implement a tagger provider](#Implement-a-tagger-provider)
   - [Build and test the code](#Build-and-test-the-code)
   - [See also](#See-also)
<!--End-Of-TOC-->

## Article

08/05/2021

Set up language-based features such as outlining by defining the kinds of text
regions you want to expand or collapse. You can define regions in the context of
a language service, or define your own file name extension and content type and
apply the region definition to only that type, or apply the region definitions
to an existing content type (such as "text"). This walkthrough shows how to
define and display outlining regions.

## Prerequisites

Starting in Visual Studio 2015, you don't install the Visual Studio SDK from the
download center. It's included as an optional feature in Visual Studio setup.
You can also install the VS SDK later on. For more information, see [Install the
Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2022).

## Create a Managed Extensibility Framework (MEF) project

To create a MEF project

Create a VSIX project. Name the solution OutlineRegionTest.

Add an Editor Classifier item template to the project. For more information, see
[Create an extension with an editor item
template](https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-an-editor-item-template?view=vs-2022).

Delete the existing class files.

## Implement an outlining tagger

Outlining regions are marked by a kind of tag (OutliningRegionTag). This tag
provides the standard outlining behavior. The outlined region can be expanded or
collapsed. The outlined region is marked by a Plus sign (+) if it's collapsed or
a Minus sign (-) if it's expanded, and the expanded region is demarcated by a
vertical line.

The following steps show how to define a tagger that creates outlining regions
for all the regions delimited by the brackets ([,]).

To implement an outlining tagger

-   Add a class file and name it OutliningTagger.

-   Import the following namespaces.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Create a class named OutliningTagger, and have it implement ITagger\<T\>:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal sealed class OutliningTagger : ITagger<IOutliningRegionTag>
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add some fields to track the text buffer and snapshot and to accumulate the sets
of lines that should be tagged as outlining regions. This code includes a list
of Region objects (to be defined later) that represent the outlining regions.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
string startHide = "[";     //the characters that start the outlining region
string endHide = "]";       //the characters that end the outlining region
string ellipsis = "...";    //the characters that are displayed when the region is collapsed
string hoverText = "hover text"; //the contents of the tooltip for the collapsed span
ITextBuffer buffer;
ITextSnapshot snapshot;
List<Region> regions;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a tagger constructor that initializes the fields, parses the buffer, and
adds an event handler to the
[Changed](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.itextbuffer.changed)
event.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public OutliningTagger(ITextBuffer buffer)
{
    this.buffer = buffer;
    this.snapshot = buffer.CurrentSnapshot;
    this.regions = new List<Region>();
    this.ReParse();
    this.buffer.Changed += BufferChanged;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the GetTags method, which instantiates the tag spans. This example
assumes that the spans in the NormalizedSpanCollection passed in to the method
are contiguous, although it may not always be the case. This method instantiates
a new tag span for each of the outlining regions.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public IEnumerable<ITagSpan<IOutliningRegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
{
    if (spans.Count == 0)
        yield break;
    List<Region> currentRegions = this.regions;
    ITextSnapshot currentSnapshot = this.snapshot;
    SnapshotSpan entire = new SnapshotSpan(spans[0].Start, spans[spans.Count - 1].End).TranslateTo(currentSnapshot, SpanTrackingMode.EdgeExclusive);
    int startLineNumber = entire.Start.GetContainingLine().LineNumber;
    int endLineNumber = entire.End.GetContainingLine().LineNumber;
    foreach (var region in currentRegions)
    {
        if (region.StartLine <= endLineNumber &&
            region.EndLine >= startLineNumber)
        {
            var startLine = currentSnapshot.GetLineFromLineNumber(region.StartLine);
            var endLine = currentSnapshot.GetLineFromLineNumber(region.EndLine);

            //the region starts at the beginning of the "[", and goes until the *end* of the line that contains the "]".
            yield return new TagSpan<IOutliningRegionTag>(
                new SnapshotSpan(startLine.Start + region.StartOffset,
                endLine.End),
                new OutliningRegionTag(false, false, ellipsis, hoverText));
        }
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Declare a TagsChanged event handler.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a BufferChanged event handler that responds to
[Changed](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.itextbuffer.changed)
events by parsing the text buffer.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
void BufferChanged(object sender, TextContentChangedEventArgs e)
{
    // If this isn't the most up-to-date version of the buffer, then ignore it for now (we'll eventually get another change event).
    if (e.After != buffer.CurrentSnapshot)
        return;
    this.ReParse();
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a method that parses the buffer. The example given here is for illustration
only. It synchronously parses the buffer into nested outlining regions.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
void ReParse()
{
    ITextSnapshot newSnapshot = buffer.CurrentSnapshot;
    List<Region> newRegions = new List<Region>();

    //keep the current (deepest) partial region, which will have
    // references to any parent partial regions.
    PartialRegion currentRegion = null;

    foreach (var line in newSnapshot.Lines)
    {
        int regionStart = -1;
        string text = line.GetText();

        //lines that contain a "[" denote the start of a new region.
        if ((regionStart = text.IndexOf(startHide, StringComparison.Ordinal)) != -1)
        {
            int currentLevel = (currentRegion != null) ? currentRegion.Level : 1;
            int newLevel;
            if (!TryGetLevel(text, regionStart, out newLevel))
                newLevel = currentLevel + 1;

            //levels are the same and we have an existing region;
            //end the current region and start the next
            if (currentLevel == newLevel && currentRegion != null)
            {
                newRegions.Add(new Region()
                {
                    Level = currentRegion.Level,
                    StartLine = currentRegion.StartLine,
                    StartOffset = currentRegion.StartOffset,
                    EndLine = line.LineNumber
                });

                currentRegion = new PartialRegion()
                {
                    Level = newLevel,
                    StartLine = line.LineNumber,
                    StartOffset = regionStart,
                    PartialParent = currentRegion.PartialParent
                };
            }
            //this is a new (sub)region
            else
            {
                currentRegion = new PartialRegion()
                {
                    Level = newLevel,
                    StartLine = line.LineNumber,
                    StartOffset = regionStart,
                    PartialParent = currentRegion
                };
            }
        }
        //lines that contain "]" denote the end of a region
        else if ((regionStart = text.IndexOf(endHide, StringComparison.Ordinal)) != -1)
        {
            int currentLevel = (currentRegion != null) ? currentRegion.Level : 1;
            int closingLevel;
            if (!TryGetLevel(text, regionStart, out closingLevel))
                closingLevel = currentLevel;

            //the regions match
            if (currentRegion != null &&
                currentLevel == closingLevel)
            {
                newRegions.Add(new Region()
                {
                    Level = currentLevel,
                    StartLine = currentRegion.StartLine,
                    StartOffset = currentRegion.StartOffset,
                    EndLine = line.LineNumber
                });

                currentRegion = currentRegion.PartialParent;
            }
        }
    }

    //determine the changed span, and send a changed event with the new spans
    List<Span> oldSpans =
        new List<Span>(this.regions.Select(r => AsSnapshotSpan(r, this.snapshot)
            .TranslateTo(newSnapshot, SpanTrackingMode.EdgeExclusive)
            .Span));
    List<Span> newSpans =
            new List<Span>(newRegions.Select(r => AsSnapshotSpan(r, newSnapshot).Span));

    NormalizedSpanCollection oldSpanCollection = new NormalizedSpanCollection(oldSpans);
    NormalizedSpanCollection newSpanCollection = new NormalizedSpanCollection(newSpans);

    //the changed regions are regions that appear in one set or the other, but not both.
    NormalizedSpanCollection removed =
    NormalizedSpanCollection.Difference(oldSpanCollection, newSpanCollection);

    int changeStart = int.MaxValue;
    int changeEnd = -1;

    if (removed.Count > 0)
    {
        changeStart = removed[0].Start;
        changeEnd = removed[removed.Count - 1].End;
    }

    if (newSpans.Count > 0)
    {
        changeStart = Math.Min(changeStart, newSpans[0].Start);
        changeEnd = Math.Max(changeEnd, newSpans[newSpans.Count - 1].End);
    }

    this.snapshot = newSnapshot;
    this.regions = newRegions;

    if (changeStart <= changeEnd)
    {
        ITextSnapshot snap = this.snapshot;
        if (this.TagsChanged != null)
            this.TagsChanged(this, new SnapshotSpanEventArgs(
                new SnapshotSpan(this.snapshot, Span.FromBounds(changeStart, changeEnd))));
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The following helper method gets an integer that represents the level of the
outlining, such that 1 is the leftmost brace pair.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
static bool TryGetLevel(string text, int startIndex, out int level)
{
    level = -1;
    if (text.Length > startIndex + 3)
    {
        if (int.TryParse(text.Substring(startIndex + 1), out level))
            return true;
    }

    return false;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The following helper method translates a Region (defined later in this article)
into a SnapshotSpan.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
static SnapshotSpan AsSnapshotSpan(Region region, ITextSnapshot snapshot)
{
    var startLine = snapshot.GetLineFromLineNumber(region.StartLine);
    var endLine = (region.StartLine == region.EndLine) ? startLine
         : snapshot.GetLineFromLineNumber(region.EndLine);
    return new SnapshotSpan(startLine.Start + region.StartOffset, endLine.End);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The following code is for illustration only. It defines a PartialRegion class
that contains the line number and offset of the start of an outlining region,
and a reference to the parent region (if any). This code enables the parser to
set up nested outlining regions. A derived Region class contains a reference to
the line number of the end of an outlining region.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
class PartialRegion
{
    public int StartLine { get; set; }
    public int StartOffset { get; set; }
    public int Level { get; set; }
    public PartialRegion PartialParent { get; set; }
}

class Region : PartialRegion
{
    public int EndLine { get; set; }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Implement a tagger provider

Export a tagger provider for your tagger. The tagger provider creates an
OutliningTagger for a buffer of the "text" content type, or else returns an
OutliningTagger if the buffer already has one.

To implement a tagger provider

Create a class named OutliningTaggerProvider that implements ITaggerProvider,
and export it with the ContentType and TagType attributes.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(ITaggerProvider))]
[TagType(typeof(IOutliningRegionTag))]
[ContentType("text")]
internal sealed class OutliningTaggerProvider : ITaggerProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the CreateTagger method by adding an OutliningTagger to the properties
of the buffer.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
{
    //create a single tagger for each buffer.
    Func<ITagger<T>> sc = delegate() { return new OutliningTagger(buffer) as ITagger<T>; };
    return buffer.Properties.GetOrCreateSingletonProperty<ITagger<T>>(sc);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Build and test the code

To test this code, build the OutlineRegionTest solution and run it in the
experimental instance.

To build and test the OutlineRegionTest solution

Build the solution.

When you run this project in the debugger, a second instance of Visual Studio is
started.

Create a text file. Type some text that includes both the opening brackets and
the closing brackets.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[
   Hello
]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

There should be an outlining region that includes both brackets. You should be
able to click the Minus Sign to the left of the open bracket to collapse the
outlining region. When the region is collapsed, the ellipsis symbol (*...*)
should appear to the left of the collapsed region, and a popup containing the
text hover text should appear when you move the pointer over the ellipsis.

## See also

[Walkthrough: Link a content type to a file name
extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-linking-a-content-type-to-a-file-name-extension?view=vs-2022)
