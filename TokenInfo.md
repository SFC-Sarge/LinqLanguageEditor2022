# TokenInfo class

<!--Start-Of-TOC-->
   - [Constructors](#Constructors)
      - [Properties](#Properties)
   - [](#)
   - [TokenInfo.EndIndex Property](#TokenInfo.EndIndex-Property)
   - [TokenInfo.StartIndex Property](#TokenInfo.StartIndex-Property)
   - [TokenInfo.Token Property](#TokenInfo.Token-Property)
   - [TokenInfo.Trigger Property](#TokenInfo.Trigger-Property)
   - [TokenInfo.Type Property](#TokenInfo.Type-Property)
   - [TokenColor Enum](#TokenColor-Enum)
      - [TokenColor Fields](#TokenColor-Fields)
- [TokenType Enum](#TokenType-Enum)
   - [TokenTriggers Enum](#TokenTriggers-Enum)
<!--End-Of-TOC-->


Namespace:

Microsoft.VisualStudio.Package

Provides information about a particular token encountered by a language
service's language parser.

Remarks

The
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
scanner uses this class to provide information about each token parsed. This
class identifies the token type, specifies a color index for the token,
specifies the start and end characters of the token (relative to the current
line being parsed), and specifies any triggers that can be handled based on the
token's type. See each property of this class for more details on how and when
these properties are used.

## Constructors

| **CONSTRUCTORS**                                                                                                                                                                                                                                                             |                                                                            |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------|
| [TokenInfo()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokeninfo.-ctor?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokeninfo-ctor)                                                                                            | Initializes a new instance of the TokenInfo class to the default values.   |
| [TokenInfo(Int32, Int32, TokenType)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokeninfo.-ctor?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokeninfo-ctor(system-int32-system-int32-microsoft-visualstudio-package-tokentype)) | Initializes a new instance of the TokenInfo class to the specified values. |

### Properties

| **PROPERTIES**                                                                                                                                                                              |                                                                |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------|
| [Color](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokeninfo.color?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokeninfo-color)                | Determines the color index to use for the token.               |
| [EndIndex](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokeninfo.endindex?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokeninfo-endindex)       | Determines the index of the last character of the token.       |
| [StartIndex](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokeninfo.startindex?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokeninfo-startindex) | Determines the index of the first character of the token.      |
| [Token](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokeninfo.token?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokeninfo-token)                | Language Specific                                              |
| [Trigger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokeninfo.trigger?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokeninfo-trigger)          | Determines the various triggers that can be set for the token. |
| [Type](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokeninfo.type?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokeninfo-type)                   | Determines the type of the token.                              |

## 

## TokenInfo.EndIndex Property

Namespace:

[Microsoft.VisualStudio.Package](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package?view=visualstudiosdk-2022)

Determines the index of the last character of the token.

Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)

Returns an integer specifying the last character of the token.

Remarks

The
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
scanner operates on lines of source, parsing each line for tokens. The character
index specified in EndIndex is an offset into the current line being parsed.

## TokenInfo.StartIndex Property

Namespace:

[Microsoft.VisualStudio.Package](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package?view=visualstudiosdk-2022)

Determines the index of the first character of the token.

Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)

Returns an integer specifying the first character of the token.

Remarks

The
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
scanner operates on lines of source, parsing each line for tokens. The character
index specified in StartIndex is an offset into the current line being parsed.

## TokenInfo.Token Property

Namespace:

[Microsoft.VisualStudio.Package](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package?view=visualstudiosdk-2022)

Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)

Returns an integer specifying the token ID.

Remarks

The token ID represents a particular language token and is specific to the
scanner or parser used. This could be used, for example, to quickly find a
specific token in a parse tree if more information about that token is needed
than can be represented in the TokenInfo class.

The managed package framework language service classes do not use this property.

## TokenInfo.Trigger Property

Namespace:

[Microsoft.VisualStudio.Package](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package?view=visualstudiosdk-2022)

Assembly:

Determines the various triggers that can be set for the token.

Property Value

[TokenTriggers](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokentriggers?view=visualstudiosdk-2022)

Returns a combination of flags from the TokenTriggers enumeration.

Remarks

A token trigger is used to signal a particular language feature in support of
IntelliSense. For example, MatchBraces can be returned when a closing brace is
parsed. This initiates or triggers the matching brace feature. Note that
triggers can be returned all the time; however, they are used only when a
particular parsing operation has been done (see the ParseReason enumeration for
the different parsing operations supported).

## TokenInfo.Type Property

Namespace:

[Microsoft.VisualStudio.Package](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package?view=visualstudiosdk-2022)

Determines the type of the token.

Property Value

[TokenType](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokentype?view=visualstudiosdk-2022)

Returns a value from the TokenType enumeration.

Remarks

Your scanner should adhere to the meaning of the existing values in the
TokenType enumeration as the default managed package framework language service
classes use those values in many places. You can add additional types as
necessary but use the existing types first.

## TokenColor Enum

Namespace:

Microsoft.VisualStudio.Package

Provides initial values for color indices as reported by an
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
scanner.

### TokenColor Fields

Fields

| **FIELDS** |   |                                                                                                                                                                      |
|------------|---|----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Comment    | 2 | = 2. Represents the color for a comment.                                                                                                                             |
| Identifier | 3 | = 3. Represents the color for an identifier or name (for example, a class, method, or variable name).                                                                |
| Keyword    | 1 | = 1. Represents the color for a language keyword (for example, "for", "if", or "else").                                                                              |
| Number     | 5 | = 5. Represents the color for a number (a string of decimal or hexadecimal digits).                                                                                  |
| String     | 4 | = 4. Represents the color for a string, typically bounded by single or double quotes.                                                                                |
| Text       | 0 | = 0. The default. This represents the color corresponding to the user's default text colors (the language service's ColorableItem list is ignored in this one case). |

Remarks

To support syntax highlighting, the language service scanner must identify each
language element as a token and provide a color value for that token. This value
is actually an index into a ColorableItem list. The language service can supply
a custom list of ColorableItem objects or the language service can rely on a
default list of ColorableItem objects supplied by Visual Studio. The index into
either list has the type TokenColor.

If you are supplying custom ColorableItem objects from your language service, it
is recommended that you adhere to the meaning of the labels for the first six
token types in your language. However, you can readily expand on the choices
here, adding additional elements as needed. Note that the first colorable item
is always ignored as Visual Studio supplies its own values for plain text.

# TokenType Enum

Definition

Namespace:

Microsoft.VisualStudio.Package

Specifies the different types of tokens that can be identified and returned from
a language service scanner.

Fields

| Comment     | 10 | A block comment. For example, in C\# or C++, a comment is bounded by /\* and \*/. In XML, the comment is bounded by \<!-- and --\>.                                                                                                                            |
|-------------|----|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Delimiter   | 7  | A token that operates as a separator between two language elements. For example, in C\#, the period "." between class name and member name. In XML, the angle brackets surrounding a tag, \< and \>.                                                           |
| Identifier  | 3  | An identifier or name. For example, the name of a variable, method, or class. In XML, this could be the name of a tag or attribute.                                                                                                                            |
| Keyword     | 2  | A language keyword, an identifier that is reserved by the language. For example, in C\#, do, while, foreach, if, and else, are all keywords.                                                                                                                   |
| LineComment | 9  | A line comment (comment is terminated at the end of the line). For example, in C\# or C++, a comment is preceded by a //. In Visual Basic, this is a single tick '.                                                                                            |
| Literal     | 5  | A literal value (a character or number). For example, in C\# or C++, this is a character bounded by single quotes, or a decimal or hexadecimal number.                                                                                                         |
| Operator    | 6  | A punctuation character that has a specific meaning in a language. For example, in C\#, arithmetic operators +, -, \*, and /. In C++, pointer dereference operator -\>, insertion operator \>\>, and extraction operation \<\<. In XML, assignment operator =. |
| String      | 4  | A string. Typically defined as zero or more characters bounded by double quotes.                                                                                                                                                                               |
| Text        | 1  | General text; any text not identified as a specified token type.                                                                                                                                                                                               |
| Unknown     | 0  | The token is an unknown type. This is typically used for any token not recognized by the parser and should be considered an error in the code being parsed.                                                                                                    |
| WhiteSpace  | 8  | A space, tab, or newline. Typically, a contiguous run of any whitespace is considered a single whitespace token. For example, the three spaces in "name this" would be treated as one whitespace token.                                                        |

## TokenTriggers Enum

Namespace:

[Microsoft.VisualStudio.Package](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package?view=visualstudiosdk-2022)

TokenTriggers: If a character has (a) trigger(s) associated with it, it may fire
one or both of the following triggers: MemberSelect - a member selection tip
window MatchBraces - highlight matching braces or the following trigger:
MethodTip - a method tip window

The following triggers exist for speed reasons: the (fast) lexer determines when
a (slow) parse might be needed. The Trigger.MethodTip trigger is subdivided in 4
other triggers. It is the best to be as specific as possible; it is better to
return Trigger.ParamStart than Trigger.Param (or Trigger.MethodTip)

This enumeration has a FlagsAttribute attribute that allows a bitwise
combination of its member values.

Inheritance

[Enum](https://msdn.microsoft.com/en-us/library/system.enum(v=vs.110).aspx)

TokenTriggers

Attributes

[FlagsAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute)

Fields

| **FIELDS**     |     |                                                                                                                                                                                                                                                                                                                                                                     |
|----------------|-----|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| MatchBraces    | 2   | The opening or closing part of a language pair has been parsed. For example, in C\#, a { or } has been parsed. In XML, a \< or \> has been parsed.                                                                                                                                                                                                                  |
| MemberSelect   | 1   | A character that indicates that the start of a member selection has been parsed. In C\#, this could be a period following a class name. In XML, this could be a \< (the member select is a list of possible tags).                                                                                                                                                  |
| MethodTip      | 240 | This is a mask for the flags used to govern the IntelliSense Method Tip operation. This mask is used to isolate the values [Parameter](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.tokentriggers?view=visualstudiosdk-2022#microsoft-visualstudio-package-tokentriggers-parameter), ParameterStart, ParameterNext, and ParameterEnd. |
| None           | 0   | Used when no triggers are set. This is the default.                                                                                                                                                                                                                                                                                                                 |
| Parameter      | 128 | A parameter in a method's parameter list has been parsed.                                                                                                                                                                                                                                                                                                           |
| ParameterEnd   | 64  | A character that marks the end of a parameter list has been parsed. For example, in C\#, this could be a close parenthesis, ")".                                                                                                                                                                                                                                    |
| ParameterNext  | 32  | A character that separates parameters in a list has been parsed. For example, in C\#, this could be a comma, ",".                                                                                                                                                                                                                                                   |
| ParameterStart | 16  | A character that marks the start of a parameter list has been parsed. For example, in C\#, this could be an open parenthesis, "(".                                                                                                                                                                                                                                  |

Remarks

Triggers provide a way for the language service's
[IScanner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.package.iscanner?view=visualstudiosdk-2022)
scanner to signal the caller about certain language elements that may be of
interest to IntelliSense support. These triggers can be returned all the time;
however, they are used only in certain parsing operation contexts (see the
ParseReason enumeration for more information about the different types of
parsing operations).

For example, the user types a closing brace and the scanner is called to examine
the line the brace is on. The brace is parsed and the scanner sets the trigger
for that token to MatchBraces. The caller sees this trigger and calls the
ParseSource method parser with the parse reason HighlightBraces. This causes the
parser to look for the matching opening brace and return the location of both
braces. The editor can then highlight the two braces.
