# Walkthrough: Highlight text


<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Prerequisites](#Prerequisites)
   - [Create a MEF project](#Create-a-MEF-project)
   - [Define a TextMarkerTag](#Define-a-TextMarkerTag)
   - [Implement an ITagger](#Implement-an-ITagger)
   - [Create a Tagger provider](#Create-a-Tagger-provider)
      - [Note](#Note)
   - [Build and test the code](#Build-and-test-the-code)
   - [See also](#See-also)
<!--End-Of-TOC-->

## Article

08/05/2021

You can add different visual effects to the editor by creating Managed
Extensibility Framework (MEF) component parts. This walkthrough shows how to
highlight every occurrence of the current word in a text file. If a word occurs
more than one time in a text file, and you position the caret in one occurrence,
every occurrence is highlighted.

## Prerequisites

Starting in Visual Studio 2015, you don't install the Visual Studio SDK from the
download center. It's included as an optional feature in Visual Studio setup.
You can also install the VS SDK later on. For more information, see [Install the
Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2022).

## Create a MEF project

Create a C\# VSIX project. (In the New Project dialog, select Visual C\# /
Extensibility, then VSIX Project.) Name the solution HighlightWordTest.

Add an Editor Classifier item template to the project. For more information, see
[Create an extension with an editor item
template](https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-an-editor-item-template?view=vs-2022).

Delete the existing class files.

## Define a TextMarkerTag

The first step in highlighting text is to subclass TextMarkerTag and define its
appearance.

To define a TextMarkerTag and a MarkerFormatDefinition

Add a class file and name it HighlightWordTag.

Add the following references:

-   Microsoft.VisualStudio.CoreUtility

-   Microsoft.VisualStudio.Text.Data

-   Microsoft.VisualStudio.Text.Logic

-   Microsoft.VisualStudio.Text.UI

-   Microsoft.VisualStudio.Text.UI.Wpf

-   System.ComponentModel.Composition

-   Presentation.Core

-   Presentation.Framework

Import the following namespaces.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.Windows.Media;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Create a class that inherits from TextMarkerTag and name it HighlightWordTag.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal class HighlightWordTag : TextMarkerTag
{

}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Create a second class that inherits from MarkerFormatDefinition, and name it
HighlightWordFormatDefinition. In order to use this format definition for your
tag, you must export it with the following attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    tags use this to reference this format

-   [UserVisibleAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.classification.uservisibleattribute):
    this causes the format to appear in the UI

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(EditorFormatDefinition))]
[Name("MarkerFormatDefinition/HighlightWordFormatDefinition")]
[UserVisible(true)]
internal class HighlightWordFormatDefinition : MarkerFormatDefinition
{

}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the constructor for HighlightWordFormatDefinition, define its display name
and appearance. The Background property defines the fill color, while the
Foreground property defines the border color.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public HighlightWordFormatDefinition()
{
    this.BackgroundColor = Colors.LightBlue;
    this.ForegroundColor = Colors.DarkBlue;
    this.DisplayName = "Highlight Word";
    this.ZOrder = 5;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the constructor for HighlightWordTag, pass in the name of the format
definition you created.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public HighlightWordTag() : base("MarkerFormatDefinition/HighlightWordFormatDefinition") { }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Implement an ITagger

The next step is to implement the ITagger\<T\> interface. This interface
assigns, to a given text buffer, tags that provide text highlighting and other
visual effects.

To implement a tagger

Create a class that implements ITagger\<T\> of type HighlightWordTag, and name
it HighlightWordTagger.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal class HighlightWordTagger : ITagger<HighlightWordTag>
{

}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add the following private fields and properties to the class:

-   An ITextView, which corresponds to the current text view.

-   An ITextBuffer, which corresponds to the text buffer that underlies the text
    view.

-   An ITextSearchService, which is used to find text.

-   An ITextStructureNavigator, which has methods for navigating within text
    spans.

-   A NormalizedSnapshotSpanCollection, which contains the set of words to
    highlight.

-   A SnapshotSpan, which corresponds to the current word.

-   A SnapshotPoint, which corresponds to the current position of the caret.

-   A lock object.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
ITextView View { get; set; }
ITextBuffer SourceBuffer { get; set; }
ITextSearchService TextSearchService { get; set; }
ITextStructureNavigator TextStructureNavigator { get; set; }
NormalizedSnapshotSpanCollection WordSpans { get; set; }
SnapshotSpan? CurrentWord { get; set; }
SnapshotPoint RequestedPoint { get; set; }
object updateLock = new object();
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a constructor that initializes the properties listed earlier and adds
LayoutChanged and PositionChanged event handlers.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public HighlightWordTagger(ITextView view, ITextBuffer sourceBuffer, ITextSearchService textSearchService,
ITextStructureNavigator textStructureNavigator)
{
    this.View = view;
    this.SourceBuffer = sourceBuffer;
    this.TextSearchService = textSearchService;
    this.TextStructureNavigator = textStructureNavigator;
    this.WordSpans = new NormalizedSnapshotSpanCollection();
    this.CurrentWord = null;
    this.View.Caret.PositionChanged += CaretPositionChanged;
    this.View.LayoutChanged += ViewLayoutChanged;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The event handlers both call the UpdateAtCaretPosition method.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
void ViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
{
    // If a new snapshot wasn't generated, then skip this layout 
    if (e.NewSnapshot != e.OldSnapshot)
    {
        UpdateAtCaretPosition(View.Caret.Position);
    }
}

void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
{
    UpdateAtCaretPosition(e.NewPosition);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

You must also add a TagsChanged event that is called by the update method.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The UpdateAtCaretPosition() method finds every word in the text buffer that is
identical to the word where the cursor is positioned and constructs a list of
SnapshotSpan objects that correspond to the occurrences of the word. It then
calls SynchronousUpdate, which raises the TagsChanged event.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
void UpdateAtCaretPosition(CaretPosition caretPosition)
{
    SnapshotPoint? point = caretPosition.Point.GetPoint(SourceBuffer, caretPosition.Affinity);

    if (!point.HasValue)
        return;

    // If the new caret position is still within the current word (and on the same snapshot), we don't need to check it 
    if (CurrentWord.HasValue
        && CurrentWord.Value.Snapshot == View.TextSnapshot
        && point.Value >= CurrentWord.Value.Start
        && point.Value <= CurrentWord.Value.End)
    {
        return;
    }

    RequestedPoint = point.Value;
    UpdateWordAdornments();
}

void UpdateWordAdornments()
{
    SnapshotPoint currentRequest = RequestedPoint;
    List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();
    //Find all words in the buffer like the one the caret is on
    TextExtent word = TextStructureNavigator.GetExtentOfWord(currentRequest);
    bool foundWord = true;
    //If we've selected something not worth highlighting, we might have missed a "word" by a little bit
    if (!WordExtentIsValid(currentRequest, word))
    {
        //Before we retry, make sure it is worthwhile 
        if (word.Span.Start != currentRequest
             || currentRequest == currentRequest.GetContainingLine().Start
             || char.IsWhiteSpace((currentRequest - 1).GetChar()))
        {
            foundWord = false;
        }
        else
        {
            // Try again, one character previous.  
            //If the caret is at the end of a word, pick up the word.
            word = TextStructureNavigator.GetExtentOfWord(currentRequest - 1);

            //If the word still isn't valid, we're done 
            if (!WordExtentIsValid(currentRequest, word))
                foundWord = false;
        }
    }

    if (!foundWord)
    {
        //If we couldn't find a word, clear out the existing markers
        SynchronousUpdate(currentRequest, new NormalizedSnapshotSpanCollection(), null);
        return;
    }

    SnapshotSpan currentWord = word.Span;
    //If this is the current word, and the caret moved within a word, we're done. 
    if (CurrentWord.HasValue && currentWord == CurrentWord)
        return;

    //Find the new spans
    FindData findData = new FindData(currentWord.GetText(), currentWord.Snapshot);
    findData.FindOptions = FindOptions.WholeWord | FindOptions.MatchCase;

    wordSpans.AddRange(TextSearchService.FindAll(findData));

    //If another change hasn't happened, do a real update 
    if (currentRequest == RequestedPoint)
        SynchronousUpdate(currentRequest, new NormalizedSnapshotSpanCollection(wordSpans), currentWord);
}
static bool WordExtentIsValid(SnapshotPoint currentRequest, TextExtent word)
{
    return word.IsSignificant
        && currentRequest.Snapshot.GetText(word.Span).Any(c => char.IsLetter(c));
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The SynchronousUpdate performs a synchronous update on the WordSpans and
CurrentWord properties, and raises the TagsChanged event.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
void SynchronousUpdate(SnapshotPoint currentRequest, NormalizedSnapshotSpanCollection newSpans, SnapshotSpan? newCurrentWord)
{
    lock (updateLock)
    {
        if (currentRequest != RequestedPoint)
            return;

        WordSpans = newSpans;
        CurrentWord = newCurrentWord;

        var tempEvent = TagsChanged;
        if (tempEvent != null)
            tempEvent(this, new SnapshotSpanEventArgs(new SnapshotSpan(SourceBuffer.CurrentSnapshot, 0, SourceBuffer.CurrentSnapshot.Length)));
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

You must implement the GetTags method. This method takes a collection of
SnapshotSpan objects and returns an enumeration of tag spans.

In C\#, implement this method as a yield iterator, which enables lazy evaluation
(that is, evaluation of the set only when individual items are accessed) of the
tags. In Visual Basic, add the tags to a list and return the list.

Here the method returns a TagSpan\<T\> object that has a "blue" TextMarkerTag,
which provides a blue background.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public IEnumerable<ITagSpan<HighlightWordTag>> GetTags(NormalizedSnapshotSpanCollection spans)
{
    if (CurrentWord == null)
        yield break;

    // Hold on to a "snapshot" of the word spans and current word, so that we maintain the same
    // collection throughout
    SnapshotSpan currentWord = CurrentWord.Value;
    NormalizedSnapshotSpanCollection wordSpans = WordSpans;

    if (spans.Count == 0 || wordSpans.Count == 0)
        yield break;

    // If the requested snapshot isn't the same as the one our words are on, translate our spans to the expected snapshot 
    if (spans[0].Snapshot != wordSpans[0].Snapshot)
    {
        wordSpans = new NormalizedSnapshotSpanCollection(
            wordSpans.Select(span => span.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive)));

        currentWord = currentWord.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive);
    }

    // First, yield back the word the cursor is under (if it overlaps) 
    // Note that we'll yield back the same word again in the wordspans collection; 
    // the duplication here is expected. 
    if (spans.OverlapsWith(new NormalizedSnapshotSpanCollection(currentWord)))
        yield return new TagSpan<HighlightWordTag>(currentWord, new HighlightWordTag());

    // Second, yield all the other words in the file 
    foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, wordSpans))
    {
        yield return new TagSpan<HighlightWordTag>(span, new HighlightWordTag());
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Create a Tagger provider

To create your tagger, you must implement a IViewTaggerProvider. This class is a
MEF component part, so you must set the correct attributes so that this
extension is recognized.

### Note

For more information about MEF, see [Managed Extensibility Framework
(MEF)](https://docs.microsoft.com/en-us/dotnet/framework/mef/index).

To create a tagger provider

Create a class named HighlightWordTaggerProvider that implements
IViewTaggerProvider, and export it with a ContentTypeAttribute of "text" and a
TagTypeAttribute of TextMarkerTag.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IViewTaggerProvider))]
[ContentType("text")]
[TagType(typeof(TextMarkerTag))]
internal class HighlightWordTaggerProvider : IViewTaggerProvider
{ }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

You must import two editor services, the ITextSearchService and the
ITextStructureNavigatorSelectorService, to instantiate the tagger.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal ITextSearchService TextSearchService { get; set; }

[Import]
internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the CreateTagger method to return an instance of HighlightWordTagger.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
{
    //provide highlighting only on the top buffer 
    if (textView.TextBuffer != buffer)
        return null;

    ITextStructureNavigator textStructureNavigator =
        TextStructureNavigatorSelector.GetTextStructureNavigator(buffer);

    return new HighlightWordTagger(textView, buffer, TextSearchService, textStructureNavigator) as ITagger<T>;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Build and test the code

To test this code, build the HighlightWordTest solution and run it in the
experimental instance.

To build and test the HighlightWordTest solution

Build the solution.

When you run this project in the debugger, a second instance of Visual Studio is
started.

Create a text file and type some text in which the words are repeated, for
example: `"hello hello hello"`.

Position the cursor in one of the occurrences of "hello". Every occurrence
should be highlighted in blue.

## See also

[Walkthrough: Link a content type to a file name
extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-linking-a-content-type-to-a-file-name-extension?view=vs-2022)
