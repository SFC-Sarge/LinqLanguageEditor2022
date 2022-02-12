
namespace LinqLanguageEditor2022.Tokens
{
    using LinqLanguageEditor2022.Classification;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    using System;
    using System.Collections.Generic;
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

    internal sealed class LinqTokenTagger : ITagger<LinqTokenTag>
    {

        ITextBuffer _buffer;
        IDictionary<string, LinqTokenTypes> _linqTypes;

        internal LinqTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            _linqTypes = new Dictionary<string, LinqTokenTypes>();
            _linqTypes["comment"] = LinqTokenTypes.comment;
            _linqTypes["keyword"] = LinqTokenTypes.keyword;
            _linqTypes["number"] = LinqTokenTypes.number;
            _linqTypes["operator"] = LinqTokenTypes.@operator;
            _linqTypes["string"] = LinqTokenTypes.@string;
            _linqTypes["whitespace"] = LinqTokenTypes.whitespace;
            _linqTypes["punctuation"] = LinqTokenTypes.punctuation;
            _linqTypes["identifier"] = LinqTokenTypes.identifier;
            _linqTypes["separator"] = LinqTokenTypes.separator;
            _linqTypes["literal"] = LinqTokenTypes.literal;
            _linqTypes["unknown"] = LinqTokenTypes.unknown;

        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        //public IEnumerable<ITagSpan<LinqTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        //{

        //    foreach (SnapshotSpan curSpan in spans)
        //    {
        //        ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
        //        int curLoc = containingLine.Start.Position;
        //        string[] tokens = containingLine.GetText().ToLower().Split(' ');

        //        foreach (string linqToken in tokens)
        //        {
        //            if (_linqTypes.ContainsKey(linqToken))
        //            {
        //                var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, linqToken.Length));
        //                if (tokenSpan.IntersectsWith(curSpan))
        //                    yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(_linqTypes[linqToken]));
        //            }

        //            //add an extra char location because of the space
        //            curLoc += linqToken.Length + 1;
        //        }
        //    }

        //}
        public IEnumerable<ITagSpan<LinqTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {

            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int curLoc = containingLine.Start.Position;
                string lineText = containingLine.GetText().ToLower();
                var tokens = SyntaxFactory.ParseTokens(lineText);
                foreach (var token in tokens)
                {
                    string currentToken = LinqClassificationHelpers.GetClassification(token);
                    if (token.Kind() != SyntaxKind.EndOfFileToken)
                    {
                        if (curLoc <= curSpan.Length)
                        {
                            var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, token.ValueText.Length));
                            if (tokenSpan.IntersectsWith(curSpan))
                            {
                                yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag((LinqTokenTypes)Enum.Parse(typeof(LinqTokenTypes), currentToken.ToLower())));
                            }
                        }
                        else
                        {
                            continue;
                        }
                        curLoc += token.ValueText.Length + 1;
                    }
                }
            }

        }
    }
}
