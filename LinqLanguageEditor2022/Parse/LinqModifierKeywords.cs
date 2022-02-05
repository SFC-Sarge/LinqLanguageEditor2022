using System.Linq;

namespace LinqLanguageEditor2022.Parse
{
    public class LinqModifierKeywords
    {
        public static readonly string[] ModifierKeywords = "abstract async const event extern new override partial readonly sealed static unsafe virtual volatile".Split().ToArray();
    }
}
