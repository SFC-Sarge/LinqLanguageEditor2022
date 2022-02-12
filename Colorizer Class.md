# Colorizer Class

Important

This API is not CLS-compliant.

This class implements the IVsColorizer interface and is used to support syntax
highlighting in an editor.

Inheritance

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)

Colorizer

Attributes

[ComVisibleAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.comvisibleattribute)

Implements

[IVsColorizer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivscolorizer?view=visualstudiosdk-2022)
IDisposable

## Remarks

The managed package framework (MPF) version of this class uses an
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
object to handle all parsing tasks. The
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
object communicates color information through the TokenInfo structure. The
[Colorizer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer?view=visualstudiosdk-2022)
class also helps the
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
object track state by passing around a state variable the parser maintains.

This class handles colorization on a line by line basis.

## Notes to Inheritors

The MPF version of this class performs all the work necessary to colorize a line
of code by interacting with the
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
object. If you find you need additional functionality not supported in the
existing
[Colorizer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer?view=visualstudiosdk-2022)
class, you must derive a class from the
[Colorizer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer?view=visualstudiosdk-2022)
class and return an instance of your class from GetColorizer(IVsTextLines).

The default implementation of GetColorizer(IVsTextLines) instantiates the MPF
version of
[Colorizer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer?view=visualstudiosdk-2022),
passing to the
[Colorizer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer?view=visualstudiosdk-2022)
constructor an instance of the
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
object obtained from GetScanner(IVsTextLines).

## Notes to Callers

The colorizer object returned from GetColorizer(IVsTextLines) is stored in the
[Source](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.source?view=visualstudiosdk-2022)
object when the
[Source](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.source?view=visualstudiosdk-2022)
object is created. The
[Source](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.source?view=visualstudiosdk-2022)
object handles all interactions with the colorizer so there is no need for any
outside involvement with the colorizer.

The methods of this class are documented in case you need to implement your own
version of the
[Source](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.source?view=visualstudiosdk-2022)
class.

## Constructors

| CONSTRUCTORS                                                                                                                                                                                                                                                                                                                                                             |                                                                                                                                                    |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------|
| [Colorizer(LanguageService, IVsTextLines, IScanner)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.-ctor?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-ctor(microsoft-visualstudio-package-languageservice-microsoft-visualstudio-textmanager-interop-ivstextlines-microsoft-visualstudio-package-iscanner)) | Initializes the [Colorizer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer?view=visualstudiosdk-2022) class. |

## Properties

| PROPERTIES                                                                                                                                                                         |                                 |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------|
| [Scanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.scanner?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-scanner) | Returns the scanner being used. |

## Methods

| METHODS                                                                                                                                                                                                                                                                                                                                                                   |                                                                                                  |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------|
| [CloseColorizer()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.closecolorizer?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-closecolorizer)                                                                                                                                                                 | Called when the colorizer is disposed of.                                                        |
| [ColorizeLine(Int32, Int32, IntPtr, Int32, UInt32[])](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.colorizeline?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-colorizeline(system-int32-system-int32-system-intptr-system-int32-system-uint32()))                                                            | Obtains color and font attribute information for each character in the specified line of text.   |
| [Dispose()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.dispose?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-dispose)                                                                                                                                                                                      | Disposes the object.                                                                             |
| [Finalize()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.finalize?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-finalize)                                                                                                                                                                                   | Called when the object is about to be destroyed.                                                 |
| [GetColorInfo(String, Int32, Int32)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.getcolorinfo?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-getcolorinfo(system-string-system-int32-system-int32))                                                                                                          | Returns the parsing state at the end of the line without returning any colorization information. |
| [GetLineInfo(IVsTextLines, Int32, IVsTextColorState)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.getlineinfo?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-getlineinfo(microsoft-visualstudio-textmanager-interop-ivstextlines-system-int32-microsoft-visualstudio-textmanager-interop-ivstextcolorstate)) | Returns color information about the specified line.                                              |
| [GetStartState(Int32)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.getstartstate?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-getstartstate(system-int32@))                                                                                                                                                | Returns the initial parsing state.                                                               |
| [GetStateAtEndOfLine(Int32, Int32, IntPtr, Int32)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.getstateatendofline?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-getstateatendofline(system-int32-system-int32-system-intptr-system-int32))                                                                 | Returns the parsing state at the end of the specified line.                                      |
| [GetStateMaintenanceFlag(Int32)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.getstatemaintenanceflag?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-getstatemaintenanceflag(system-int32@))                                                                                                                  | Called to determine if the colorizer requires per line state management.                         |
| [Resume()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.resume?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-resume)                                                                                                                                                                                         | Called to resume use of the colorizer.                                                           |
| [Suspend()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.colorizer.suspend?view=visualstudiosdk-2022#microsoft-visualstudio-package-colorizer-suspend)                                                                                                                                                                                      | Called to suspend use of the colorizer.                                                          |
