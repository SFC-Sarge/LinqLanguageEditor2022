# Walkthrough: Display QuickInfo tooltips


<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Prerequisites](#Prerequisites)
   - [Create a MEF project](#Create-a-MEF-project)
   - [Implement the QuickInfo source](#Implement-the-QuickInfo-source)
   - [Implement a QuickInfo source provider](#Implement-a-QuickInfo-source-provider)
   - [Implement a QuickInfo controller](#Implement-a-QuickInfo-controller)
   - [Implementing the QuickInfo controller provider](#Implementing-the-QuickInfo-controller-provider)
   - [Build and test the Code](#Build-and-test-the-Code)
   - [See also](#See-also)
<!--End-Of-TOC-->

## Article


08/05/2021

QuickInfo is an IntelliSense feature that displays method signatures and
descriptions when a user moves the pointer over a method name. You can implement
language-based features such as QuickInfo by defining the identifiers for which
you want to provide QuickInfo descriptions, and then creating a tooltip in which
to display the content. You can define QuickInfo in the context of a language
service, or you can define your own file name extension and content type and
display the QuickInfo for just that type, or you can display QuickInfo for an
existing content type (such as "text"). This walkthrough shows how to display
QuickInfo for the "text" content type.

The QuickInfo example in this walkthrough displays the tooltips when a user
moves the pointer over a method name. This design requires you to implement
these four interfaces:

-   source interface

-   source provider interface

-   controller interface

-   controller provider interface

The source and controller providers are Managed Extensibility Framework (MEF)
component parts, and are responsible for exporting the source and controller
classes and importing services and brokers such as the
ITextBufferFactoryService, which creates the tooltip text buffer, and the
IQuickInfoBroker, which triggers the QuickInfo session.

In this example, the QuickInfo source uses a hard-coded list of method names and
descriptions, but in full implementations, the language service and the language
documentation are responsible for providing that content.

## Prerequisites

Starting in Visual Studio 2015, you don't need to install the Visual Studio SDK
from the download center. It's included as an optional feature in Visual Studio
setup. You can also install the VS SDK later on. For more information, see
[Install the Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2022).

## Create a MEF project

To create a MEF project

Create a C\# VSIX project. (In the New Project dialog, select Visual C\# /
Extensibility, then VSIX Project.) Name the solution QuickInfoTest.

Add an Editor Classifier item template to the project. For more information, see
[Create an extension with an editor item
template](https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-an-editor-item-template?view=vs-2022).

Delete the existing class files.

## Implement the QuickInfo source

The QuickInfo source is responsible for collecting the set of identifiers and
their descriptions and adding the content to the tooltip text buffer when one of
the identifiers is encountered. In this example, the identifiers and their
descriptions are just added in the source constructor.

To implement the QuickInfo source

-   Add a class file and name it TestQuickInfoSource.

-   Add a reference to *Microsoft.VisualStudio.Language.IntelliSense*.

-   Add the following imports.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Declare a class that implements IQuickInfoSource, and name it
TestQuickInfoSource.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal class TestQuickInfoSource : IQuickInfoSource
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add fields for the QuickInfo source provider, the text buffer, and a set of
method names and method signatures. In this example, the method names and
signatures are initialized in the TestQuickInfoSource constructor.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private TestQuickInfoSourceProvider m_provider;
private ITextBuffer m_subjectBuffer;
private Dictionary<string, string> m_dictionary;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a constructor that sets the QuickInfo source provider and the text buffer,
and populates the set of method names, and method signatures and descriptions.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public TestQuickInfoSource(TestQuickInfoSourceProvider provider, ITextBuffer subjectBuffer)
{
    m_provider = provider;
    m_subjectBuffer = subjectBuffer;

    //these are the method names and their descriptions
    m_dictionary = new Dictionary<string, string>();
    m_dictionary.Add("add", "int add(int firstInt, int secondInt)\nAdds one integer to another.");
    m_dictionary.Add("subtract", "int subtract(int firstInt, int secondInt)\nSubtracts one integer from another.");
    m_dictionary.Add("multiply", "int multiply(int firstInt, int secondInt)\nMultiplies one integer by another.");
    m_dictionary.Add("divide", "int divide(int firstInt, int secondInt)\nDivides one integer by another.");
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the AugmentQuickInfoSession method. In this example, the method finds
the current word, or the previous word if the cursor is at the end of a line or
a text buffer. If the word is one of the method names, the description for that
method name is added to the QuickInfo content.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> qiContent, out ITrackingSpan applicableToSpan)
{
    // Map the trigger point down to our buffer.
    SnapshotPoint? subjectTriggerPoint = session.GetTriggerPoint(m_subjectBuffer.CurrentSnapshot);
    if (!subjectTriggerPoint.HasValue)
    {
        applicableToSpan = null;
        return;
    }

    ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;
    SnapshotSpan querySpan = new SnapshotSpan(subjectTriggerPoint.Value, 0);

    //look for occurrences of our QuickInfo words in the span
    ITextStructureNavigator navigator = m_provider.NavigatorService.GetTextStructureNavigator(m_subjectBuffer);
    TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
    string searchText = extent.Span.GetText();

    foreach (string key in m_dictionary.Keys)
    {
        int foundIndex = searchText.IndexOf(key, StringComparison.CurrentCultureIgnoreCase);
        if (foundIndex > -1)
        {
            applicableToSpan = currentSnapshot.CreateTrackingSpan
                (
                //querySpan.Start.Add(foundIndex).Position, 9, SpanTrackingMode.EdgeInclusive
                                        extent.Span.Start + foundIndex, key.Length, SpanTrackingMode.EdgeInclusive
                );

            string value;
            m_dictionary.TryGetValue(key, out value);
            if (value != null)
                qiContent.Add(value);
            else
                qiContent.Add("");

            return;
        }
    }

    applicableToSpan = null;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

You must also implement a Dispose() method, since IQuickInfoSource implements
IDisposable:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private bool m_isDisposed;
public void Dispose()
{
    if (!m_isDisposed)
    {
        GC.SuppressFinalize(this);
        m_isDisposed = true;
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Implement a QuickInfo source provider

The provider of the QuickInfo source serves primarily to export itself as a MEF
component part and instantiate the QuickInfo source. Because it's a MEF
component part, it can import other MEF component parts.

To implement a QuickInfo source provider

Declare a QuickInfo source provider named TestQuickInfoSourceProvider that
implements IQuickInfoSourceProvider, and export it with a NameAttribute of
"ToolTip QuickInfo Source", an OrderAttribute of Before="default", and a
ContentTypeAttribute of "text".

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IQuickInfoSourceProvider))]
[Name("ToolTip QuickInfo Source")]
[Order(Before = "Default Quick Info Presenter")]
[ContentType("text")]
internal class TestQuickInfoSourceProvider : IQuickInfoSourceProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Import two editor services,
[ITextStructureNavigatorSelectorService](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.operations.itextstructurenavigatorselectorservice)
and
[ITextBufferFactoryService](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.itextbufferfactoryservice),
as properties of TestQuickInfoSourceProvider.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

[Import]
internal ITextBufferFactoryService TextBufferFactoryService { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement TryCreateQuickInfoSource to return a new TestQuickInfoSource.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
{
    return new TestQuickInfoSource(this, textBuffer);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Implement a QuickInfo controller

QuickInfo controllers determine when QuickInfo is displayed. In this example,
QuickInfo appears when the pointer is over a word that corresponds to one of the
method names. The QuickInfo controller implements a mouse hover event handler
that triggers a QuickInfo session.

To implement a QuickInfo controller

Declare a class that implements IIntellisenseController, and name it
TestQuickInfoController.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal class TestQuickInfoController : IIntellisenseController
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add private fields for the text view, the text buffers represented in the text
view, the QuickInfo session, and the QuickInfo controller provider.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private ITextView m_textView;
private IList<ITextBuffer> m_subjectBuffers;
private TestQuickInfoControllerProvider m_provider;
private IQuickInfoSession m_session;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a constructor that sets the fields and adds the mouse hover event handler.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal TestQuickInfoController(ITextView textView, IList<ITextBuffer> subjectBuffers, TestQuickInfoControllerProvider provider)
{
    m_textView = textView;
    m_subjectBuffers = subjectBuffers;
    m_provider = provider;

    m_textView.MouseHover += this.OnTextViewMouseHover;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add the mouse hover event handler that triggers the QuickInfo session.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private void OnTextViewMouseHover(object sender, MouseHoverEventArgs e)
{
    //find the mouse position by mapping down to the subject buffer
    SnapshotPoint? point = m_textView.BufferGraph.MapDownToFirstMatch
         (new SnapshotPoint(m_textView.TextSnapshot, e.Position),
        PointTrackingMode.Positive,
        snapshot => m_subjectBuffers.Contains(snapshot.TextBuffer),
        PositionAffinity.Predecessor);

    if (point != null)
    {
        ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(point.Value.Position,
        PointTrackingMode.Positive);

        if (!m_provider.QuickInfoBroker.IsQuickInfoActive(m_textView))
        {
            m_session = m_provider.QuickInfoBroker.TriggerQuickInfo(m_textView, triggerPoint, true);
        }
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the
[Detach](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.iintellisensecontroller.detach)
method so that it removes the mouse hover event handler when the controller is
detached from the text view.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public void Detach(ITextView textView)
{
    if (m_textView == textView)
    {
        m_textView.MouseHover -= this.OnTextViewMouseHover;
        m_textView = null;
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the ConnectSubjectBuffer method and the DisconnectSubjectBuffer method
as empty methods for this example.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
{
}

public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
{
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Implementing the QuickInfo controller provider

The provider of the QuickInfo controller serves primarily to export itself as a
MEF component part and instantiate the QuickInfo controller. Because it's a MEF
component part, it can import other MEF component parts.

To implement the QuickInfo controller provider

Declare a class named TestQuickInfoControllerProvider that implements
IIntellisenseControllerProvider, and export it with a NameAttribute of "ToolTip
QuickInfo Controller" and a ContentTypeAttribute of "text":

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IIntellisenseControllerProvider))]
[Name("ToolTip QuickInfo Controller")]
[ContentType("text")]
internal class TestQuickInfoControllerProvider : IIntellisenseControllerProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Import the IQuickInfoBroker as a property.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal IQuickInfoBroker QuickInfoBroker { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the TryCreateIntellisenseController method by instantiating the
QuickInfo controller.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
{
    return new TestQuickInfoController(textView, subjectBuffers, this);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Build and test the Code

To test this code, build the QuickInfoTest solution and run it in the
experimental instance.

To build and test the QuickInfoTest solution

Build the solution.

When you run this project in the debugger, a second instance of Visual Studio is
started.

Create a text file and type some text that includes the words "add" and
"subtract".

Move the pointer over one of the occurrences of "add". The signature and the
description of the add method should be displayed.

## See also

[Walkthrough: Link a content type to a file name
extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-linking-a-content-type-to-a-file-name-extension?view=vs-2022)
