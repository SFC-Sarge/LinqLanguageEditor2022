using System.Linq;

namespace LinqLanguageEditor2022.Parse
{
    public class LinqNamespaceKeywords
    {
        public static readonly string[] NamespaceKeywords = "using|.|:|extern alias".Split('|').ToArray();
    }
}
