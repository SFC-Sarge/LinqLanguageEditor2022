using System.Linq;

namespace LinqLanguageEditor2022.Parse
{
    public class LinqStatementModifierKeywords
    {
        public static readonly string[] StatementModifierKeywords = "if else switch case do for foreach in while break continue default goto return yield throw try catch finally checked unchecked fixed lock".Split().ToArray();
    }
}
