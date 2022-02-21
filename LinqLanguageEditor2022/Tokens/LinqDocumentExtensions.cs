using LinqLanguageEditor2022.LinqEditor;

using Microsoft.VisualStudio.Text;

using System.Linq;


namespace LinqLanguageEditor2022.Tokens
{
    public static class LinqDocumentExtensions
    {
        public static Span ToSpan(this SnapshotSpan span)
        {
            return Span.FromBounds(span.Start.Position, span.End.Position + 1);
        }

        public static LinqDocument GetDocument(this ITextBuffer buffer)
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new LinqDocument(buffer));
        }
    }
}
