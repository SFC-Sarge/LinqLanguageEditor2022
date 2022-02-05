using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell.Interop;
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

    public class LinqError
    {
        public LinqError(string errorCode, string message, string category, __VSERRORCATEGORY severity)
        {
            ErrorCode = errorCode;
            Message = message;
            Category = category;
            Severity = severity;
        }

        public string ErrorCode { get; }
        public string Message { get; }
        public string Category { get; }
        public __VSERRORCATEGORY Severity { get; }

        public LinqError WithFormat(params string[] replacements)
        {
            return new LinqError(ErrorCode, string.Format(Message, replacements), Category, Severity);
        }
    }
}
