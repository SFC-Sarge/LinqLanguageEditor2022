# Get started with language service and editor extensions

<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Note](#Note)
   - [The Windows Presentation Foundation (WPF) and editor extensions](#The-Windows-Presentation-Foundation-(WPF)-and-editor-extensions)
   - [The Managed Extensibility Framework (MEF) and editor extensions](#The-Managed-Extensibility-Framework-(MEF)-and-editor-extensions)
   - [Visual Studio editor extension points and extensions](#Visual-Studio-editor-extension-points-and-extensions)
   - [Deploying editor extensions](#Deploying-editor-extensions)
      - [Warning](#Warning)
   - [Run extensions in the experimental instance](#Run-extensions-in-the-experimental-instance)
   - [Manage extensions](#Manage-extensions)
   - [Use templates to create editor extensions](#Use-templates-to-create-editor-extensions)
<!--End-Of-TOC-->


## Article

08/05/2021

You can use editor extensions to add language service features such as
outlining, brace matching, IntelliSense, and light bulbs to your own programming
language or to any content type. You can also customize the appearance and
behavior of the Visual Studio editor, for example text coloring, margins,
adornments, and other visual elements. You can also define your own type of
content, and specify the appearance and behavior of the text views in which your
content appears.

To get started writing editor extensions, use the editor project templates that
are installed as part of the Visual Studio SDK. The Visual Studio SDK is a
downloadable set of tools that make it easier to develop Visual Studio
extensions, either by using VSPackages or by using the Managed Extensibility
Framework (MEF).

## Note

For more information about the Visual Studio SDK, see [Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/visual-studio-sdk?view=vs-2022).

We recommend that you learn about the following concepts and technologies before
you write your own editor extensions.

## The Windows Presentation Foundation (WPF) and editor extensions

The Visual Studio editor user interface (UI) is implemented by using the Windows
Presentation Foundation (WPF). The WPF provides a rich visual experience and a
consistent programming model that separates the visual aspects of the code from
the business logic. You can use many WPF elements and features when you create
editor extensions. For more information, see [Windows Presentation
Foundation](https://docs.microsoft.com/en-us/dotnet/framework/wpf/index).

## The Managed Extensibility Framework (MEF) and editor extensions

The Visual Studio editor uses the Managed Extensibility Framework (MEF) to
manage its components and extensions. The MEF also lets developers more easily
create extensions for a host application like Visual Studio. In this framework,
you define an extension according to a MEF contract and export it as a MEF
component part. The host application manages the component parts by finding
them, registering them, and making sure that they are applied to the correct
context.

Note

For more information about the MEF in the editor, see [Managed Extensibility
Framework in the
editor](https://docs.microsoft.com/en-us/visualstudio/extensibility/managed-extensibility-framework-in-the-editor?view=vs-2022).

## Visual Studio editor extension points and extensions

Editor extension points are MEF component parts that you can customize and
extend. In some cases you extend the extension point by implementing an
interface and exporting it together with the correct metadata. In other cases
you just declare an extension and export it as a particular type.

The following are some of the basic kinds of editor extensions:

-   Margins and scrollbars

-   Tags

-   Adornments

-   Options

-   IntelliSense

For more information about editor extension points, see [Language service and
editor extension
points](https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2022).

## Deploying editor extensions

In Visual Studio, you deploy editor extensions by adding a metadata file named
*source.extension.vsixmanifest* to the solution, building the solution, and then
adding a copy of the binary files and the manifest in a folder that is known to
Visual Studio. The manifest file defines the basic facts about the extension
(for example, name, author, version, and type of content). For more information
about the VSIX manifest file and how to deploy extensions, see [Ship Visual
Studio
extensions](https://docs.microsoft.com/en-us/visualstudio/extensibility/shipping-visual-studio-extensions?view=vs-2022).

When you install an extension on a computer, include the binaries and the
manifest in a subfolder of folder that is known to Visual Studio.

### Warning

You do not have to worry about the details of manifests and deployment locations
if you use one of the editor extensibility templates that are included in Visual
Studio. The templates contain everything that is required to register and deploy
an extension.

## Run extensions in the experimental instance

You can insulate your working version of Visual Studio while you are developing
an extension by deploying it in the following experimental folder (on Windows
Vista and Windows 7):

*{%LOCALAPPDATA%}\\VisualStudio\\10.0Exp\\Extensions\\{Company}\\{ExtensionID}*

where *%LOCALAPPDATA%* is the name of the logged-on user, *Company* is the name
of the company that owns the extension, and *ExtensionID* is the ID of the
extension.

When you deploy an extension to the experimental location, it runs in debug
mode. A second instance of Visual Studio is started, and is named Microsoft
Visual Studio - Experimental Instance.

## Manage extensions

Extensions to Visual Studio are listed in Extensions and Updates (on the Tools
menu). If you are testing an extension in the experimental instance, it is
listed in Extensions and Updates in the experimental instance, but is not listed
in the development instance.

For more information, see [Find and use Visual Studio
extensions](https://docs.microsoft.com/en-us/visualstudio/ide/finding-and-using-visual-studio-extensions?view=vs-2022).

## Use templates to create editor extensions

You can use editor templates to create MEF extensions that customize
classifiers, adornments, and margins. There are templates for both C\# and
Visual Basic projects. For more information, see [Create an extension with an
editor item
template](https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-an-editor-item-template?view=vs-2022).

You can also use the VSIX Project template to create extensions. This template
provides only the elements that are required to deploy any kind of extension,
and include the *source.extension.vsixmanifest* file, the required assembly
references, and a project file that includes the build tasks that allow you to
deploy the extension. For more information, see [VSIX project
template](https://docs.microsoft.com/en-us/visualstudio/extensibility/vsix-project-template?view=vs-2022).

You can also create editor MEF components from a Visual Studio Package
extension. See the following walkthroughs for details:

-   [Walkthrough: Using a shell command with an editor
    extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-using-a-shell-command-with-an-editor-extension?view=vs-2022)

-   [Walkthrough: Using a shortcut key with an editor
    extension](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-using-a-shortcut-key-with-an-editor-extension?view=vs-2022)

See also

[Language service and editor extension
points](https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2022)
