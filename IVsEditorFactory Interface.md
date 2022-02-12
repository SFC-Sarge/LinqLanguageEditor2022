# IVsEditorFactory Interface

### Reference

<!--Start-Of-TOC-->
      - [Derived](#Derived)
      - [Attributes](#Attributes)
      - [Remarks](#Remarks)
   - [Methods](#Methods)
<!--End-Of-TOC-->


Creates instances of document view objects and of data objects.

### Derived

[Microsoft.VisualStudio.Modeling.Shell.ModelingEditorFactory](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.modeling.shell.modelingeditorfactory?view=visualstudiosdk-2022)

[Microsoft.VisualStudio.Package.EditorFactory](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.editorfactory?view=visualstudiosdk-2022)

### Attributes

[GuidAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.guidattribute)
InterfaceTypeAttribute

### Remarks

An editor factory acts like an OLE IClassFactory for instantiating document view
objects and document data objects in an editor. The editor factory architecture
allows you to create editors that support data/view separation (for example, an
editor could support the Window.NewWindow functionality). For more information,
see [How to: Register an Editor
Factory](http://msdn.microsoft.com/en-us/b3a58e9c-6bee-4e88-86c5-e11075ed6ec1).

The following table lists examples of common tasks using IVsEditorFactory.

| REMARKS                                                            |                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
|--------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| To do this                                                         | See                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| Attach a view to an existing buffer                                | [How to: Attach Views to Document Data](https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-attach-views-to-document-data.md)                                                                                                                                                                                                                                                                                                          |
| Register your editor factory with the environment                  | [How to: Register an Editor Factory](http://msdn.microsoft.com/en-us/b3a58e9c-6bee-4e88-86c5-e11075ed6ec1)                                                                                                                                                                                                                                                                                                                                            |
| Customize the Visual Studio core editor with your language service | [Instantiating the Core Editor By Using the Legacy API](https://docs.microsoft.com/en-us/visualstudio/extensibility/instantiating-the-core-editor-by-using-the-legacy-api.md)  [Inside the Core Editor](https://docs.microsoft.com/en-us/visualstudio/extensibility/inside-the-core-editor.md)  [Developing a Legacy Language Service](https://docs.microsoft.com/en-us/visualstudio/extensibility/internals/developing-a-legacy-language-service.md) |

Implement this interface to support loading your editor in the environment in
response to a third party or the environment calling OpenSpecificEditor or
[OpenStandardEditor](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.ivsuishellopendocument.openstandardeditor?view=visualstudiosdk-2022).

## Methods

| METHODS                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |                                                                                              |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------|
| [Close()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.ivseditorfactory.close?view=visualstudiosdk-2022#microsoft-visualstudio-shell-interop-ivseditorfactory-close)                                                                                                                                                                                                                                                                                                                                        | Releases all cached interface pointers and unregisters any event sinks.                      |
| [CreateEditorInstance(UInt32, String, String, IVsHierarchy, UInt32, IntPtr, IntPtr, IntPtr, String, Guid, Int32)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.ivseditorfactory.createeditorinstance?view=visualstudiosdk-2022#microsoft-visualstudio-shell-interop-ivseditorfactory-createeditorinstance(system-uint32-system-string-system-string-microsoft-visualstudio-shell-interop-ivshierarchy-system-uint32-system-intptr-system-intptr@-system-intptr@-system-string@-system-guid@-system-int32@)) | Used by the editor factory architecture to create editors that support data/view separation. |
| [MapLogicalView(Guid, String)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.ivseditorfactory.maplogicalview?view=visualstudiosdk-2022#microsoft-visualstudio-shell-interop-ivseditorfactory-maplogicalview(system-guid@-system-string@))                                                                                                                                                                                                                                                                    | Maps a logical view to a physical view.                                                      |
| [SetSite(IServiceProvider)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.ivseditorfactory.setsite?view=visualstudiosdk-2022#microsoft-visualstudio-shell-interop-ivseditorfactory-setsite(microsoft-visualstudio-ole-interop-iserviceprovider))                                                                                                                                                                                                                                                             | Initializes an editor in the environment.                                                    |
