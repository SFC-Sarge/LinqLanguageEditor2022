# ExportAttribute Class

<!--Start-Of-TOC-->
   - [Definition](#Definition)
   - [Examples](#Examples)
   - [Remarks](#Remarks)
   - [Constructors](#Constructors)
   - [Properties](#Properties)
   - [Methods](#Methods)
   - [Applies to](#Applies-to)
   - [See also](#See-also)
<!--End-Of-TOC-->


## Definition

Namespace:

[System.ComponentModel.Composition](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition?view=dotnet-plat-ext-6.0)

Assembly:

System.ComponentModel.Composition.dll

Specifies that a type, property, field, or method provides a particular export.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Field | System.AttributeTargets.Method | System.AttributeTargets.Property, AllowMultiple=true, Inherited=false)]
public class ExportAttribute : Attribute
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Inheritance

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object?view=dotnet-plat-ext-6.0)

[Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0)

ExportAttribute

Derived

[System.ComponentModel.Composition.InheritedExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.inheritedexportattribute?view=dotnet-plat-ext-6.0)

Attributes

[AttributeUsageAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.attributeusageattribute?view=dotnet-plat-ext-6.0)

## Examples

The following example shows three classes decorated with the ExportAttribute,
and three imports that match them.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
//Default export infers type and contract name from the
//exported type.  This is the preferred method.
[Export]
public class MyExport1
{
    public String data = "Test Data 1.";
}

public class MyImporter1
{
    [Import]
    public MyExport1 importedMember { get; set; }
}

public interface MyInterface
{
}

//Specifying the contract type may be important if
//you want to export a type other then the base type,
//such as an interface.
[Export(typeof(MyInterface))]
public class MyExport2 : MyInterface
{
    public String data = "Test Data 2.";
}

public class MyImporter2
{
    //The import must match the contract type!
    [Import(typeof(MyInterface))]
    public MyExport2 importedMember { get; set; }
}

//Specifying a contract name should only be
//needed in rare caes. Usually, using metadata
//is a better approach.
[Export("MyContractName", typeof(MyInterface))]
public class MyExport3 : MyInterface
{
    public String data = "Test Data 3.";
}

public class MyImporter3
{
    //Both contract name and type must match!
    [Import("MyContractName", typeof(MyInterface))]
    public MyExport3 importedMember { get; set; }
}

class Program
{

    static void Main(string[] args)
    {
        AggregateCatalog catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(MyExport1).Assembly));
        CompositionContainer _container = new CompositionContainer(catalog);
        MyImporter1 test1 = new MyImporter1();
        MyImporter2 test2 = new MyImporter2();
        MyImporter3 test3 = new MyImporter3();
        _container.SatisfyImportsOnce(test1);
        _container.SatisfyImportsOnce(test2);
        _container.SatisfyImportsOnce(test3);
        Console.WriteLine(test1.importedMember.data);
        Console.WriteLine(test2.importedMember.data);
        Console.WriteLine(test3.importedMember.data);
        Console.ReadLine();
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

## Remarks

In the Attributed Programming Model, the ExportAttribute declares that a part
exports, or provides to the composition container, an object that fulfills a
particular contract. During composition, parts with imports that have matching
contracts will have those dependencies filled by the exported object.

The ExportAttribute can decorate either an entire class, or a property, field,
or method of a class. If the entire class is decorated, an instance of the class
is the exported object. If a member of a class is decorated, the exported object
will be the value of the decorated member.

Whether or not a contract matches is determined primarily by the contract name
and the contract type. For more information, see ImportAttribute.

## Constructors

| CONSTRUCTORS                                                                                                                                                                                                                                    |                                                                                                                                                                         |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [ExportAttribute()](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor)                                        | Initializes a new instance of the ExportAttribute class, exporting the type or member marked with this attribute under the default contract name.                       |
| [ExportAttribute(String)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-string))                   | Initializes a new instance of the ExportAttribute class, exporting the type or member marked with this attribute under the specified contract name.                     |
| [ExportAttribute(String, Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-string-system-type)) | Initializes a new instance of the ExportAttribute class, exporting the specified type under the specified contract name.                                                |
| [ExportAttribute(Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-type))                       | Initializes a new instance of the ExportAttribute class, exporting the type or member marked with this attribute under a contract name derived from the specified type. |

## Properties

| PROPERTIES                                                                                                                                                                                                         |                                                                                                                                                                                                                                                                                                |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [ContractName](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.contractname?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-contractname) | Gets the contract name that is used to export the type or member marked with this attribute.                                                                                                                                                                                                   |
| [ContractType](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.contracttype?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-contracttype) | Gets the contract type that is exported by the member that this attribute is attached to.                                                                                                                                                                                                      |
| [TypeId](https://docs.microsoft.com/en-us/dotnet/api/system.attribute.typeid?view=dotnet-plat-ext-6.0#system-attribute-typeid)                                                                                     | When implemented in a derived class, gets a unique identifier for this [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0). (Inherited from [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0)) |

## Methods

| METHODS                                                                                                                                                              |                                                                                                                                                                                                                                                   |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [Equals(Object)](https://docs.microsoft.com/en-us/dotnet/api/system.attribute.equals?view=dotnet-plat-ext-6.0#system-attribute-equals(system-object))                | Returns a value that indicates whether this instance is equal to a specified object. (Inherited from [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0))                                          |
| [GetHashCode()](https://docs.microsoft.com/en-us/dotnet/api/system.attribute.gethashcode?view=dotnet-plat-ext-6.0#system-attribute-gethashcode)                      | Returns the hash code for this instance. (Inherited from [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0))                                                                                      |
| [GetType()](https://docs.microsoft.com/en-us/dotnet/api/system.object.gettype?view=dotnet-plat-ext-6.0#system-object-gettype)                                        | Gets the [Type](https://docs.microsoft.com/en-us/dotnet/api/system.type?view=dotnet-plat-ext-6.0) of the current instance. (Inherited from [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object?view=dotnet-plat-ext-6.0))          |
| [IsDefaultAttribute()](https://docs.microsoft.com/en-us/dotnet/api/system.attribute.isdefaultattribute?view=dotnet-plat-ext-6.0#system-attribute-isdefaultattribute) | When overridden in a derived class, indicates whether the value of this instance is the default value for the derived class. (Inherited from [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0))  |
| [Match(Object)](https://docs.microsoft.com/en-us/dotnet/api/system.attribute.match?view=dotnet-plat-ext-6.0#system-attribute-match(system-object))                   | When overridden in a derived class, returns a value that indicates whether this instance equals a specified object. (Inherited from [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0))           |
| [MemberwiseClone()](https://docs.microsoft.com/en-us/dotnet/api/system.object.memberwiseclone?view=dotnet-plat-ext-6.0#system-object-memberwiseclone)                | Creates a shallow copy of the current [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object?view=dotnet-plat-ext-6.0). (Inherited from [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object?view=dotnet-plat-ext-6.0)) |
| [ToString()](https://docs.microsoft.com/en-us/dotnet/api/system.object.tostring?view=dotnet-plat-ext-6.0#system-object-tostring)                                     | Returns a string that represents the current object. (Inherited from [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object?view=dotnet-plat-ext-6.0))                                                                                |

| EXPLICIT INTERFACE IMPLEMENTATIONS |
|------------------------------------|
|                                    |

## Applies to

| APPLIES TO               |                                                                   |
|--------------------------|-------------------------------------------------------------------|
| Product                  | Versions                                                          |
| .NET Framework           | 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8 |
| .NET Platform Extensions | 2.1, 2.2, 3.0, 3.1, 5.0, 6.0                                      |
| Xamarin.iOS              | 10.8                                                              |
| Xamarin.Mac              | 3.0                                                               |

## See also

[ImportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute?view=dotnet-plat-ext-6.0)

[Attributed Programming Model
Overview](https://docs.microsoft.com/en-us/dotnet/framework/mef/)
