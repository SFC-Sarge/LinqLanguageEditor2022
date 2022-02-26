### [Walkthrough: Create Custom Language Editor](#Walkthrough-Create-Custom-Language-Editor)

The walkthrough will show you how to create a Custom Language Editor.

The Editor features are:

- Code Syntax colorzation
- IntelliSense
- ToolWindow
    - Toolbar in ToolWindow
    - Toolbar button in ToolWindow
    - ToolWindow Messenger Support
- Select LINQ Queries and create new temp file, display in temp view tab, and return query results in ToolWindow
    - Using LPRun7-x64.exe and LINQPad.Runtime.dll to compile the LINQ query and return the results.
    - Single line LINQ Query selection and result support.
    - LINQ Query Method selection and result support.
- LINQ language file extension .linq
- IVsRunningDocTableEvents document events support
    - OnBeforeDocumentWindowShow (Before .linq document is displayed in tabbed documents view.)
    - OnAfterDocumentWindowHide (When .linq document is removed from tabbed documents view.)
- Code Formatting
- Light Bulb Suggestions
- Tools Options and Settings Support


## [Create Visual Studio 2022 C# Extension](#Create-Visual-Studio-2022-Extension)

Using the [VsixCommunity Community.VisualStudio.Toolkit](https://github.com/VsixCommunity/Community.VisualStudio.Toolkit) VSIX Project w/Tool Window (Community) Project Template

![VSIX Project w/Tool Window (Community) Project Template](ToolWindowTemplate.png)


Name the extension project LinqLanguageEditor2022



## [Download Full Source Code](#Download-Full-Source-Code)

Get the source for this walkthrough from: [LinqLanguageEditor2022](https://github.com/SFC-Sarge/LinqLanguageEditor2022)