# Walkthrough: Implement code snippets


<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Prerequisites](#Prerequisites)
   - [Create and register code snippets](#Create-and-register-code-snippets)
   - [Add the Insert Snippet command to the shortcut menu](#Add-the-Insert-Snippet-command-to-the-shortcut-menu)
   - [Implement snippet expansion in the Snippet Picker UI](#Implement-snippet-expansion-in-the-Snippet-Picker-UI)
   - [Build and test code snippet expansion](#Build-and-test-code-snippet-expansion)
<!--End-Of-TOC-->

## Article

08/05/2021

You can create code snippets and include them in an editor extension so that
users of the extension can add them to their own code.

A code snippet is a fragment of code or other text that can be incorporated in a
file. To view all snippets that have been registered for particular programming
languages, on the Tools menu, click Code Snippet Manager. To insert a snippet in
a file, right-click where you want the snippet, click Insert Snippet, or
Surround With, locate the snippet you want, and then double-click it. Press Tab
or Shift+Tab to modify the relevant parts of the snippet and then press Enter or
Esc to accept it. For more information, see [Code
snippets](https://docs.microsoft.com/en-us/visualstudio/ide/code-snippets?view=vs-2022).

A code snippet is contained in an XML file that has the .snippet\* file name
extension. A snippet can contain fields that are highlighted after the snippet
is inserted so that the user can find and change them. A snippet file also
provides information for the Code Snippet Manager so that it can display the
snippet name in the correct category. For information about the snippet schema,
see [Code snippets schema
reference](https://docs.microsoft.com/en-us/visualstudio/ide/code-snippets-schema-reference?view=vs-2022).

This walkthrough teaches how to accomplish these tasks:

Create and register code snippets for a specific language.

Add the Insert Snippet command to a shortcut menu.

Implement snippet expansion.

This walkthrough is based on [Walkthrough: Display statement
completion](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-statement-completion?view=vs-2022).

## Prerequisites

Starting in Visual Studio 2015, you don't install the Visual Studio SDK from the
download center. It's included as an optional feature in Visual Studio setup.
You can also install the VS SDK later on. For more information, see [Install the
Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2022).

## Create and register code snippets

Typically, code snippets are associated with a registered language service.
However, you do not have to implement a
[LanguageService](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.languageservice)
to register code snippets. Instead, just specify a GUID in the snippet index
file and then use the same GUID in the
[ProvideLanguageCodeExpansionAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.providelanguagecodeexpansionattribute)
that you add to your project.

The following steps demonstrate how to create code snippets and associate them
with a specific GUID.

Create the following directory structure:

%InstallDir%\\TestSnippets\\Snippets\\1033\\

where *%InstallDir%* is the Visual Studio installation folder. (Although this
path is typically used to install code snippets, you can specify any path.)

In the \\1033\\ folder, create an *.xml* file and name it TestSnippets.xml.
(Although this name is typically used for a snippet index file, you can specify
any name as long as it has an *.xml* file name extension.) Add the following
text, and then delete the placeholder GUID and add your own.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ XML
<?xml version="1.0" encoding="utf-8" ?>
<SnippetCollection>
    <Language Lang="TestSnippets" Guid="{00000000-0000-0000-0000-000000000000}">
        <SnippetDir>
            <OnOff>On</OnOff>
            <Installed>true</Installed>
            <Locale>1033</Locale>
            <DirPath>%InstallRoot%\TestSnippets\Snippets\%LCID%\</DirPath>
            <LocalizedName>Snippets</LocalizedName>
        </SnippetDir>
    </Language>
</SnippetCollection>
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Create a file in the snippet folder, name it test.snippet, and then add the
following text:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ XML
<?xml version="1.0" encoding="utf-8" ?>
<CodeSnippets  xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">
    <CodeSnippet Format="1.0.0">
        <Header>
            <Title>Test replacement fields</Title>
            <Shortcut>test</Shortcut>
            <Description>Code snippet for testing replacement fields</Description>
            <Author>MSIT</Author>
            <SnippetTypes>
                <SnippetType>Expansion</SnippetType>
            </SnippetTypes>
        </Header>
        <Snippet>
            <Declarations>
                <Literal>
                  <ID>param1</ID>
                    <ToolTip>First field</ToolTip>
                    <Default>first</Default>
                </Literal>
                <Literal>
                    <ID>param2</ID>
                    <ToolTip>Second field</ToolTip>
                    <Default>second</Default>
                </Literal>
            </Declarations>
            <References>
               <Reference>
                   <Assembly>System.Windows.Forms.dll</Assembly>
               </Reference>
            </References>
            <Code Language="TestSnippets">
                <![CDATA[MessageBox.Show("$param1$");
     MessageBox.Show("$param2$");]]>
            </Code>
        </Snippet>
    </CodeSnippet>
</CodeSnippets>
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The following steps show how to register the code snippets.

To register code snippets for a specific GUID

Open the CompletionTest project. For information about how to create this
project, see [Walkthrough: Display statement
completion](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-statement-completion?view=vs-2022).

In the project, add references to the following assemblies:

-   Microsoft.VisualStudio.TextManager.Interop

-   Microsoft.VisualStudio.TextManager.Interop.8.0

-   microsoft.msxml

In the project, open the source.extension.vsixmanifest file.

Make sure that the Assets tab contains a VsPackage content type and that Project
is set to the name of the project.

Select the CompletionTest project and in the Properties window set Generate
Pkgdef File to true. Save the project.

Add a static SnippetUtilities class to the project.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
static class SnippetUtilities
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the SnippetUtilities class, define a GUID and give it the value that you used
in the *SnippetsIndex.xml* file.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal const string LanguageServiceGuidStr = "00000000-0000-0000-0000-00000000";
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add the
[ProvideLanguageCodeExpansionAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.providelanguagecodeexpansionattribute)
to the TestCompletionHandler class. This attribute can be added to any public or
internal (non-static) class in the project. (You may have to add a using
directive for the Microsoft.VisualStudio.Shell namespace.)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[ProvideLanguageCodeExpansion(
SnippetUtilities.LanguageServiceGuidStr,
"TestSnippets", //the language name
0,              //the resource id of the language
"TestSnippets", //the language ID used in the .snippet files
@"%InstallRoot%\TestSnippets\Snippets\%LCID%\TestSnippets.xml",
    //the path of the index file
SearchPaths = @"%InstallRoot%\TestSnippets\Snippets\%LCID%\",
ForceCreateDirs = @"%InstallRoot%\TestSnippets\Snippets\%LCID%\")]
internal class TestCompletionCommandHandler : IOleCommandTarget
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Build and run the project. In the experimental instance of Visual Studio that
starts when the project is run, the snippet you just registered should be
displayed in the Code Snippets Manager under the TestSnippets language.

## Add the Insert Snippet command to the shortcut menu

The Insert Snippet command is not included on the shortcut menu for a text file.
Therefore, you must enable the command.

To add the Insert Snippet command to the shortcut menu

Open the TestCompletionCommandHandler class file.

Because this class implements
[IOleCommandTarget](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.ole.interop.iolecommandtarget),
you can activate the Insert Snippet command in the
[QueryStatus](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.ole.interop.iolecommandtarget.querystatus)
method. Before you enable the command, check that this method is not being
called inside an automation function because when the Insert Snippet command is
clicked, it displays the snippet picker user interface (UI).

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
{
    if (!VsShellUtilities.IsInAutomationFunction(m_provider.ServiceProvider))
    {
        if (pguidCmdGroup == VSConstants.VSStd2K && cCmds > 0)
        {
            // make the Insert Snippet command appear on the context menu 
            if ((uint)prgCmds[0].cmdID == (uint)VSConstants.VSStd2KCmdID.INSERTSNIPPET)
            {
                prgCmds[0].cmdf = (int)Constants.MSOCMDF_ENABLED | (int)Constants.MSOCMDF_SUPPORTED;
                return VSConstants.S_OK;
            }
        }
    }

    return m_nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Build and run the project. In the experimental instance, open a file that has
the *.zzz* file name extension and then right-click anywhere in it. The Insert
Snippet command should appear on the shortcut menu.

## Implement snippet expansion in the Snippet Picker UI

This section shows how to implement code snippet expansion so that the snippet
picker UI is displayed when Insert Snippet is clicked on the shortcut menu. A
code snippet is also expanded when a user types the code-snippet shortcut and
then presses Tab.

To display the snippet picker UI and to enable navigation and post-insertion
snippet acceptance, use the
[Exec](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.ole.interop.iolecommandtarget.exec)
method. The insertion itself is handled by the
[OnItemChosen](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansionclient.onitemchosen)
method.

The implementation of code snippet expansion uses legacy
[Microsoft.VisualStudio.TextManager.Interop](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop)
interfaces. When you translate from the current editor classes to the legacy
code, remember that the legacy interfaces use a combination of line numbers and
column numbers to specify locations in a text buffer, but the current classes
use one index. Therefore, if a buffer has three lines each of which has 10
characters (plus a newline, which counts as one character), the fourth character
on the third line is at position 27 in the current implementation, but it is at
line 2, position 3 in the old implementation.

To implement snippet expansion

To the file that contains the TestCompletionCommandHandler class, add the
following using directives.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using Microsoft.VisualStudio.Text.Operations;
using MSXML;
using System.ComponentModel.Composition;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Make the TestCompletionCommandHandler class implement the
[IVsExpansionClient](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansionclient)
interface.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal class TestCompletionCommandHandler : IOleCommandTarget, IVsExpansionClient
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the TestCompletionCommandHandlerProvider class, import the
[ITextStructureNavigatorSelectorService](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.operations.itextstructurenavigatorselectorservice).

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Add some private fields for the code expansion interfaces and the
[IVsTextView](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivstextview).

-   IVsTextView m_vsTextView;

-   IVsExpansionManager m_exManager;

-   IVsExpansionSession m_exSession;

In the constructor of the TestCompletionCommandHandler class, set the following
fields.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal TestCompletionCommandHandler(IVsTextView textViewAdapter, ITextView textView, TestCompletionHandlerProvider provider)
{
    this.m_textView = textView;
    m_vsTextView = textViewAdapter;
    m_provider = provider;
    //get the text manager from the service provider
    IVsTextManager2 textManager = (IVsTextManager2)m_provider.ServiceProvider.GetService(typeof(SVsTextManager));
    textManager.GetExpansionManager(out m_exManager);
    m_exSession = null;

    //add the command to the command chain
    textViewAdapter.AddCommandFilter(this, out m_nextCommandHandler);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

To display the snippet picker when the user clicks the Insert Snippet command,
add the following code to the
[Exec](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.ole.interop.iolecommandtarget.exec)
method. (To make this explanation more readable, the Exec()code that's used for
statement completion is not shown; instead, blocks of code are added to the
existing method.) Add the following block of code after the code that checks for
a character.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
//code previously written for Exec
if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
{
    typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
}
//the snippet picker code starts here
if (nCmdID == (uint)VSConstants.VSStd2KCmdID.INSERTSNIPPET)
{
    IVsTextManager2 textManager = (IVsTextManager2)m_provider.ServiceProvider.GetService(typeof(SVsTextManager));

    textManager.GetExpansionManager(out m_exManager);

    m_exManager.InvokeInsertionUI(
        m_vsTextView,
        this,      //the expansion client
        new Guid(SnippetUtilities.LanguageServiceGuidStr),
        null,       //use all snippet types
        0,          //number of types (0 for all)
        0,          //ignored if iCountTypes == 0
        null,       //use all snippet kinds
        0,          //use all snippet kinds
        0,          //ignored if iCountTypes == 0
        "TestSnippets", //the text to show in the prompt
        string.Empty);  //only the ENTER key causes insert 

    return VSConstants.S_OK;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

If a snippet has fields that can be navigated, the expansion session is kept
open until the expansion is explicitly accepted; if the snippet has no fields,
the session is closed and is returned as null by the
[InvokeInsertionUI](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansionmanager.invokeinsertionui)
method. In the
[Exec](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.ole.interop.iolecommandtarget.exec)
method, after the snippet picker UI code that you added in the previous step,
add the following code to handle snippet navigation (when the user presses Tab
or Shift+Tab after snippet insertion).

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
//the expansion insertion is handled in OnItemChosen
//if the expansion session is still active, handle tab/backtab/return/cancel
if (m_exSession != null)
{
    if (nCmdID == (uint)VSConstants.VSStd2KCmdID.BACKTAB)
    {
        m_exSession.GoToPreviousExpansionField();
        return VSConstants.S_OK;
    }
    else if (nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB)
    {

        m_exSession.GoToNextExpansionField(0); //false to support cycling through all the fields
        return VSConstants.S_OK;
    }
    else if (nCmdID == (uint)VSConstants.VSStd2KCmdID.RETURN || nCmdID == (uint)VSConstants.VSStd2KCmdID.CANCEL)
    {
        if (m_exSession.EndCurrentExpansion(0) == VSConstants.S_OK)
        {
            m_exSession = null;
            return VSConstants.S_OK;
        }
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

To insert the code snippet when the user types the corresponding shortcut and
then presses Tab, add code to the
[Exec](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.ole.interop.iolecommandtarget.exec)
method. The private method that inserts the snippet will be shown in a later
step. Add the following code after the navigation code that you added in the
previous step.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
//neither an expansion session nor a completion session is open, but we got a tab, so check whether the last word typed is a snippet shortcut 
if (m_session == null && m_exSession == null && nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB)
{
    //get the word that was just added 
    CaretPosition pos = m_textView.Caret.Position;
    TextExtent word = m_provider.NavigatorService.GetTextStructureNavigator(m_textView.TextBuffer).GetExtentOfWord(pos.BufferPosition - 1); //use the position 1 space back
    string textString = word.Span.GetText(); //the word that was just added
    //if it is a code snippet, insert it, otherwise carry on
    if (InsertAnyExpansion(textString, null, null))
        return VSConstants.S_OK;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the methods of the
[IVsExpansionClient](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansionclient)
interface. In this implementation, the only methods of interest are
[EndExpansion](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansionclient.endexpansion)
and
[OnItemChosen](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansionclient.onitemchosen).
The other methods should just return
[S_OK](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.vsconstants.s_ok).

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public int EndExpansion()
{
    m_exSession = null;
    return VSConstants.S_OK;
}

public int FormatSpan(IVsTextLines pBuffer, TextSpan[] ts)
{
    return VSConstants.S_OK;
}

public int GetExpansionFunction(IXMLDOMNode xmlFunctionNode, string bstrFieldName, out IVsExpansionFunction pFunc)
{
    pFunc = null;
    return VSConstants.S_OK;
}

public int IsValidKind(IVsTextLines pBuffer, TextSpan[] ts, string bstrKind, out int pfIsValidKind)
{
    pfIsValidKind = 1;
    return VSConstants.S_OK;
}

public int IsValidType(IVsTextLines pBuffer, TextSpan[] ts, string[] rgTypes, int iCountTypes, out int pfIsValidType)
{
    pfIsValidType = 1;
    return VSConstants.S_OK;
}

public int OnAfterInsertion(IVsExpansionSession pSession)
{
    return VSConstants.S_OK;
}

public int OnBeforeInsertion(IVsExpansionSession pSession)
{
    return VSConstants.S_OK;
}

public int PositionCaretForEditing(IVsTextLines pBuffer, TextSpan[] ts)
{
    return VSConstants.S_OK;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Implement the
[OnItemChosen](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansionclient.onitemchosen)
method. The helper method that actually inserts the expansions is covered in a
later step. The
[TextSpan](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.textspan)
provides line and column information, which you can get from the
[IVsTextView](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivstextview).

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public int OnItemChosen(string pszTitle, string pszPath)
{
    InsertAnyExpansion(null, pszTitle, pszPath);
    return VSConstants.S_OK;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The following private method inserts a code snippet, based either on the
shortcut or on the title and path. It then calls the
[InsertNamedExpansion](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansion.insertnamedexpansion)
method with the snippet.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
private bool InsertAnyExpansion(string shortcut, string title, string path)
{
    //first get the location of the caret, and set up a TextSpan
    int endColumn, startLine;
    //get the column number from  the IVsTextView, not the ITextView
    m_vsTextView.GetCaretPos(out startLine, out endColumn);

    TextSpan addSpan = new TextSpan();
    addSpan.iStartIndex = endColumn;
    addSpan.iEndIndex = endColumn;
    addSpan.iStartLine = startLine;
    addSpan.iEndLine = startLine;

    if (shortcut != null) //get the expansion from the shortcut
    {
        //reset the TextSpan to the width of the shortcut, 
        //because we're going to replace the shortcut with the expansion
        addSpan.iStartIndex = addSpan.iEndIndex - shortcut.Length;

        m_exManager.GetExpansionByShortcut(
            this,
            new Guid(SnippetUtilities.LanguageServiceGuidStr),
            shortcut,
            m_vsTextView,
            new TextSpan[] { addSpan },
            0,
            out path,
            out title);

    }
    if (title != null && path != null)
    {
        IVsTextLines textLines;
        m_vsTextView.GetBuffer(out textLines);
        IVsExpansion bufferExpansion = (IVsExpansion)textLines;

        if (bufferExpansion != null)
        {
            int hr = bufferExpansion.InsertNamedExpansion(
                title,
                path,
                addSpan,
                this,
                new Guid(SnippetUtilities.LanguageServiceGuidStr),
                0,
               out m_exSession);
            if (VSConstants.S_OK == hr)
            {
                return true;
            }
        }
    }
    return false;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Build and test code snippet expansion

You can test whether snippet expansion works in your project.

Build the solution. When you run this project in the debugger, a second instance
of Visual Studio is started.

Open a text file and type some text.

Right-click somewhere in the text and then click Insert Snippet.

The snippet picker UI should appear with a pop-up that says Test replacement
fields. Double-click the pop-up.

The following snippet should be inserted.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
MessageBox.Show("first");
MessageBox.Show("second");
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Don't press Enter or Esc.

Press Tab and Shift+Tab to toggle between "first" and "second".

Accept the insertion by pressing either Enter or Esc.

In a different part of the text, type "test" and then press Tab. Because "test"
is the code-snippet shortcut, the snippet should be inserted again.
