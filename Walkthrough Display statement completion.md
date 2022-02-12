# Walkthrough: Display statement completion


<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Prerequisites](#Prerequisites)
   - [Create a MEF Project](#Create-a-MEF-Project)
   - [Implement the completion source](#Implement-the-completion-source)
   - [Implement the completion source provider](#Implement-the-completion-source-provider)
   - [Implement the completion command handler provider](#Implement-the-completion-command-handler-provider)
   - [Implement the completion command handler](#Implement-the-completion-command-handler)
   - [Build and test the code](#Build-and-test-the-code)
   - [See also](#See-also)
<!--End-Of-TOC-->

## Article


08/05/2021

You can implement language-based statement completion by defining the
identifiers for which you want to provide completion and then triggering a
completion session. You can define statement completion in the context of a
language service, define your own file name extension and content type and then
display completion for just that type. Or, you can trigger completion for an
existing content type—for example, "plaintext". This walkthrough shows how to
trigger statement completion for the "plaintext" content type, which is the
content type of text files. The "text" content type is the ancestor of all other
content types, including code and XML files.

Statement completion is typically triggered by typing certain characters—for
example, by typing the beginning of an identifier such as "using". It's
typically dismissed by pressing the Spacebar, Tab, or Enter key to commit a
selection. You can implement the IntelliSense features that trigger when typing
a character by using a command handler for the keystrokes (the IOleCommandTarget
interface) and a handler provider that implements the
IVsTextViewCreationListener interface. To create the completion source, which is
the list of identifiers that participate in completion, implement the
ICompletionSource interface and a completion source provider (the
ICompletionSourceProvider interface). The providers are Managed Extensibility
Framework (MEF) component parts. They're responsible for exporting the source
and controller classes and importing services and brokers—for example, the
ITextStructureNavigatorSelectorService, which enables navigation in the text
buffer, and the ICompletionBroker, which triggers the completion session.

This walkthrough shows how to implement statement completion for a hard-coded
set of identifiers. In full implementations, the language service and the
language documentation are responsible for providing that content.

## Prerequisites

Starting in Visual Studio 2015, you don't install the Visual Studio SDK from the
download center. It's included as an optional feature in Visual Studio setup.
You can also install the VS SDK later on. For more information, see [Install the
Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2022).

## Create a MEF Project

To create a MEF project

Create a C\# VSIX project. (In the New Project dialog, select Visual C\# /
Extensibility, then VSIX Project.) Name the solution CompletionTest.

Add an Editor Classifier item template to the project. For more information, see
[Create an extension with an editor item
template](https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-an-editor-item-template?view=vs-2022).

Delete the existing class files.

Add the following references to the project and make sure that CopyLocal is set
to false:

-   Microsoft.VisualStudio.Editor

-   Microsoft.VisualStudio.Language.Intellisense

-   Microsoft.VisualStudio.OLE.Interop

-   Microsoft.VisualStudio.Shell.17.0

-   Microsoft.VisualStudio.Shell.Immutable.17.0

-   Microsoft.VisualStudio.TextManager.Interop

## Implement the completion source

The completion source is responsible for collecting the set of identifiers and
adding the content to the completion window when a user types a completion
trigger, such as the first letters of an identifier. In this example, the
identifiers and their descriptions are hard-coded in the
AugmentCompletionSession method. In most real-world uses, you would use your
language's parser to get the tokens to populate the completion list.

To implement the completion source

Add a class file and name it TestCompletionSource.

Add these imports:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Modify the class declaration for TestCompletionSource so that it implements
ICompletionSource:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal class TestCompletionSource : ICompletionSource
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add private fields for the source provider, the text buffer, and a list of
[Completion](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.completion)
objects (which correspond to the identifiers that will participate in the
completion session):

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private TestCompletionSourceProvider m_sourceProvider;
private ITextBuffer m_textBuffer;
private List<Completion> m_compList;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a constructor that sets the source provider and buffer. The
TestCompletionSourceProvider class is defined in later steps:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public TestCompletionSource(TestCompletionSourceProvider sourceProvider, ITextBuffer textBuffer)
{
    m_sourceProvider = sourceProvider;
    m_textBuffer = textBuffer;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the AugmentCompletionSession method by adding a completion set that
contains the completions you want to provide in the context. Each completion set
contains a set of
[Completion](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.completion)
completions, and corresponds to a tab of the completion window. (In Visual Basic
projects, the completion window tabs are named Common and All.) The
FindTokenSpanAtPosition method is defined in the next step.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
void ICompletionSource.AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
{
    List<string> strList = new List<string>();
    strList.Add("addition");
    strList.Add("adaptation");
    strList.Add("subtraction");
    strList.Add("summation");
    m_compList = new List<Completion>();
    foreach (string str in strList)
        m_compList.Add(new Completion(str, str, str, null, null));

    completionSets.Add(new CompletionSet(
        "Tokens",    //the non-localized title of the tab
        "Tokens",    //the display title of the tab
        FindTokenSpanAtPosition(session.GetTriggerPoint(m_textBuffer),
            session),
        m_compList,
        null));
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The following method is used to find the current word from the position of the
cursor:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
{
    SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
    ITextStructureNavigator navigator = m_sourceProvider.NavigatorService.GetTextStructureNavigator(m_textBuffer);
    TextExtent extent = navigator.GetExtentOfWord(currentPoint);
    return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the Dispose() method:

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

## Implement the completion source provider

The completion source provider is the MEF component part that instantiates the
completion source.

To implement the completion source provider

Add a class named TestCompletionSourceProvider that implements
ICompletionSourceProvider. Export this class with a ContentTypeAttribute of
"plaintext" and a NameAttribute of "test completion".

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(ICompletionSourceProvider))]
[ContentType("plaintext")]
[Name("token completion")]
internal class TestCompletionSourceProvider : ICompletionSourceProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Import a ITextStructureNavigatorSelectorService, which finds the current word in
the completion source.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the TryCreateCompletionSource method to instantiate the completion
source.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
{
    return new TestCompletionSource(this, textBuffer);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Implement the completion command handler provider

The completion command handler provider is derived from a
IVsTextViewCreationListener, which listens for a text view creation event and
converts the view from an IVsTextView—which enables the addition of the command
to the command chain of the Visual Studio shell—to an ITextView. Because this
class is a MEF export, you can also use it to import the services that are
required by the command handler itself.

To implement the completion command handler provider

Add a file named TestCompletionCommandHandler.

Add these using directives:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a class named TestCompletionHandlerProvider that implements
IVsTextViewCreationListener. Export this class with a NameAttribute of "token
completion handler", a ContentTypeAttribute of "plaintext", and a
TextViewRoleAttribute of
[Editable](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.predefinedtextviewroles.editable).

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IVsTextViewCreationListener))]
[Name("token completion handler")]
[ContentType("plaintext")]
[TextViewRole(PredefinedTextViewRoles.Editable)]
internal class TestCompletionHandlerProvider : IVsTextViewCreationListener
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Import the IVsEditorAdaptersFactoryService, which enables conversion from a
IVsTextView to a ITextView, a ICompletionBroker, and a SVsServiceProvider that
enables access to standard Visual Studio services.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal IVsEditorAdaptersFactoryService AdapterService = null;
[Import]
internal ICompletionBroker CompletionBroker { get; set; }
[Import]
internal SVsServiceProvider ServiceProvider { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the VsTextViewCreated method to instantiate the command handler.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public void VsTextViewCreated(IVsTextView textViewAdapter)
{
    ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
    if (textView == null)
        return;

    Func<TestCompletionCommandHandler> createCommandHandler = delegate() { return new TestCompletionCommandHandler(textViewAdapter, textView, this); };
    textView.Properties.GetOrCreateSingletonProperty(createCommandHandler);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Implement the completion command handler

Because statement completion is triggered by keystrokes, you must implement the
IOleCommandTarget interface to receive and process the keystrokes that trigger,
commit, and dismiss the completion session.

To implement the completion command handler

Add a class named TestCompletionCommandHandler that implements
IOleCommandTarget:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal class TestCompletionCommandHandler : IOleCommandTarget
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add private fields for the next command handler (to which you pass the command),
the text view, the command handler provider (which enables access to various
services), and a completion session:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private IOleCommandTarget m_nextCommandHandler;
private ITextView m_textView;
private TestCompletionHandlerProvider m_provider;
private ICompletionSession m_session;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add a constructor that sets the text view and the provider fields, and adds the
command to the command chain:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal TestCompletionCommandHandler(IVsTextView textViewAdapter, ITextView textView, TestCompletionHandlerProvider provider)
{
    this.m_textView = textView;
    this.m_provider = provider;

    //add the command to the command chain
    textViewAdapter.AddCommandFilter(this, out m_nextCommandHandler);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the QueryStatus method by passing the command along:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
{
    return m_nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the
[Exec](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.ole.interop.iolecommandtarget.exec)
method. When this method receives a keystroke, it must do one of these things:

Allow the character to be written to the buffer, and then trigger or filter
completion. (Printing characters do this.)

Commit the completion, but do not allow the character to be written to the
buffer. (Whitespace, Tab, and Enter do this when a completion session is
displayed.)

Allow the command to be passed on to the next handler. (All other commands.)

Because this method may display UI, call IsInAutomationFunction to make sure
that it's not called in an automation context:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
{
    if (VsShellUtilities.IsInAutomationFunction(m_provider.ServiceProvider))
    {
        return m_nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
    }
    //make a copy of this so we can look at it after forwarding some commands
    uint commandID = nCmdID;
    char typedChar = char.MinValue;
    //make sure the input is a char before getting it
    if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
    {
        typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
    }

    //check for a commit character
    if (nCmdID == (uint)VSConstants.VSStd2KCmdID.RETURN
        || nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB
        || (char.IsWhiteSpace(typedChar) || char.IsPunctuation(typedChar)))
    {
        //check for a selection
        if (m_session != null && !m_session.IsDismissed)
        {
            //if the selection is fully selected, commit the current session
            if (m_session.SelectedCompletionSet.SelectionStatus.IsSelected)
            {
                m_session.Commit();
                //also, don't add the character to the buffer
                return VSConstants.S_OK;
            }
            else
            {
                //if there is no selection, dismiss the session
                m_session.Dismiss();
            }
        }
    }

    //pass along the command so the char is added to the buffer
    int retVal = m_nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
    bool handled = false;
    if (!typedChar.Equals(char.MinValue) && char.IsLetterOrDigit(typedChar))
    {
        if (m_session == null || m_session.IsDismissed) // If there is no active session, bring up completion
        {
            this.TriggerCompletion();
            m_session.Filter();
        }
        else    //the completion session is already active, so just filter
        {
            m_session.Filter();
        }
        handled = true;
    }
    else if (commandID == (uint)VSConstants.VSStd2KCmdID.BACKSPACE   //redo the filter if there is a deletion
        || commandID == (uint)VSConstants.VSStd2KCmdID.DELETE)
    {
        if (m_session != null && !m_session.IsDismissed)
            m_session.Filter();
        handled = true;
    }
    if (handled) return VSConstants.S_OK;
    return retVal;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

This code is a private method that triggers the completion session:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private bool TriggerCompletion()
{
    //the caret must be in a non-projection location 
    SnapshotPoint? caretPoint =
    m_textView.Caret.Position.Point.GetPoint(
    textBuffer => (!textBuffer.ContentType.IsOfType("projection")), PositionAffinity.Predecessor);
    if (!caretPoint.HasValue)
    {
        return false;
    }

    m_session = m_provider.CompletionBroker.CreateCompletionSession
 (m_textView,
        caretPoint.Value.Snapshot.CreateTrackingPoint(caretPoint.Value.Position, PointTrackingMode.Positive),
        true);

    //subscribe to the Dismissed event on the session 
    m_session.Dismissed += this.OnSessionDismissed;
    m_session.Start();

    return true;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The next example is a private method that unsubscribes from the
[Dismissed](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.iintellisensesession.dismissed)
event:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private void OnSessionDismissed(object sender, EventArgs e)
{
    m_session.Dismissed -= this.OnSessionDismissed;
    m_session = null;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Build and test the code

To test this code, build the CompletionTest solution and run it in the
experimental instance.

To build and test the CompletionTest solution

Build the solution.

When you run this project in the debugger, a second instance of Visual Studio is
started.

Create a text file and type some text that includes the word "add".

As you type first "a" and then "d", a list that contains "addition" and
"adaptation" should appear. Notice that addition is selected. When you type
another "d", the list should contain only "addition", which is now selected. You
can commit "addition" by pressing the Spacebar, Tab, or Enter key, or dismiss
the list by typing Esc or any other key.

## See also

[Walkthrough: Link a content type to a file name
extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-linking-a-content-type-to-a-file-name-extension?view=vs-2022)
