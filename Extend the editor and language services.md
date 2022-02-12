# Extend the editor and language services

<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Related Topics](#Related-Topics)
   - [Reference](#Reference)
<!--End-Of-TOC-->


## Article

08/05/2021

You can add language service features (such as IntelliSense) to your own editor,
and extend most features of the Visual Studio code editor. For a full list of
what you can extend, see [Language service and editor extension
points](https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2022).

You extend most editor features by using the Managed Extensibility Framework
(MEF). For example, if the editor feature you want to extend is syntax coloring,
you can write a MEF *component part* that defines the classifications for which
you want different coloring and how you want them handled. The editor also
supports multiple extensions of the same feature.

The editor presentation layer is based the Windows Presentation Framework (WPF).
WPF provides a graphics library for flexible text formatting, and also provides
visualizations such as graphics and animations.

The Visual Studio SDK provides adapters known as *shims* to support VSPackages
that were written for earlier versions. Nevertheless, if you have an existing
VSPackage, we recommend that you update it to the new technology to obtain
better performance and reliability.

## Related Topics

| RELATED TOPICS                                                                                                                                                                                                                     |                                                                                                                                                                                                                                                                          |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Title                                                                                                                                                                                                                              | Description                                                                                                                                                                                                                                                              |
| [Get started with language service and editor extensions](https://docs.microsoft.com/en-us/visualstudio/extensibility/getting-started-with-language-service-and-editor-extensions?view=vs-2022)                                    | Explains how to create an extension to the editor.                                                                                                                                                                                                                       |
| [Inside the editor](https://docs.microsoft.com/en-us/visualstudio/extensibility/inside-the-editor?view=vs-2022)                                                                                                                    | Describes the general structure of the editor, and lists some of its features.                                                                                                                                                                                           |
| [Managed Extensibility Framework in the editor](https://docs.microsoft.com/en-us/visualstudio/extensibility/managed-extensibility-framework-in-the-editor?view=vs-2022)                                                            | Explains how to use the Managed Extensibility Framework (MEF) with the editor.                                                                                                                                                                                           |
| [Language service and editor extension points](https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2022)                                                              | Lists the extension points of the editor. Extension points represent the editor features that can be extended.                                                                                                                                                           |
| [Walkthrough: Create a view adornment, commands, and settings (column guides)](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-creating-a-view-adornment-commands-and-settings-column-guides?view=vs-2022) | Walks through and explains building a view adornment that draws column guide lines to help you keep code to a certain display width. Also shows reading and writing settings as well as declaring and implementing commands that you can invoke from the Command Window. |
| [Editor imports](https://docs.microsoft.com/en-us/visualstudio/extensibility/editor-imports?view=vs-2022)                                                                                                                          | Lists the services that an extension can import.                                                                                                                                                                                                                         |
| [Adapt legacy code to the editor](https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2015/extensibility/adapting-legacy-code-to-the-editor?preserve-view=true&view=vs-2015)                             | Explains different ways to adapt legacy code (pre-Visual Studio 2010) to extend the editor.                                                                                                                                                                              |
| [Migrate a legacy language service](https://docs.microsoft.com/en-us/visualstudio/extensibility/internals/migrating-a-legacy-language-service?view=vs-2022)                                                                        | Explains how to migrate a VSPackage based language service.                                                                                                                                                                                                              |
| [Walkthrough: Link a content type to a file name extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-linking-a-content-type-to-a-file-name-extension?view=vs-2022)                                  | Shows how to link a content type to a file name extension.                                                                                                                                                                                                               |
| [Walkthrough: Create a margin glyph](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-creating-a-margin-glyph?view=vs-2022)                                                                                 | Shows how to add an icon to a margin.                                                                                                                                                                                                                                    |
| [Walkthrough: Highlight text](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-highlighting-text?view=vs-2022)                                                                                              | Shows how to use *tags* to highlight text.                                                                                                                                                                                                                               |
| [Walkthrough: Add outlining](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-outlining?view=vs-2022)                                                                                                       | Shows how to add outlining for specific kinds of braces.                                                                                                                                                                                                                 |
| [Walkthrough: Display matching braces](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-matching-braces?view=vs-2022)                                                                            | Shows how to highlight matching braces.                                                                                                                                                                                                                                  |
| [Walkthrough: Display QuickInfo tooltips](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-quickinfo-tooltips?view=vs-2022)                                                                      | Shows how to display QuickInfo popups that describe elements of code such as properties, methods, and events.                                                                                                                                                            |
| [Walkthrough: Display signature help](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-signature-help?view=vs-2022)                                                                              | Shows how to display popups that give information about the number and types of parameters in a signature.                                                                                                                                                               |
| [Walkthrough: Display statement completion](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-statement-completion?view=vs-2022)                                                                  | Shows how to implement statement completion.                                                                                                                                                                                                                             |
| [Walkthrough: Implement code snippets](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-implementing-code-snippets?view=vs-2022)                                                                            | Shows how to implement code-snippet expansion.                                                                                                                                                                                                                           |
| [Walkthrough: Display light bulb suggestions](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-light-bulb-suggestions?view=vs-2022)                                                              | Shows how to display light bulbs for code suggestions.                                                                                                                                                                                                                   |
| [Walkthrough: Use a shell command with an editor extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-using-a-shell-command-with-an-editor-extension?view=vs-2022)                                   | Shows how to associate a menu command in a VSPackage with a MEF component.                                                                                                                                                                                               |
| [Walkthrough: Use a shortcut key with an editor extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-using-a-shortcut-key-with-an-editor-extension?view=vs-2022)                                     | Shows how to associate a menu shortcut in a VSPackage with a MEF component.                                                                                                                                                                                              |
| [Managed Extensibility Framework (MEF)](https://docs.microsoft.com/en-us/dotnet/framework/mef/index)                                                                                                                               | Provides information about the Managed Extensibility Framework (MEF).                                                                                                                                                                                                    |
| [Windows Presentation Foundation](https://docs.microsoft.com/en-us/dotnet/framework/wpf/index)                                                                                                                                     | Provides information about the Windows Presentation Foundation (WPF).                                                                                                                                                                                                    |

## Reference

The Visual Studio editor includes the following namespaces.

[Microsoft.VisualStudio.Language.Intellisense](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense)

[Microsoft.VisualStudio.Language.StandardClassification](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.standardclassification)

[Microsoft.VisualStudio.Editor](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.editor)

[Microsoft.VisualStudio.Text](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text)

[Microsoft.VisualStudio.Text.Adornments](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.adornments)

[Microsoft.VisualStudio.Text.Classification](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.classification)

[Microsoft.VisualStudio.Text.Differencing](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.differencing)

[Microsoft.VisualStudio.Text.Document](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.document)

[Microsoft.VisualStudio.Text.Editor](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor)

[Microsoft.VisualStudio.Text.Editor.OptionsExtensionMethods](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.optionsextensionmethods)

[Microsoft.VisualStudio.Text.Formatting](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.formatting)

[Microsoft.VisualStudio.Text.IncrementalSearch](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.incrementalsearch)

[Microsoft.VisualStudio.Text.Operations](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.operations)

[Microsoft.VisualStudio.Text.Outlining](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.outlining)

[Microsoft.VisualStudio.Text.Projection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.projection)

[Microsoft.VisualStudio.Text.Tagging](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.tagging)

[Microsoft.VisualStudio.Utilities](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities)
