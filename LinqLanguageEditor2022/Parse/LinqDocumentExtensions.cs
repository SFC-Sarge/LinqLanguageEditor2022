using Microsoft.VisualStudio.Text;

using System.Linq;


namespace LinqLanguageEditor2022.Parse
{
    public static class LinqDocumentExtensions
    {
        public static LinqDocument GetDocument(this ITextBuffer buffer)
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new LinqDocument(buffer));
        }
    }
}
