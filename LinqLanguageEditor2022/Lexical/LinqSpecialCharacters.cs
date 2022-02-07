using System.Linq;

namespace LinqLanguageEditor2022.Lexical
{
    public class LinqSpecialCharacters
    {
        // Special Characters Reference URL: (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/)
        // Interpolated string character $ Reference URL: (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated)
        // Verbatim identifier character @ Reference URL: (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim)
        public static readonly string[] SpecialCharacters = "$ @".Split().ToArray();
    }
}
