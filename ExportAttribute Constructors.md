# ExportAttribute Constructors

<!--Start-Of-TOC-->
      - [Namespace:](#Namespace:)
      - [Assembly:](#Assembly:)
   - [Overloads](#Overloads)
   - [ExportAttribute()](#ExportAttribute())
      - [Remarks](#Remarks)
   - [ExportAttribute(String)](#ExportAttribute(String))
      - [Remarks](#Remarks)
   - [ExportAttribute(Type)](#ExportAttribute(Type))
      - [Remarks](#Remarks)
   - [ExportAttribute(String, Type)](#ExportAttribute(String,-Type))
<!--End-Of-TOC-->


### Namespace:

[System.ComponentModel.Composition](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition?view=dotnet-plat-ext-6.0)

### Assembly:

System.ComponentModel.Composition.dll

Initializes a new instance of the
[ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0)
class.

## Overloads

| OVERLOADS                                                                                                                                                                                                                                       |                                                                                                                                                                                                                                                                                                   |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [ExportAttribute()](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor)                                        | Initializes a new instance of the [ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0) class, exporting the type or member marked with this attribute under the default contract name.                       |
| [ExportAttribute(String)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-string))                   | Initializes a new instance of the [ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0) class, exporting the type or member marked with this attribute under the specified contract name.                     |
| [ExportAttribute(Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-type))                       | Initializes a new instance of the [ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0) class, exporting the type or member marked with this attribute under a contract name derived from the specified type. |
| [ExportAttribute(String, Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-string-system-type)) | Initializes a new instance of the [ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0) class, exporting the specified type under the specified contract name.                                                |

## ExportAttribute()

Initializes a new instance of the
[ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0)
class, exporting the type or member marked with this attribute under the default
contract name.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ExportAttribute ();
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### Remarks

The default contract name is the result of calling the
[GetContractName](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.attributedmodelservices.getcontractname?view=dotnet-plat-ext-6.0)
method on the property or field type, or on the type that is marked with this
attribute.

Methods marked with this attribute must specify a contract name or a type by
using either
[ExportAttribute(String)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-string))
or
[ExportAttribute(Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-type)).

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

## ExportAttribute(String)

Initializes a new instance of the
[ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0)
class, exporting the type or member marked with this attribute under the
specified contract name.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ExportAttribute (string? contractName);
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Parameters

contractName

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string?view=dotnet-plat-ext-6.0)

The contract name that is used to export the type or member marked with this
attribute, or null or an empty string ("") to use the default contract name.

### Remarks

The default contract name is the result of calling the
[GetContractName](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.attributedmodelservices.getcontractname?view=dotnet-plat-ext-6.0)
method on the property or field type, or on the type that this is marked with
this attribute.

Methods marked with this attribute must specify a contract name or a type by
using either
[ExportAttribute(String)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-string))
or
[ExportAttribute(Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-type)).

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

## ExportAttribute(Type)

Initializes a new instance of the
[ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0)
class, exporting the type or member marked with this attribute under a contract
name derived from the specified type.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ExportAttribute (Type? contractType);
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Parameters

contractType

[Type](https://docs.microsoft.com/en-us/dotnet/api/system.type?view=dotnet-plat-ext-6.0)

A type from which to derive the contract name that is used to export the type or
member marked with this attribute, or null to use the default contract name.

### Remarks

The contract name is the result of calling the
[GetContractName](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.attributedmodelservices.getcontractname?view=dotnet-plat-ext-6.0)
method on contractType.

The default contract name is the result of calling the
[GetContractName](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.attributedmodelservices.getcontractname?view=dotnet-plat-ext-6.0)
method on the property or field type, or on the type that is marked with this
attribute.

Methods marked with this attribute must specify a contract name or a type by
using either
[ExportAttribute(String)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-string))
or
[ExportAttribute(Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute.-ctor?view=dotnet-plat-ext-6.0#system-componentmodel-composition-exportattribute-ctor(system-type)).

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

## ExportAttribute(String, Type)

Initializes a new instance of the
[ExportAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute?view=dotnet-plat-ext-6.0)
class, exporting the specified type under the specified contract name.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ CSharp
public ExportAttribute (string? contractName, Type? contractType);
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Parameters

contractName

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string?view=dotnet-plat-ext-6.0)

The contract name that is used to export the type or member marked with this
attribute, or null or an empty string ("") to use the default contract name.

contractType

[Type](https://docs.microsoft.com/en-us/dotnet/api/system.type?view=dotnet-plat-ext-6.0)

The type to export.

Remarks

The default contract name is the result of calling
[AttributedModelServices.GetContractName(Type)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.attributedmodelservices.getcontractname?view=dotnet-plat-ext-6.0#system-componentmodel-composition-attributedmodelservices-getcontractname(system-type))
on the property or field type, or on the type itself that this is marked with
this attribute.

The contract name is compared using a case-sensitive, non-linguistic comparison
using
[StringComparer.Ordinal](https://docs.microsoft.com/en-us/dotnet/api/system.stringcomparer.ordinal?view=dotnet-plat-ext-6.0#system-stringcomparer-ordinal).

Applies to

.NET Framework 4.8 and other versions

| **TABLE 5**                  |                                                                   |
|------------------------------|-------------------------------------------------------------------|
| **Product**                  | **Versions**                                                      |
| **.NET Framework**           | 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8 |
| **.NET Platform Extensions** | 2.1, 2.2, 3.0, 3.1, 5.0, 6.0                                      |
| **Xamarin.iOS**              | 10.8                                                              |
| **Xamarin.Mac**              | 3.0                                                               |
