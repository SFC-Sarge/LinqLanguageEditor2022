using System.Linq;

namespace LinqLanguageEditor2022.Parse
{
    public class LinqSeparators
    {
        // Query Keywords reference URL: (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/query-keywords)
        public static readonly string[] Separators = "\r \n \r\n".Split().ToArray();
    }
}
