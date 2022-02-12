# Walkthrough: Link a content type to a file name extension


<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Prerequisites](#Prerequisites)
   - [Create a MEF project](#Create-a-MEF-project)
   - [Define the content type](#Define-the-content-type)
   - [Link a file name extension to a content type](#Link-a-file-name-extension-to-a-content-type)
   - [Add the content type to an editor export](#Add-the-content-type-to-an-editor-export)
<!--End-Of-TOC-->

## Article

08/05/2021

You can define your own content type and link a file name extension to it by
using the editor Managed Extensibility Framework (MEF) extensions. In some
cases, the file name extension is already defined by a language service. But, to
use it with MEF, you must still link it to a content type.

## Prerequisites

Starting in Visual Studio 2015, you don't install the Visual Studio SDK from the
download center. It's included as an optional feature in Visual Studio setup.
You can also install the VS SDK later. For more information, see [Install the
Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2022).

## Create a MEF project

Create a C\# VSIX project. (In the New Project dialog, select Visual C\# /
Extensibility, then VSIX Project.) Name the solution ContentTypeTest.

In the source.extension.vsixmanifest file, go to the Assets tab, and set the
Type field to Microsoft.VisualStudio.MefComponent, the Source field to A project
in current solution, and the Project field to the name of the project.

## Define the content type

Add a class file and name it FileAndContentTypes.

Add references to the following assemblies:

-   System.ComponentModel.Composition

-   Microsoft.VisualStudio.Text.Logic

-   Microsoft.VisualStudio.CoreUtility

Add the following using directives.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Declare a static class that contains the definitions.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal static class FileAndContentTypeDefinitions
{. . .}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In this class, export a
[ContentTypeDefinition](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypedefinition)
named "hid" and declare its base definition to be "text".

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal static class FileAndContentTypeDefinitions
{
    [Export]
    [Name("hid")]
    [BaseDefinition("text")]
    internal static ContentTypeDefinition hidingContentTypeDefinition;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Link a file name extension to a content type

To map this content type to a file name extension, export a
[FileExtensionToContentTypeDefinition](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.fileextensiontocontenttypedefinition)
that has the extension *.hid* and the content type "hid".

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal static class FileAndContentTypeDefinitions
{
     [Export]
     [Name("hid")]
     [BaseDefinition("text")]
    internal static ContentTypeDefinition hidingContentTypeDefinition;

     [Export]
     [FileExtension(".hid")]
     [ContentType("hid")]
    internal static FileExtensionToContentTypeDefinition hiddenFileExtensionDefinition;
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Add the content type to an editor export

Create an editor extension. For example, you can use the margin glyph extension
described in [Walkthrough: Create a margin
glyph](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-creating-a-margin-glyph?view=vs-2022).

Add the class you defined in this procedure.

When you export the extension class, add a
[ContentTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypeattribute)
of type "hid" to it.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export]
[ContentType("hid")]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

See also

[Language service and editor extension
points](https://docs.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points?view=vs-2022)
