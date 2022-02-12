# ProvideEditorExtensionAttribute Class

<!--Start-Of-TOC-->
   - [Inheritance](#Inheritance)
   - [Remarks](#Remarks)
   - [When to Call](#When-to-Call)
   - [Registry Entries](#Registry-Entries)
   - [Note](#Note)
   - [Constructors](#Constructors)
   - [Properties](#Properties)
   - [Methods](#Methods)
<!--End-Of-TOC-->


Associates a file extension to a given editor factory when applied to a class
that inherits from
[Package](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.package?view=visualstudiosdk-2022)
or implements the interface IVsPackage.

## Inheritance

[Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute)

[RegistrationAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.registrationattribute?view=visualstudiosdk-2022)

[ProvideEditorAttributeBase](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorattributebase?view=visualstudiosdk-2022)

ProvideEditorExtensionAttribute

[AttributeUsageAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.attributeusageattribute)

## Remarks

This attribute associates a file extension with a given editor factory. The file
extension should include the prefixing ".". The editor factory may be specified
as either a GUID or a type.

This attribute also associates a priority with the editor factory. For a given
file extension, the editor with the highest priority is given the chance to read
a file first. If the editor fails to read the file, the remaining editors are
used in order of priority. To make a given editor the default, assign a priority
greater than 0x60.

This attribute class is only used to provide data for external registration
tools. It does not affect runtime behavior.

## When to Call

Apply this file extension attribute to a package class that implements an editor
factory. The package class must inherit from
[Package](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.package?view=visualstudiosdk-2022)
or IVsPackage.

## Registry Entries

The following registry entry is created by this attribute:

*VSROOT*\\Editors\\*{FactoryGuid}*\\Extensions\\*Extension* = Priority

## Note

The GUIDs for the Visual C\# and Visual Basic project types are
{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC} and
{F184B08F-C81C-45F6-A57F-5ABD9991F28F}, respectively.

## Constructors

| CONSTRUCTORS                                                                                                                                                                                                                                                                                                   |                                                             |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------|
| [ProvideEditorExtensionAttribute(Object, String, Int32)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.-ctor?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-ctor(system-object-system-string-system-int32)) | Initializes an instance of ProvideEditorExtensionAttribute. |

## Properties

| PROPERTIES                                                                                                                                                                                                                                                     |                                                                                                                                                                           |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [DefaultName](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.defaultname?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-defaultname)                         | Gets or sets the default name of the editor.                                                                                                                              |
| [EditorFactoryNotify](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.editorfactorynotify?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-editorfactorynotify) | Determines whether or not the extension should be registered with a EditorFactoryNotify registry value, which associates a file extension with a specific editor factory. |
| [Extension](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.extension?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-extension)                               | Gets the file extension associated with this editor.                                                                                                                      |
| [Factory](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorattributebase.factory?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorattributebase-factory)                                               | The editor factory guid. (Inherited from ProvideEditorAttributeBase)                                                                                                      |
| [NameResourceID](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.nameresourceid?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-nameresourceid)                | Gets and sets the integer NameResourceID.                                                                                                                                 |
| [Priority](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.priority?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-priority)                                  | Gets the editor's priority as set in the constructor.                                                                                                                     |
| [ProjectGuid](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.projectguid?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-projectguid)                         | Sets and gets the GUID of the project associated with this editor.                                                                                                        |
| [TemplateDir](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.templatedir?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-templatedir)                         | Gets or sets the template directory to be used by the editor factory to retrieve its source files.                                                                        |
| [TypeId](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.registrationattribute.typeid?view=visualstudiosdk-2022#microsoft-visualstudio-shell-registrationattribute-typeid)                                                            | Gets the current instance of this attribute. (Inherited from RegistrationAttribute)                                                                                       |

## Methods

| METHODS                                                                                                                                                                                                                                                                                                                                                |                                                                                                                                                                                                                   |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [GetPackageRegKeyPath(Guid)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.registrationattribute.getpackageregkeypath?view=visualstudiosdk-2022#microsoft-visualstudio-shell-registrationattribute-getpackageregkeypath(system-guid))                                                                                       | Gets the registry path (relative to the registry root of the application) of the VSPackage. (Inherited from RegistrationAttribute)                                                                                |
| [Register(RegistrationAttribute+RegistrationContext)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.register?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-register(microsoft-visualstudio-shell-registrationattribute-registrationcontext))       | Called to register this attribute with the given context. The context contains the location where the registration information should be placed. It also contains the type being registered and path information. |
| [Unregister(RegistrationAttribute+RegistrationContext)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.provideeditorextensionattribute.unregister?view=visualstudiosdk-2022#microsoft-visualstudio-shell-provideeditorextensionattribute-unregister(microsoft-visualstudio-shell-registrationattribute-registrationcontext)) | Removes the registration information about a VSPackage when called by an external registration tool such as RegPkg.exe. For more information, see Registering VSPackages.                                         |
