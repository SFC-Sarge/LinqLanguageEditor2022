
namespace LinqLanguageEditor2022.Tokens
{
    using LinqLanguageEditor2022.Classification;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;

    using System;
    using System.Collections.Generic;

    internal sealed class LinqTokenTagger : ITagger<LinqTokenTag>
    {
        ITextBuffer _buffer;
        IDictionary<string, LinqTokenTypes> _LinqTypes;

        internal LinqTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            _LinqTypes = new Dictionary<string, LinqTokenTypes>();
            _LinqTypes[LinqTokenTypes.whitespace.ToString()] = LinqTokenTypes.whitespace;
            _LinqTypes[LinqTokenTypes.number.ToString()] = LinqTokenTypes.number;
            _LinqTypes[LinqTokenTypes.identifier.ToString()] = LinqTokenTypes.identifier;
            _LinqTypes[LinqTokenTypes.comment.ToString()] = LinqTokenTypes.comment;
            _LinqTypes[LinqTokenTypes.@operator.ToString()] = LinqTokenTypes.@operator;
            _LinqTypes[LinqTokenTypes.unknown.ToString()] = LinqTokenTypes.unknown;
            _LinqTypes[LinqTokenTypes.keyword.ToString()] = LinqTokenTypes.keyword;
            _LinqTypes[LinqTokenTypes.punctuation.ToString()] = LinqTokenTypes.punctuation;
            _LinqTypes[LinqTokenTypes.separator.ToString()] = LinqTokenTypes.separator;
            _LinqTypes[LinqTokenTypes.literal.ToString()] = LinqTokenTypes.literal;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<LinqTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {

            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int curLoc = containingLine.Start.Position;
                //string[] tokens = containingLine.GetText().ToLower().Split(' ');
                //string lineText = containingLine.GetText().ToLower().Trim(' ', '\t', '\r', '\n');
                string lineText = containingLine.GetText().ToLower();
                var tokens = SyntaxFactory.ParseTokens(lineText);
                foreach (var token in tokens)
                {
                    string currentToken = ClassificationHelpers.GetClassification(token);
                    if (token.Kind() != SyntaxKind.EndOfFileToken)
                    {
                        //var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(token.FullSpan.Start, token.ValueText.Length));
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, token.ValueText.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                        {
                            yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag((LinqTokenTypes)Enum.Parse(typeof(LinqTokenTypes), currentToken.ToLower())));
                        }
                    }
                    curLoc += token.ValueText.Length + 1;
                }
            }
        }

    }
}
