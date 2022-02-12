# ImportAttribute Constructors

<!--Start-Of-TOC-->
   - [Overloads](#Overloads)
   - [ImportAttribute()](#ImportAttribute())
      - [Remarks](#Remarks)
   - [ImportAttribute(String)](#ImportAttribute(String))
      - [Remarks](#Remarks)
   - [ImportAttribute(Type)](#ImportAttribute(Type))
      - [Remarks](#Remarks)
   - [ImportAttribute(String, Type)](#ImportAttribute(String,-Type))
<!--End-Of-TOC-->


Namespace:

[System.ComponentModel.Composition](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition?view=dotnet-plat-ext-6.0)

Assembly:

System.ComponentModel.Composition.dll

Initializes a new instance of the ImportAttribute class.

## Overloads

| OVERLOADS                                                                                                                                                                                                                                       |                                                                                                                                       |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------|
| [ImportAttribute()](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-ctor)                                        | Initializes a new instance of the ImportAttribute class, importing the export with the default contract name.                         |
| [ImportAttribute(String)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-ctor(system-string))                   | Initializes a new instance of the ImportAttribute class, importing the export with the specified contract name.                       |
| [ImportAttribute(Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-ctor(system-type))                       | Initializes a new instance of the ImportAttribute class, importing the export with the contract name derived from the specified type. |
| [ImportAttribute(String, Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.importattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-importattribute-ctor(system-string-system-type)) | Initializes a new instance of the ImportAttribute class, importing the export with the specified contract name and type.              |

## ImportAttribute()

Initializes a new instance of the ImportAttribute class, importing the export
with the default contract name.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ImportAttribute ();
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### Remarks

The default contract name is the result of calling the GetContractName method on
the property, field, or parameter type that this is marked with this attribute.

The contract name is compared by using the
[Ordinal](https://docs.microsoft.com/en-us/dotnet/api/system.stringcomparer.ordinal?view=dotnet-plat-ext-6.0)
property to perform a case-sensitive, non-linguistic comparison.

Applies to

.NET Framework 4.8 and other versions

| TABLE 2                  |                                                                   |
|--------------------------|-------------------------------------------------------------------|
| Product                  | Versions                                                          |
| .NET Framework           | 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8 |
| .NET Platform Extensions | 2.1, 2.2, 3.0, 3.1, 5.0, 6.0                                      |
| Xamarin.iOS              | 10.8                                                              |
| Xamarin.Mac              | 3.0                                                               |

## ImportAttribute(String)

Initializes a new instance of the ImportAttribute class, importing the export
with the specified contract name.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ImportAttribute (string? contractName);
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Parameters

contractName

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string?view=dotnet-plat-ext-6.0)

The contract name of the export to import, or null or an empty string ("") to
use the default contract name.

### Remarks

The default contract name is the result of calling the GetContractName method on
the property, field, or parameter type that is marked with this attribute.

The contract name is compared by using the
[Ordinal](https://docs.microsoft.com/en-us/dotnet/api/system.stringcomparer.ordinal?view=dotnet-plat-ext-6.0)
property to perform a case-sensitive, non-linguistic comparison.

Applies to

.NET Framework 4.8 and other versions

| TABLE 3                  |                                                                   |
|--------------------------|-------------------------------------------------------------------|
| Product                  | Versions                                                          |
| .NET Framework           | 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8 |
| .NET Platform Extensions | 2.1, 2.2, 3.0, 3.1, 5.0, 6.0                                      |
| Xamarin.iOS              | 10.8                                                              |
| Xamarin.Mac              | 3.0                                                               |

## ImportAttribute(Type)

Initializes a new instance of the ImportAttribute class, importing the export
with the contract name derived from the specified type.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ImportAttribute (Type? contractType);
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Parameters

contractType

[Type](https://docs.microsoft.com/en-us/dotnet/api/system.type?view=dotnet-plat-ext-6.0)

The type to derive the contract name of the export from, or null to use the
default contract name.

### Remarks

The contract name is the result of calling the GetContractName method on
contractType.

The default contract name is the result of calling the GetContractName method on
the property, field, or parameter type that is marked with this attribute.

The contract name is compared by using the
[Ordinal](https://docs.microsoft.com/en-us/dotnet/api/system.stringcomparer.ordinal?view=dotnet-plat-ext-6.0)
property to perform a case-sensitive, non-linguistic comparison.

Applies to

.NET Framework 4.8 and other versions

| TABLE 4                  |                                                                   |
|--------------------------|-------------------------------------------------------------------|
| Product                  | Versions                                                          |
| .NET Framework           | 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8 |
| .NET Platform Extensions | 2.1, 2.2, 3.0, 3.1, 5.0, 6.0                                      |
| Xamarin.iOS              | 10.8                                                              |
| Xamarin.Mac              | 3.0                                                               |

## ImportAttribute(String, Type)

Initializes a new instance of the ImportAttribute class, importing the export
with the specified contract name and type.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ImportAttribute (string? contractName, Type? contractType);
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Parameters

contractName

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string?view=dotnet-plat-ext-6.0)

The contract name of the export to import, or null or an empty string ("") to
use the default contract name.

contractType

[Type](https://docs.microsoft.com/en-us/dotnet/api/system.type?view=dotnet-plat-ext-6.0)

The type of the export to import.

Applies to

.NET Framework 4.8 and other versions

| TABLE 5                  |                                                                   |
|--------------------------|-------------------------------------------------------------------|
| Product                  | Versions                                                          |
| .NET Framework           | 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8 |
| .NET Platform Extensions | 2.1, 2.2, 3.0, 3.1, 5.0, 6.0                                      |
| Xamarin.iOS              | 10.8                                                              |
| Xamarin.Mac              | 3.0                                                               |
