using System.Linq;

namespace LinqLanguageEditor2022.Parse
{
    public class LinqOperatorKeywords
    {
        public static readonly string[] OperatorKeywords = "as await is new sizeof typeof stackalloc checked unchecked".Split().ToArray();
    }
}
