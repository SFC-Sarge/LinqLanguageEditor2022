# Walkthrough: Display matching braces

<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Prerequisites](#Prerequisites)
   - [Create a Managed Extensibility Framework (MEF) project](#Create-a-Managed-Extensibility-Framework-(MEF)-project)
   - [Implement a brace matching tagger](#Implement-a-brace-matching-tagger)
   - [Implement a brace matching tagger provider](#Implement-a-brace-matching-tagger-provider)
   - [Build and test the code](#Build-and-test-the-code)
<!--End-Of-TOC-->

## Article



08/05/2021

Implement language-based features, such as, brace matching by defining the
braces you want to match, and adding a text marker tag to the matching braces
when the caret is on one of the braces. You can define braces in the context of
a language, define your own file name extension and content type, and apply the
tags to just that type or apply the tags to an existing content type (such as
"text"). The following walkthrough shows how to apply brace matching tags to the
"text" content type.

## Prerequisites

Starting in Visual Studio 2015, you don't install the Visual Studio SDK from the
download center. It's included as an optional feature in Visual Studio setup.
You can also install the VS SDK later on. For more information, see [Install the
Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2022).

## Create a Managed Extensibility Framework (MEF) project

To create a MEF project

Create an Editor Classifier project. Name the solution BraceMatchingTest.

Add an Editor Classifier item template to the project. For more information, see
[Create an extension with an editor item
template](https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-an-editor-item-template?view=vs-2022).

Delete the existing class files.

## Implement a brace matching tagger

To get a brace highlighting effect that resembles the one that's used in Visual
Studio, you can implement a tagger of type TextMarkerTag. The following code
shows how to define the tagger for brace pairs at any level of nesting. In this
example, the brace pairs of [] and {} are defined in the tagger constructor, but
in a full language implementation, the relevant brace pairs would be defined in
the language specification.

To implement a brace matching tagger

Add a class file and name it BraceMatching.

Import the following namespaces.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Define a class BraceMatchingTagger that inherits from ITagger\<T\> of type
TextMarkerTag.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal class BraceMatchingTagger : ITagger<TextMarkerTag>
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add properties for the text view, the source buffer, the current snapshot point,
and also a set of brace pairs.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
ITextView View { get; set; }
ITextBuffer SourceBuffer { get; set; }
SnapshotPoint? CurrentChar { get; set; }
private Dictionary<char, char> m_braceList;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the tagger constructor, set the properties and subscribe to the view change
events PositionChanged and LayoutChanged. In this example, for illustrative
purposes, the matching pairs are also defined in the constructor.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal BraceMatchingTagger(ITextView view, ITextBuffer sourceBuffer)
{
    //here the keys are the open braces, and the values are the close braces
    m_braceList = new Dictionary<char, char>();
    m_braceList.Add('{', '}');
    m_braceList.Add('[', ']');
    m_braceList.Add('(', ')');
    this.View = view;
    this.SourceBuffer = sourceBuffer;
    this.CurrentChar = null;

    this.View.Caret.PositionChanged += CaretPositionChanged;
    this.View.LayoutChanged += ViewLayoutChanged;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

As part of the ITagger\<T\> implementation, declare a TagsChanged event.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The event handlers update the current caret position of the CurrentChar property
and raise the TagsChanged event.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
void ViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
{
    if (e.NewSnapshot != e.OldSnapshot) //make sure that there has really been a change
    {
        UpdateAtCaretPosition(View.Caret.Position);
    }
}

void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
{
    UpdateAtCaretPosition(e.NewPosition);
}
void UpdateAtCaretPosition(CaretPosition caretPosition)
{
    CurrentChar = caretPosition.Point.GetPoint(SourceBuffer, caretPosition.Affinity);

    if (!CurrentChar.HasValue)
        return;

    var tempEvent = TagsChanged;
    if (tempEvent != null)
        tempEvent(this, new SnapshotSpanEventArgs(new SnapshotSpan(SourceBuffer.CurrentSnapshot, 0,
            SourceBuffer.CurrentSnapshot.Length)));
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the GetTags method to match braces either when the current character
is an open brace or when the previous character is a close brace, as in Visual
Studio. When the match is found, this method instantiates two tags, one for the
open brace and one for the close brace.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public IEnumerable<ITagSpan<TextMarkerTag>> GetTags(NormalizedSnapshotSpanCollection spans)
{
    if (spans.Count == 0)   //there is no content in the buffer
        yield break;

    //don't do anything if the current SnapshotPoint is not initialized or at the end of the buffer
    if (!CurrentChar.HasValue || CurrentChar.Value.Position >= CurrentChar.Value.Snapshot.Length)
        yield break;

    //hold on to a snapshot of the current character
    SnapshotPoint currentChar = CurrentChar.Value;

    //if the requested snapshot isn't the same as the one the brace is on, translate our spans to the expected snapshot
    if (spans[0].Snapshot != currentChar.Snapshot)
    {
        currentChar = currentChar.TranslateTo(spans[0].Snapshot, PointTrackingMode.Positive);
    }

    //get the current char and the previous char
    char currentText = currentChar.GetChar();
    SnapshotPoint lastChar = currentChar == 0 ? currentChar : currentChar - 1; //if currentChar is 0 (beginning of buffer), don't move it back
    char lastText = lastChar.GetChar();
    SnapshotSpan pairSpan = new SnapshotSpan();

    if (m_braceList.ContainsKey(currentText))   //the key is the open brace
    {
        char closeChar;
        m_braceList.TryGetValue(currentText, out closeChar);
        if (BraceMatchingTagger.FindMatchingCloseChar(currentChar, currentText, closeChar, View.TextViewLines.Count, out pairSpan) == true)
        {
            yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(currentChar, 1), new TextMarkerTag("blue"));
            yield return new TagSpan<TextMarkerTag>(pairSpan, new TextMarkerTag("blue"));
        }
    }
    else if (m_braceList.ContainsValue(lastText))    //the value is the close brace, which is the *previous* character 
    {
        var open = from n in m_braceList
                   where n.Value.Equals(lastText)
                   select n.Key;
        if (BraceMatchingTagger.FindMatchingOpenChar(lastChar, (char)open.ElementAt<char>(0), lastText, View.TextViewLines.Count, out pairSpan) == true)
        {
            yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(lastChar, 1), new TextMarkerTag("blue"));
            yield return new TagSpan<TextMarkerTag>(pairSpan, new TextMarkerTag("blue"));
        }
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The following private methods find the matching brace at any level of nesting.
The first method finds the close character that matches the open character:

The following helper method finds the open character that matches a close
character:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private static bool FindMatchingOpenChar(SnapshotPoint startPoint, char open, char close, int maxLines, out SnapshotSpan pairSpan)
{
    pairSpan = new SnapshotSpan(startPoint, startPoint);

    ITextSnapshotLine line = startPoint.GetContainingLine();

    int lineNumber = line.LineNumber;
    int offset = startPoint - line.Start - 1; //move the offset to the character before this one

    //if the offset is negative, move to the previous line
    if (offset < 0)
    {
        line = line.Snapshot.GetLineFromLineNumber(--lineNumber);
        offset = line.Length - 1;
    }

    string lineText = line.GetText();

    int stopLineNumber = 0;
    if (maxLines > 0)
        stopLineNumber = Math.Max(stopLineNumber, lineNumber - maxLines);

    int closeCount = 0;

    while (true)
    {
        // Walk the entire line
        while (offset >= 0)
        {
            char currentChar = lineText[offset];

            if (currentChar == open)
            {
                if (closeCount > 0)
                {
                    closeCount--;
                }
                else // We've found the open character
                {
                    pairSpan = new SnapshotSpan(line.Start + offset, 1); //we just want the character itself
                    return true;
                }
            }
            else if (currentChar == close)
            {
                closeCount++;
            }
            offset--;
        }

        // Move to the previous line
        if (--lineNumber < stopLineNumber)
            break;

        line = line.Snapshot.GetLineFromLineNumber(lineNumber);
        lineText = line.GetText();
        offset = line.Length - 1;
    }
    return false;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Implement a brace matching tagger provider

In addition to implementing a tagger, you must also implement and export a
tagger provider. In this case, the content type of the provider is "text". So,
brace matching will appear in all types of text files, but a fuller
implementation applies brace matching only to a specific content type.

To implement a brace matching tagger provider

Declare a tagger provider that inherits from IViewTaggerProvider, name it
BraceMatchingTaggerProvider, and export it with a ContentTypeAttribute of "text"
and a TagTypeAttribute of TextMarkerTag.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IViewTaggerProvider))]
[ContentType("text")]
[TagType(typeof(TextMarkerTag))]
internal class BraceMatchingTaggerProvider : IViewTaggerProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the CreateTagger method to instantiate a BraceMatchingTagger.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
{
    if (textView == null)
        return null;

    //provide highlighting only on the top-level buffer
    if (textView.TextBuffer != buffer)
        return null;

    return new BraceMatchingTagger(textView, buffer) as ITagger<T>;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Build and test the code

To test this code, build the BraceMatchingTest solution and run it in the
experimental instance.

To build and test BraceMatchingTest solution

Build the solution.

When you run this project in the debugger, a second instance of Visual Studio is
started.

Create a text file and type some text that includes matching braces.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
hello {
goodbye}

{}

{hello}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

When you position the caret before an open brace, both that brace and the
matching close brace should be highlighted. When you position the cursor just
after the close brace, both that brace and the matching open brace should be
highlighted.

See also

[Walkthrough: Link a content type to a file name
extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-linking-a-content-type-to-a-file-name-extension?view=vs-2022)
