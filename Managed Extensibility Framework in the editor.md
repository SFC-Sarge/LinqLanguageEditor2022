# Managed Extensibility Framework in the editor

<!--Start-Of-TOC-->
   - [Article](#Article)
   - [Overview of the Managed Extensibility Framework](#Overview-of-the-Managed-Extensibility-Framework)
      - [Component parts and composition containers](#Component-parts-and-composition-containers)
      - [Export and import component parts](#Export-and-import-component-parts)
      - [The export contract](#The-export-contract)
      - [Import a MEF Export](#Import-a-MEF-Export)
   - [Get editor functionality from a MEF component part](#Get-editor-functionality-from-a-MEF-component-part)
      - [To consume editor functionality from a MEF component part](#To-consume-editor-functionality-from-a-MEF-component-part)
<!--End-Of-TOC-->


## Article

08/05/2021

The editor is built by using Managed Extensibility Framework (MEF) components.
You can build your own MEF components to extend the editor, and your code can
consume editor components as well.

## Overview of the Managed Extensibility Framework

The MEF is a .NET library that lets you add and modify features of an
application or component that follows the MEF programming model. The Visual
Studio editor can both provide and consume MEF component parts.

The MEF is contained in the .NET Framework version 4
*System.ComponentModel.Composition.dll* assembly.

For more information about MEF, see [Managed Extensibility Framework
(MEF)](https://docs.microsoft.com/en-us/dotnet/framework/mef/index).

### Component parts and composition containers

A component part is a class or a member of a class that can do one (or both) of
the following:

-   Consume another component

-   Be consumed by another component

    For example, consider a shopping application that has an order entry component
    that depends on product availability data provided by a warehouse inventory
    component. In MEF terms, the inventory part can *export* product availability
    data, and the order entry part can *import* the data. The order entry part and
    the inventory part do not have to know about each other; the *composition
    container* (provided by the host application) is responsible for maintaining the
    set of exports, and resolving the exports and imports.

    The composition container, CompositionContainer, is typically owned by the host.
    The composition container maintains a *catalog* of exported component parts.

### Export and import component parts

You can export any functionality, as long as it is implemented as a public class
or a public member of a class (property or method). You do not have to derive
your component part from ComposablePart. Instead, you must add a ExportAttribute
attribute to the class or class member that you want to export. This attribute
specifies the *contract* by which another component part can import your
functionality.

### The export contract

The ExportAttribute defines the entity (class, interface, or structure) that is
being exported. Typically, the export attribute takes a parameter that specifies
the type of the export.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export(typeof(ContentTypeDefinition))]
class TestContentTypeDefinition : ContentTypeDefinition {   }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

By default, the ExportAttribute attribute defines a contract that is the type of
the exporting class.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export]
[Name("Structure")]
[Order(After = "Selection", Before = "Text")]
class TestAdornmentLayerDefinition : AdornmentLayerDefinition {   }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the example, the default [Export] attribute is equivalent to
[Export(typeof(TestAdornmentLayerDefinition))].

You can also export a property or method, as shown in the following example.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Export]
[Name("Scarlet")]
[Order(After = "Selection", Before = "Text")]
public AdornmentLayerDefinition scarletLayerDefinition;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### Import a MEF Export

When you want to consume a MEF export, you must know the contract (typically the
type) by which it was exported, and add a ImportAttribute attribute that has
that value. By default, the import attribute takes one parameter, which is the
type of the class that it modifies. The following lines of code import the
IClassificationTypeRegistryService type.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
internal IClassificationTypeRegistryService ClassificationRegistry;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Get editor functionality from a MEF component part

If your existing code is a MEF component part, you can use MEF metadata to
consume editor component parts.

### To consume editor functionality from a MEF component part

1.  Add references to *System.Composition.ComponentModel.dll*, which is in the
    global assembly cache (GAC), and to the editor assemblies.

2.  Add the relevant using directives.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

3.  Add the [Import] attribute to your service interface, as follows.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[Import]
ITextBufferFactoryService textBufferService;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

4.  When you have obtained the service, you can consume any one of its
    components.

5.  When you have compiled your assembly, put it in the
    \*..\\Common7\\IDE\\Components\* folder of your Visual Studio installation.
