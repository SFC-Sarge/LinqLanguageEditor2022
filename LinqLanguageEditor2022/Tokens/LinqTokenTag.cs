
namespace LinqLanguageEditor2022.Tokens
{
    using Microsoft.VisualStudio.Text.Tagging;
    public class LinqTokenTag : ITag
    {
        public LinqTokenTypes type { get; private set; }

        public LinqTokenTag(LinqTokenTypes type)
        {
            this.type = type;
        }
    }
}
