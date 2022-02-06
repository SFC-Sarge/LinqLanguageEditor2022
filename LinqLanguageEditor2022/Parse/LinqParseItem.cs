using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Text;

using System.Collections.Generic;
using System.Linq;

namespace LinqLanguageEditor2022.Parse
{
    public class LinqParseItem
    {
        public HashSet<LinqError> _errors = new();

        public LinqParseItem(int start, string text, LinqDocument document, LinqTokenTypes type)
        {
            Text = text;
            Document = document;
            Type = type;
            Span = new Span(start, Text.Length);
        }

        public List<LinqParseItem> Children = new();

        public LinqTokenTypes Type { get; }
        public TokenInfo _TokenInfo { get; }

        public virtual Span Span { get; }

        public virtual string Text { get; }

        public LinqDocument Document { get; }

        public List<LinqParseItem> References { get; } = new();

        public ICollection<LinqError> Errors => _errors;

        public bool IsValid => _errors.Count == 0;

        public LinqParseItem Previous
        {
            get
            {
                int index = Document.Items.IndexOf(this);
                return index > 0 ? Document.Items[index - 1] : null;
            }
        }

        public LinqParseItem Next
        {
            get
            {
                int index = Document.Items.IndexOf(this);
                return Document.Items.ElementAtOrDefault(index + 1);
            }
        }

        public static implicit operator Span(LinqParseItem parseItem)
        {
            return parseItem.Span;
        }

        public override string ToString()
        {
            return Type + " " + Text;
        }

        public override int GetHashCode()
        {
            int hashCode = -1393027003;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Span.Start.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is LinqParseItem item &&
                   Type == item.Type &&
                   EqualityComparer<Span>.Default.Equals(Span, item.Span) &&
                   Text == item.Text;
        }
    }
}
