# ImportAttribute Class

<!--Start-Of-TOC-->
   - [Examples](#Examples)
   - [Remarks](#Remarks)
   - [Constructors](#Constructors)
   - [Properties](#Properties)
   - [Methods](#Methods)
   - [Applies to](#Applies-to)
   - [See also](#See-also)
<!--End-Of-TOC-->


Namespace:

[System.ComponentModel.Composition](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition?view=dotnet-plat-ext-6.0)

Assembly:

System.ComponentModel.Composition.dll

Specifies that a property, field, or parameter value should be provided by the
CompositionContainer.object.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
public class ImportAttribute : Attribute
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Inheritance

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object?view=dotnet-plat-ext-6.0)

[Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0)

ImportAttribute

Attributes

[AttributeUsageAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.attributeusageattribute?view=dotnet-plat-ext-6.0)

## Examples

The following example shows three classes with members decorated with the
ImportAttribute, and three exports that match them.

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

In the Attributed Programming Model, the ImportAttribute is used to declare the
imports, or dependencies, of a given part. It can decorate a property, a field,
or a method. During composition, a part's imports will be filled by the
CompositionContainer object to which that part belongs, by using the exports
provided to that CompositionContainer object.

Whether an import matches a given export is determined primarily by comparing
the contract name and the contract type. Ordinarily, you do not have to specify
either of these when using the import attribute in code, and they will be
automatically inferred from the type of the decorated member. If the import must
match an export of a different type (for example, a subclass of the type of the
decorated member, or an interface implemented by that member), then the contract
type must be explicitly specified. The contract name can also be explicitly
specified, for example to distinguish between multiple contracts with the same
type, but it is usually better to do this through metadata. For more information
about metadata, see PartMetadataAttribute.

## Constructors

| CONSTRUCTORS                                                                                                                                                                                                                                    |                                                                                                                                       |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------|
| [ImportAttribute()](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-ctor)                                        | Initializes a new instance of the ImportAttribute class, importing the export with the default contract name.                         |
| [ImportAttribute(String)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-ctor(system-string))                   | Initializes a new instance of the ImportAttribute class, importing the export with the specified contract name.                       |
| [ImportAttribute(String, Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-ctor(system-string-system-type)) | Initializes a new instance of the ImportAttribute class, importing the export with the specified contract name and type.              |
| [ImportAttribute(Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-ctor(system-type))                       | Initializes a new instance of the ImportAttribute class, importing the export with the contract name derived from the specified type. |

## Properties

| PROPERTIES                                                                                                                                                                                                                                       |                                                                                                                                                                                                                                                                                                |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [AllowDefault](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.allowdefault?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-allowdefault)                               | Gets or sets a value that indicates whether the property, field, or parameter will be set to its type's default value when an export with the contract name is not present in the container.                                                                                                   |
| [AllowRecomposition](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.allowrecomposition?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-allowrecomposition)             | Gets or sets a value that indicates whether the property or field will be recomposed when exports with a matching contract have changed in the container.                                                                                                                                      |
| [ContractName](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.contractname?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-contractname)                               | Gets the contract name of the export to import.                                                                                                                                                                                                                                                |
| [ContractType](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.contracttype?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-contracttype)                               | Gets the type of the export to import.                                                                                                                                                                                                                                                         |
| [RequiredCreationPolicy](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.requiredcreationpolicy?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-requiredcreationpolicy) | Gets or sets a value that indicates that the importer requires a specific CreationPolicy for the exports used to satisfy this import.                                                                                                                                                          |
| [Source](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.source?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-source)                                                 | Gets or sets a value that specifies the scopes from which this import may be satisfied.                                                                                                                                                                                                        |
| [TypeId](https://docs.microsoft.com/en-us/dotnet/api/system.attribute.typeid?view=dotnet-plat-ext-6.0#system-attribute-typeid)                                                                                                                   | When implemented in a derived class, gets a unique identifier for this [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0). (Inherited from [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute?view=dotnet-plat-ext-6.0)) |

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

[ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0)

[Attributed Programming Model
Overview](https://docs.microsoft.com/en-us/dotnet/framework/mef/)
