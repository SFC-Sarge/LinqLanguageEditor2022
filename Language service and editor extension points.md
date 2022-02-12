# Language service and editor extension points

<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Extend content types](#Extend-content-types)
   - [FileExtensionToContentTypeDefinition Class](#FileExtensionToContentTypeDefinition-Class)
      - [Definition](#Definition)
      - [Namespace:](#Namespace:)
      - [Inheritance](#Inheritance)
      - [Examples](#Examples)
      - [Remarks](#Remarks)
      - [Constructors](#Constructors)
      - [Note](#Note)
   - [Extend classification types and classification formats](#Extend-classification-types-and-classification-formats)
   - [Extend margins and scrollbars](#Extend-margins-and-scrollbars)
   - [Extend tags](#Extend-tags)
      - [Note](#Note)
      - [Tags and MarkerFormatDefinitions](#Tags-and-MarkerFormatDefinitions)
      - [Note](#Note)
   - [Extend adornments](#Extend-adornments)
   - [Extending Mouse Processors](#Extending-Mouse-Processors)
   - [Extend drop handlers](#Extend-drop-handlers)
   - [Extending Editor Options](#Extending-Editor-Options)
   - [Extend IntelliSense](#Extend-IntelliSense)
      - [Implement an IntelliSense controller](#Implement-an-IntelliSense-controller)
<!--End-Of-TOC-->


## Article

08/05/2021

The editor provides extension points that you can extend as Managed
Extensibility Framework (MEF) component parts, including most language service
features. These are the main extension point categories:

-   Content types

-   Classification types and classification formats

-   Margins and scrollbars

-   Tags

-   Adornments

-   Mouse processors

-   Drop handlers

-   Options

-   IntelliSense

## Extend content types

Content types are the definitions of the kinds of text handled by the editor,
for example, "text", "code", or "CSharp". You define a new content type by
declaring a variable of the type ContentTypeDefinition and giving the new
content type a unique name. To register the content type with the editor, export
it together with the following attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute)
    is the name of the content type.

-   [BaseDefinitionAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.basedefinitionattribute)
    is the name of the content type from which this content type is derived. A
    content type may inherit from multiple other content types.

Because the ContentTypeDefinition class is sealed, you can export it with no
type parameter.

The following example shows export attributes on a content type definition.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export]
[Name("test")]
[BaseDefinition("code")]
[BaseDefinition("projection")]
internal static ContentTypeDefinition TestContentTypeDefinition;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Content types can be based on zero or more pre-existing content types. These are
the built-in types:

-   Any: the basic content type. Parent of all other content types.

-   Text: the basic type for non-projection content. Inherits from "any".

-   Plaintext: for non-code text. Inherits from "text".

-   Code: for code of all kinds. Inherits from "text".

-   Inert: excludes the text from any kind of handling. Text of this content
    type will never have any extension applied to it.

-   Projection: for the contents of projection buffers. Inherits from "any".

-   Intellisense: for the contents of IntelliSense. Inherits from "text".

-   Sighelp: signature help. Inherits from "intellisense".

-   Sighelp-doc: signature help documentation. Inherits from "intellisense".

-   These are some of the content types that are defined by Visual Studio and
    some of the languages that are hosted in Visual Studio:

-   Basic

-   C/C++

-   ConsoleOutput

-   CSharp

-   CSS

-   ENC

-   FindResults

-   F\#

-   HTML

-   JScript

-   XAML

-   XML

To discover the list of available content types, import the
IContentTypeRegistryService, which maintains the collection of content types for
the editor. The following code imports this service as a property.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal IContentTypeRegistryService ContentTypeRegistryService { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

To associate a content type with a file name extension, use
[FileExtensionToContentTypeDefinition](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.fileextensiontocontenttypedefinition).

## FileExtensionToContentTypeDefinition Class

### Definition

### Namespace:

[Microsoft.VisualStudio.Utilities](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities?view=visualstudiosdk-2022)

Assembly:

Microsoft.VisualStudio.CoreUtility.dll

Specifies a mapping between a content type and a file extension.

### Inheritance

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)

FileExtensionToContentTypeDefinition

### Examples

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
internal sealed class Components
{
   [Export]
   [FileExtension(".abc")]           // Any file with the extension "abc" will get the "alphabet" content type.
   [ContentType("alphabet")]
   internal FileExtensionToContentTypeDefinition abcFileExtensionDefinition;

   [Export]
   [FileExtension(".abc.def")]       // Any file with the compound extension "abc.def" will get the "alphabet" content type.
   [ContentType("alphabet")]
   internal FileExtensionToContentTypeDefinition abcDefFileExtensionDefinition;

   [Export]
   [FileName("readme")]           // Any file named "readme" will get the "alphabet" content type.
   [ContentType("alphabet")]
   internal FileExtensionToContentTypeDefinition readmeFileNameDefinition;
   { other components }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### Remarks

Because you cannot subclass this type, you can use the [Export] attribute with
no type.

Compound extensions, such as '.abc.def' are supported via the
FileExtensionAttribute, however, if there is a mapping for a compound '.abc.def'
and a simple extension 'def', the one that is longer wins.

### Constructors

| CONSTRUCTORS                                                                                                                                                                                                                                                           |                                                                     |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------|
| [FileExtensionToContentTypeDefinition()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.fileextensiontocontenttypedefinition.-ctor?view=visualstudiosdk-2022#microsoft-visualstudio-utilities-fileextensiontocontenttypedefinition-ctor) | Initializes a new instance of FileExtensionToContentTypeDefinition. |

### Note

In Visual Studio, file name extensions are registered by using the
ProvideLanguageExtensionAttribute on a language service package. The
FileExtensionToContentTypeDefinition associates a MEF content type with a file
name extension that has been registered in this manner.

To export the file name extension to the content type definition, you must
include the following attributes:

-   [FileExtensionAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.fileextensionattribute):
    specifies the file name extension.

-   [ContentTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypeattribute):
    specifies the content type.

Because the FileExtensionToContentTypeDefinition class is sealed, you can export
it with no type parameter.

The following example shows export attributes on a file name extension to a
content type definition.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export]
[FileExtension(".test")]
[ContentType("test")]
internal static FileExtensionToContentTypeDefinition TestFileExtensionDefinition;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The IFileExtensionRegistryService manages the associations between file name
extensions and content types.

## Extend classification types and classification formats

You can use classification types to define the kinds of text for which you want
to provide different handling (for example, coloring the "keyword" text blue and
the "comment" text green). Define a new classification type by declaring a
variable of type ClassificationTypeDefinition and giving it a unique name.

To register the classification type with the editor, export it together with the
following attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    the name of the classification type.

-   [BaseDefinitionAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.basedefinitionattribute):
    the name of the classification type from which this classification type
    inherits. All classification types inherit from "text", and a classification
    type may inherit from multiple other classification types.

Because the ClassificationTypeDefinition class is sealed, you can export it with
no type parameter.

The following example shows export attributes on a classification type
definition.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export]
[Name("csharp.test")]
[BaseDefinition("test")]
internal static ClassificationTypeDefinition CSharpTestDefinition;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The IStandardClassificationService provides access to standard classifications.
Built-in classification types include these:

-   "text"

-   "natural language" (derives from "text")

-   "formal language" (derives from "text")

-   "string" (derives from "literal")

-   "character" (derives from "literal")

-   "numerical" (derives from "literal")

A set of different error types inherit from ErrorTypeDefinition. They include
the following error types:

-   "syntax error"

-   "compiler error"

-   "other error"

-   "warning"

To discover the list of available classification types, import the
IClassificationTypeRegistryService, which maintains the collection of
classification types for the editor. The following code imports this service as
a property.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal IClassificationTypeRegistryService ClassificationTypeRegistryService { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

You can define a classification format definition for your new classification
type. Derive a class from ClassificationFormatDefinition and export it with the
type EditorFormatDefinition, together with the following attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    the name of the format.

-   [DisplayNameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.displaynameattribute):
    the display name of the format.

-   [UserVisibleAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.classification.uservisibleattribute):
    specifies whether the format appears on the Fonts and Colors page of the
    Options dialog box.

-   [OrderAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.orderattribute):
    the priority of the format. Valid values are from
    [Priority](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.classification.priority).

-   [ClassificationTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.classification.classificationtypeattribute):
    the name of the classification type to which this format is mapped.

The following example shows export attributes on a classification format
definition.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(EditorFormatDefinition))]
[ClassificationType(ClassificationTypeNames = "test")]
[Name("test")]
[DisplayName("Test")]
[UserVisible(true)]
[Order(After = Priority.Default, Before = Priority.High)]
internal sealed class TestFormat : ClassificationFormatDefinition
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

To discover the list of available formats, import the IEditorFormatMapService,
which maintains the collection of formats for the editor. The following code
imports this service as a property.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal IEditorFormatMapService FormatMapService { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Extend margins and scrollbars

Margins and scrollbars are the main view elements of the editor in addition to
the text view itself. You can provide any number of margins in addition to the
standard margins that appear around the text view.

Implement an IWpfTextViewMargin interface to define a margin. You must also
implement the IWpfTextViewMarginProvider interface to create the margin.

To register the margin provider with the editor, you must export the provider
together with the following attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    the name of the margin.

-   [OrderAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.orderattribute):
    the order in which the margin appears, relative to the other margins.

These are the built-in margins:

-   "Wpf Horizontal Scrollbar"

-   "Wpf Vertical Scrollbar"

-   "Wpf Line Number Margin"

Horizontal margins that have an order attribute of After="Wpf Horizontal
Scrollbar" are displayed below the built-in margin, and horizontal margins that
have an order attribute of Before ="Wpf Horizontal Scrollbar" are displayed
above the built-in margin. Right vertical margins that have an order attribute
of After="Wpf Vertical Scrollbar" are displayed to the right of the scrollbar.
Left vertical margins that have an order attribute of After="Wpf Line Number
Margin" appear to the left of the line number margin (if it is visible).

-   [MarginContainerAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.margincontainerattribute):
    the kind of margin (left, right, top, or bottom).

-   [ContentTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypeattribute):
    the kind of content (for example, "text" or "code") for which your margin is
    valid.

The following example shows export attributes on a margin provider for a margin
that appears to the right of the line number margin.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IWpfTextViewMarginProvider))]
[Name("TestMargin")]
[Order(Before = "Wpf Line Number Margin")]
[MarginContainer(PredefinedMarginNames.Left)]
[ContentType("text")]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Extend tags

Tags are a way of associating data with different kinds of text. In many cases,
the associated data is displayed as a visual effect, but not all tags have a
visual presentation. You can define your own kind of tag by implementing ITag.
You must also implement ITagger\<T\> to provide the tags for a given set of text
spans, and an ITaggerProvider to provide the tagger. You must export the tagger
provider together with the following attributes:

-   [ContentTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypeattribute):
    the kind of content (for example, "text" or "code") for which your tag is
    valid.

-   [TagTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.tagging.tagtypeattribute):
    the kind of tag.

The following example shows export attributes on a tagger provider.

\<CodeContentPlaceHolder\>8 The following kinds of tag are built-in:

-   [ClassificationTag](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.tagging.classificationtag):
    associated with an IClassificationType.

-   [ErrorTag](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.tagging.errortag):
    associated with error types.

-   [TextMarkerTag](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.tagging.textmarkertag):
    associated with an adornment.

### Note

For an example of a TextMarkerTag, see the HighlightWordTag definition in
[Walkthrough: Highlighting
Text](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-highlighting-text?view=vs-2022).

-   [OutliningRegionTag](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.tagging.outliningregiontag):
    associated with regions that can be expanded or collapsed in outlining.

-   [SpaceNegotiatingAdornmentTag](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.tagging.spacenegotiatingadornmenttag):
    defines the space an adornment occupies in a text view. For more information
    about space-negotiating adornments, see the following section.

-   [IntraTextAdornmentTag](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.intratextadornmenttag):
    provides automatic spacing and sizing for the adornment.

To find and use tags for buffers and views, import the
IViewTagAggregatorFactoryService or the IBufferTagAggregatorFactoryService,
which give you an ITagAggregator\<T\> of the requested type. The following code
imports this service as a property.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal IViewTagAggregatorFactoryService ViewTagAggregatorFactoryService { get; set; }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### Tags and MarkerFormatDefinitions

You can extend the MarkerFormatDefinition class to define the appearance of a
tag. You must export your class (as a EditorFormatDefinition)with the following
attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    the name used to reference this format

-   [UserVisibleAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.classification.uservisibleattribute):
    this causes the format to appear in the UI

In the constructor, you define the display name and appearance of the tag.
BackgroundColor defines the fill color, and ForegroundColor defines the border
color. The
[DisplayName](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.classification.editorformatdefinition.displayname)
is the localizable name of the format definition.

The following is an example of a format definition:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(EditorFormatDefinition))]
[Name("MarkerFormatDefinition/HighlightWordFormatDefinition")]
[UserVisible(true)]
internal class HighlightWordFormatDefinition : MarkerFormatDefinition
{
    public HighlightWordFormatDefinition()
    {
        this.BackgroundColor = Colors.LightBlue;
        this.ForegroundColor = Colors.DarkBlue;
        this.DisplayName = "Highlight Word";
        this.ZOrder = 5;
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

To apply this format definition to a tag, reference the name you set in the name
attribute of the class (not the display name).

### Note

For an example of a MarkerFormatDefinition, see the
HighlightWordFormatDefinition class in [Walkthrough: Highlighting
Text](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-highlighting-text?view=vs-2022).

## Extend adornments

Adornments define visual effects that can be added either to the text that is
displayed in a text view or to the text view itself. You can define your own
adornment as any type of UIElement.

In your adornment class, you must declare an AdornmentLayerDefinition. To
register your adornment layer, export it together with the following attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    the name of the adornment.

-   [OrderAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.orderattribute):
    the ordering of the adornment with respect to other adornment layers. The
    class PredefinedAdornmentLayers defines four default layers: Selection,
    Outlining, Caret, and Text.

The following example shows export attributes on an adornment layer definition.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export]
[Name("TestEmbeddedAdornment")]
[Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
internal AdornmentLayerDefinition testLayerDefinition;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

You must create a second class that implements IWpfTextViewCreationListener and
handles its TextViewCreated event by instantiating the adornment. You must
export this class together with the following attributes:

-   [ContentTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypeattribute):
    the kind of content (for example, "text" or "code") for which the adornment
    is valid.

-   [TextViewRoleAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.textviewroleattribute):
    the kind of text view for which this adornment is valid. The class
    PredefinedTextViewRoles has the set of predefined text view roles. For
    example,
    [Document](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.predefinedtextviewroles.document)
    is primarily used for text views of files.
    [Interactive](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.predefinedtextviewroles.interactive)
    is used for text views that a user can edit or navigate by using a mouse and
    keyboard. Examples of
    [Interactive](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.predefinedtextviewroles.interactive)
    views are the editor text view and the Output window.

The following example shows export attributes on the adornment provider.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IWpfTextViewCreationListener))]
[ContentType("csharp")]
[TextViewRole(PredefinedTextViewRoles.Document)]
internal sealed class TestAdornmentProvider : IWpfTextViewCreationListener
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

A space-negotiating adornment is one that occupies space at the same level as
the text. To create this kind of adornment, you must define a tag class that
inherits from SpaceNegotiatingAdornmentTag, which defines the amount of space
the adornment occupies.

As with all adornments, you must export the adornment layer definition.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export]
[Name("TestAdornment")]
[Order(After = DefaultAdornmentLayers.Text)]
internal AdornmentLayerDefinition testAdornmentLayer;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

To instantiate the space-negotiating adornment, you must create a class that
implements ITaggerProvider, in addition to the class that implements
IWpfTextViewCreationListener (as with other kinds of adornments).

To register the tagger provider, you must export it together with the following
attributes:

-   [ContentTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypeattribute):
    the kind of content (for example, "text" or "code") for which your adornment
    is valid.

-   [TextViewRoleAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.textviewroleattribute):
    the kind of text view for which this tag or adornment is valid. The class
    PredefinedTextViewRoles has the set of predefined text view roles. For
    example,
    [Document](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.predefinedtextviewroles.document)
    is primarily used for text views of files.
    [Interactive](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.predefinedtextviewroles.interactive)
    is used for text views that a user can edit or navigate by using a mouse and
    keyboard. Examples of
    [Interactive](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.predefinedtextviewroles.interactive)
    views are the editor text view and the Output window.

-   [TagTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.tagging.tagtypeattribute):
    the kind of tag or adornment that you have defined. You must add a second
    TagTypeAttribute for SpaceNegotiatingAdornmentTag.

The following example shows export attributes on the tagger provider for a
space-negotiating adornment tag.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(ITaggerProvider))]
[ContentType("text")]
[TextViewRole(PredefinedTextViewRoles.Document)]
[TagType(typeof(SpaceNegotiatingAdornmentTag))]
[TagType(typeof(TestSpaceNegotiatingTag))]
internal sealed class TestTaggerProvider : ITaggerProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Extending Mouse Processors

You can add special handling for mouse input. Create a class that inherits from
MouseProcessorBase and override the mouse events for the input you want to
handle. You must also implement IMouseProcessorProvider in a second class and
export it together with the ContentTypeAttribute that specifies the kind of
content (for example, "text" or "code") for which your mouse handler is valid.

The following example shows export attributes on a mouse processor provider.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IMouseProcessorProvider))]
[Name("test mouse processor")]
[ContentType("text")]
[TextViewRole(PredefinedTextViewRoles.Interactive)]
internal sealed class TestMouseProcessorProvider : IMouseProcessorProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Extend drop handlers

You can customize the behavior of drop handlers for specific kinds of text by
creating a class that implements IDropHandler and a second class that implements
IDropHandlerProvider to create the drop handler. You must export the drop
handler together with the following attributes:

[DropFormatAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.dragdrop.dropformatattribute):
the text format for which this drop handler is valid. The following formats are
handled in priority order from highest to lowest:

1.  Any custom format

2.  FileDrop

3.  EnhancedMetafile

4.  WaveAudio

5.  Riff

6.  Dif

7.  Locale

8.  Palette

9.  PenData

10. Serializable

11. SymbolicLink

12. Xaml

13. XamlPackage

14. Tiff

15. Bitmap

16. Dib

17. MetafilePicture

18. CSV

19. System.String

20. HTML Format

21. UnicodeText

22. OEMText

23. Text

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    the name of the drop handler.

-   [OrderAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.orderattribute):
    the ordering of the drop handler before or after the default drop handler.
    The default drop handler for Visual Studio is named
    "DefaultFileDropHandler".

The following example shows export attributes on a drop handler provider.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(IDropHandlerProvider))]
[DropFormat("Text")]
[Name("TestDropHandler")]
[Order(Before="DefaultFileDropHandler")]
internal class TestDropHandlerProvider : IDropHandlerProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Extending Editor Options

You can define options to be valid only in a certain scope, for example, in a
text view. The editor provides this set of predefined options: editor options,
view options, and Windows Presentation Foundation (WPF) view options. These
options can be found in DefaultOptions, DefaultTextViewOptions, and
DefaultWpfViewOptions.

To add a new option, derive a class from one of these option definition classes:

-   [EditorOptionDefinition\<T\>](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.editoroptiondefinition-1)

-   [ViewOptionDefinition\<T\>](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.viewoptiondefinition-1)

-   [WpfViewOptionDefinition\<T\>](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.text.editor.wpfviewoptiondefinition-1)

The following example shows how to export an option definition that has a
Boolean value.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(EditorOptionDefinition))]
internal sealed class TestOption : EditorOptionDefinition<bool>
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Extend IntelliSense

IntelliSense is a general term for a group of features that provide information
about structured text, and statement completion for it. These features include
statement completion, signature help, Quick Info, and light bulbs. Statement
completion helps users type a language keyword or member name correctly.
Signature help displays the signature or signatures for the method that the user
has just typed. Quick Info displays a complete signature for a type or member
name when the mouse rests on it. Light bulb provide additional actions for
certain identifiers in certain contexts, for example, renaming all occurrences
of a variable after one occurrence has been renamed.

The design of an IntelliSense feature is much the same in all cases:

An IntelliSense *broker* is responsible for the overall process.

An IntelliSense *session* represents the sequence of events between the
triggering of the presenter and the committal or cancellation of the selection.
The session is typically triggered by some user gesture.

An IntelliSense *controller* is responsible for deciding when the session should
start and end. It also decides when the information should be committed and when
the session should be cancelled.

An IntelliSense *source* provides the content and decides the best match.

An IntelliSense *presenter* is responsible for displaying the content.

In most cases, we recommend that you provide at least a source and a controller.
You can also provide a presenter if you want to customize the display.

Implement an IntelliSense Source

To customize a source, you must implement one (or more) of the following source
interfaces:

-   [ICompletionSource](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.icompletionsource)

-   [IQuickInfoSource](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.iquickinfosource)

-   [ISignatureHelpSource](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.isignaturehelpsource)

-   [ISuggestedActionsSource](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.isuggestedactionssource)

>   Important

>   [ISmartTagSource](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.ismarttagsource)
>   has been deprecated in favor of ISuggestedActionsSource.

>   In addition, you must implement a provider of the same kind:

-   [ICompletionSourceProvider](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.icompletionsourceprovider)

-   [IQuickInfoSourceProvider](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.iquickinfosourceprovider)

-   [ISignatureHelpSourceProvider](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.isignaturehelpsourceprovider)

-   [ISuggestedActionsSourceProvider](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.isuggestedactionssourceprovider)

>   Important

>   [ISmartTagSourceProvider](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.ismarttagsourceprovider)
>   has been deprecated in favor of ISuggestedActionsSourceProvider.

You must export the provider together with the following attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    the name of the source.

-   [ContentTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypeattribute):
    the kind of content (for example, "text" or "code") to which the source
    applies.

-   [OrderAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.orderattribute):
    the order in which the source should appear (with respect to other sources).

The following example shows export attributes on a completion source provider.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
Export(typeof(ICompletionSourceProvider))]
[Name(" Test Statement Completion Provider")]
[Order(Before = "default")]
[ContentType("text")]
internal class TestCompletionSourceProvider : ICompletionSourceProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

For more information about implementing IntelliSense sources, see the following
walkthroughs:

-   [Walkthrough: Display QuickInfo
    tooltips](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-quickinfo-tooltips?view=vs-2022)

-   [Walkthrough: Display Signature
    Help](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-signature-help?view=vs-2022)

-   [Walkthrough: Display statement
    completion](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-statement-completion?view=vs-2022)

### Implement an IntelliSense controller

To customize a controller, you must implement the IIntellisenseController
interface. In addition, you must implement a controller provider together with
the following attributes:

-   [NameAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.nameattribute):
    the name of the controller.

-   [ContentTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.contenttypeattribute):
    the kind of content (for example, "text" or "code") to which the controller
    applies.

-   [OrderAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.orderattribute):
    the order in which the controller should appear (with respect to other
    controllers).

The following example shows export attributes on a completion controller
provider.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
Export(typeof(IIntellisenseControllerProvider))]
[Name(" Test Controller Provider")]
[Order(Before = "default")]
[ContentType("text")]
internal class TestIntellisenseControllerProvider : IIntellisenseControllerProvider
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

For more information about using IntelliSense controllers, see the following
walkthroughs:

-   [Walkthrough: Display QuickInfo
    tooltips](https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-quickinfo-tooltips?view=vs-2022)
