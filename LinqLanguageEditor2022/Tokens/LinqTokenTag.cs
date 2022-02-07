
namespace LinqLanguageEditor2022.Tokens
{
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    using System.ComponentModel.Composition;

    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.LinqLanguageName)]
    [TagType(typeof(LinqTokenTag))]
    internal sealed class LinqTokenTagProvider : ITaggerProvider
    {

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new LinqTokenTagger(buffer) as ITagger<T>;
        }
    }

    public class LinqTokenTag : ITag
    {
        public LinqTokenTypes type { get; private set; }

        public LinqTokenTag(LinqTokenTypes type)
        {
            this.type = type;
        }
    }
}
