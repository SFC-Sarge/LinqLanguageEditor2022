
namespace LinqLanguageEditor2022.Parse
{
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
            _LinqTypes["0"] = LinqTokenTypes.Number;
            _LinqTypes["1"] = LinqTokenTypes.Number;
            _LinqTypes["2"] = LinqTokenTypes.Number;
            _LinqTypes["3"] = LinqTokenTypes.Number;
            _LinqTypes["4"] = LinqTokenTypes.Number;
            _LinqTypes["5"] = LinqTokenTypes.Number;
            _LinqTypes["6"] = LinqTokenTypes.Number;
            _LinqTypes["7"] = LinqTokenTypes.Number;
            _LinqTypes["8"] = LinqTokenTypes.Number;
            _LinqTypes["9"] = LinqTokenTypes.Number;
            _LinqTypes["debug"] = LinqTokenTypes.Identifier;
            _LinqTypes["console"] = LinqTokenTypes.Identifier;
            _LinqTypes["result"] = LinqTokenTypes.Identifier;
            _LinqTypes["///"] = LinqTokenTypes.Comment;
            _LinqTypes["//"] = LinqTokenTypes.Comment;
            _LinqTypes["/*"] = LinqTokenTypes.Comment;
            _LinqTypes["*/"] = LinqTokenTypes.Comment;
            _LinqTypes["*/"] = LinqTokenTypes.Comment;
            _LinqTypes["+"] = LinqTokenTypes.Operator;
            _LinqTypes["-"] = LinqTokenTypes.Operator;
            _LinqTypes["*"] = LinqTokenTypes.Operator;
            _LinqTypes["/"] = LinqTokenTypes.Operator;
            _LinqTypes["%"] = LinqTokenTypes.Operator;
            _LinqTypes["&"] = LinqTokenTypes.Operator;
            _LinqTypes["("] = LinqTokenTypes.Operator;
            _LinqTypes[")"] = LinqTokenTypes.Operator;
            _LinqTypes["["] = LinqTokenTypes.Operator;
            _LinqTypes["]"] = LinqTokenTypes.Operator;
            _LinqTypes["|"] = LinqTokenTypes.Operator;
            _LinqTypes["^"] = LinqTokenTypes.Operator;
            _LinqTypes["!"] = LinqTokenTypes.Operator;
            _LinqTypes["~"] = LinqTokenTypes.Operator;
            _LinqTypes["&&"] = LinqTokenTypes.Operator;
            _LinqTypes["||"] = LinqTokenTypes.Operator;
            _LinqTypes[","] = LinqTokenTypes.Operator;
            _LinqTypes["++"] = LinqTokenTypes.Operator;
            _LinqTypes["--"] = LinqTokenTypes.Operator;
            _LinqTypes["<<"] = LinqTokenTypes.Operator;
            _LinqTypes[">>"] = LinqTokenTypes.Operator;
            _LinqTypes["=="] = LinqTokenTypes.Operator;
            _LinqTypes["!="] = LinqTokenTypes.Operator;
            _LinqTypes["<"] = LinqTokenTypes.Operator;
            _LinqTypes[">"] = LinqTokenTypes.Operator;
            _LinqTypes["<="] = LinqTokenTypes.Operator;
            _LinqTypes[">="] = LinqTokenTypes.Operator;
            _LinqTypes["="] = LinqTokenTypes.Operator;
            _LinqTypes["+="] = LinqTokenTypes.Operator;
            _LinqTypes["-="] = LinqTokenTypes.Operator;
            _LinqTypes["*="] = LinqTokenTypes.Operator;
            _LinqTypes["/="] = LinqTokenTypes.Operator;
            _LinqTypes["%="] = LinqTokenTypes.Operator;
            _LinqTypes["&="] = LinqTokenTypes.Operator;
            _LinqTypes["|="] = LinqTokenTypes.Operator;
            _LinqTypes["^="] = LinqTokenTypes.Operator;
            _LinqTypes["<<="] = LinqTokenTypes.Operator;
            _LinqTypes[">>="] = LinqTokenTypes.Operator;
            _LinqTypes["."] = LinqTokenTypes.Operator;
            _LinqTypes["[]"] = LinqTokenTypes.Operator;
            _LinqTypes["()"] = LinqTokenTypes.Operator;
            _LinqTypes["?:"] = LinqTokenTypes.Operator;
            _LinqTypes["=>"] = LinqTokenTypes.Operator;
            _LinqTypes["??"] = LinqTokenTypes.Operator;
            _LinqTypes["$"] = LinqTokenTypes.String;
            _LinqTypes["@"] = LinqTokenTypes.String;
            _LinqTypes["Unknown"] = LinqTokenTypes.Unknown;
            _LinqTypes["using"] = LinqTokenTypes.Keyword;
            _LinqTypes[":"] = LinqTokenTypes.Keyword;
            _LinqTypes["namespace"] = LinqTokenTypes.Keyword;
            _LinqTypes["extern alias"] = LinqTokenTypes.Keyword;
            _LinqTypes["as"] = LinqTokenTypes.Keyword;
            _LinqTypes["await"] = LinqTokenTypes.Keyword;
            _LinqTypes["is"] = LinqTokenTypes.Keyword;
            _LinqTypes["new"] = LinqTokenTypes.Keyword;
            _LinqTypes["sizeof"] = LinqTokenTypes.Keyword;
            _LinqTypes["typeof"] = LinqTokenTypes.Keyword;
            _LinqTypes["stackalloc"] = LinqTokenTypes.Keyword;
            _LinqTypes["checked"] = LinqTokenTypes.Keyword;
            _LinqTypes["unchecked"] = LinqTokenTypes.Keyword;
            _LinqTypes["public"] = LinqTokenTypes.Keyword;
            _LinqTypes["private"] = LinqTokenTypes.Keyword;
            _LinqTypes["internal"] = LinqTokenTypes.Keyword;
            _LinqTypes["protected"] = LinqTokenTypes.Keyword;
            _LinqTypes["params"] = LinqTokenTypes.Keyword;
            _LinqTypes["ref"] = LinqTokenTypes.Keyword;
            _LinqTypes["out"] = LinqTokenTypes.Keyword;
            _LinqTypes["abstract"] = LinqTokenTypes.Keyword;
            _LinqTypes["async"] = LinqTokenTypes.Keyword;
            _LinqTypes["const"] = LinqTokenTypes.Keyword;
            _LinqTypes["event"] = LinqTokenTypes.Keyword;
            _LinqTypes["extern"] = LinqTokenTypes.Keyword;
            _LinqTypes["new"] = LinqTokenTypes.Keyword;
            _LinqTypes["override"] = LinqTokenTypes.Keyword;
            _LinqTypes["partial"] = LinqTokenTypes.Keyword;
            _LinqTypes["readonly"] = LinqTokenTypes.Keyword;
            _LinqTypes["sealed"] = LinqTokenTypes.Keyword;
            _LinqTypes["static"] = LinqTokenTypes.Keyword;
            _LinqTypes["void"] = LinqTokenTypes.Keyword;
            _LinqTypes["unsafe"] = LinqTokenTypes.Keyword;
            _LinqTypes["virtual"] = LinqTokenTypes.Keyword;
            _LinqTypes["volatile"] = LinqTokenTypes.Keyword;
            _LinqTypes["if"] = LinqTokenTypes.Keyword;
            _LinqTypes["else"] = LinqTokenTypes.Keyword;
            _LinqTypes["switch"] = LinqTokenTypes.Keyword;
            _LinqTypes["case"] = LinqTokenTypes.Keyword;
            _LinqTypes["do"] = LinqTokenTypes.Keyword;
            _LinqTypes["for"] = LinqTokenTypes.Keyword;
            _LinqTypes["foreach"] = LinqTokenTypes.Keyword;
            _LinqTypes["in"] = LinqTokenTypes.Keyword;
            _LinqTypes["while"] = LinqTokenTypes.Keyword;
            _LinqTypes["break"] = LinqTokenTypes.Keyword;
            _LinqTypes["continue"] = LinqTokenTypes.Keyword;
            _LinqTypes["default"] = LinqTokenTypes.Keyword;
            _LinqTypes["goto"] = LinqTokenTypes.Keyword;
            _LinqTypes["return"] = LinqTokenTypes.Keyword;
            _LinqTypes["yield"] = LinqTokenTypes.Keyword;
            _LinqTypes["throw"] = LinqTokenTypes.Keyword;
            _LinqTypes["try"] = LinqTokenTypes.Keyword;
            _LinqTypes["catch"] = LinqTokenTypes.Keyword;
            _LinqTypes["finally"] = LinqTokenTypes.Keyword;
            _LinqTypes["checked"] = LinqTokenTypes.Keyword;
            _LinqTypes["unchecked"] = LinqTokenTypes.Keyword;
            _LinqTypes["fixed"] = LinqTokenTypes.Keyword;
            _LinqTypes["int"] = LinqTokenTypes.Keyword;
            _LinqTypes["double"] = LinqTokenTypes.Keyword;
            _LinqTypes["string"] = LinqTokenTypes.Keyword;
            _LinqTypes["var"] = LinqTokenTypes.Keyword;
            _LinqTypes["lock"] = LinqTokenTypes.Keyword;
            _LinqTypes[" "] = LinqTokenTypes.WhiteSpace;
            _LinqTypes[","] = LinqTokenTypes.Punctuation;
            _LinqTypes["   "] = LinqTokenTypes.Literal;
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
                string[] tokens = containingLine.GetText().ToLower().Split(' ');

                foreach (string LinqToken in tokens)
                {
                    string linqToken = LinqToken.Trim(' ', '\t', '\r', '\n');
                    if (!_LinqTypes.ContainsKey(linqToken))
                    {
                        if (LinqToken.Contains("int") && linqToken.Contains("[]"))
                        {
                            string[] tempArray = { "int", "[]" };
                            foreach (string LinqTemp in tempArray)
                            {
                                var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, LinqTemp.Length));
                                if (tokenSpan.IntersectsWith(curSpan))
                                    yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(_LinqTypes[LinqTemp]));
                            }
                        }
                        else if (linqToken.Contains("string") && linqToken.Contains("[]"))
                        {
                            string[] tempArray = { "string", "[]" };
                            foreach (string LinqTemp in tempArray)
                            {
                                var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, LinqTemp.Length));
                                if (tokenSpan.IntersectsWith(curSpan))
                                    yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(_LinqTypes[LinqTemp]));
                            }
                        }
                        else if (linqToken.Contains(",") && linqToken.EndsWith(",") && linqToken.Length > 1)
                        {
                            string[] tempArray = linqToken.Split(',');
                            if (int.TryParse(tempArray[0], out _))
                            {
                                foreach (string LinqTemp in tempArray)
                                {
                                    if (LinqTemp == ",")
                                    {
                                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, LinqTemp.Length));
                                        if (tokenSpan.IntersectsWith(curSpan))
                                            yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.Punctuation));
                                    }
                                    else if (int.TryParse(LinqTemp, out _))
                                    {
                                        if (linqToken.Length > 0)
                                        {
                                            var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, LinqTemp.Length));
                                            if (tokenSpan.IntersectsWith(curSpan))
                                                yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.Number));
                                        }
                                    }
                                    else
                                    {
                                        if (linqToken.Length > 0)
                                        {
                                            var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, LinqTemp.Length));
                                            if (tokenSpan.IntersectsWith(curSpan))
                                                yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.Identifier));
                                        }
                                    }
                                }
                            }
                            else if (linqToken.Contains(".aggregate") || linqToken.Contains(".where") || linqToken.Contains(".all") || linqToken.Contains(".any") || linqToken.Contains(".startswith") || linqToken.Contains(".distinct") || linqToken.Contains(".writeLine"))
                            {
                                if (LinqToken.Length > 0)
                                {
                                    var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, linqToken.Length));
                                    if (tokenSpan.IntersectsWith(curSpan))
                                        yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.QueryFilter));
                                }
                            }
                        }
                        else if (linqToken.Contains(".aggregate") || linqToken.Contains(".where") || linqToken.Contains(".all") || linqToken.Contains(".any") || linqToken.Contains(".startswith") || linqToken.Contains(".distinct") || linqToken.Contains(".writeLine"))
                        {
                            if (LinqToken.Length > 0)
                            {
                                var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, linqToken.Length));
                                if (tokenSpan.IntersectsWith(curSpan))
                                    yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.QueryFilter));
                            }
                        }
                        else if (int.TryParse(linqToken, out _))
                        {
                            if (LinqToken.Length > 0)
                            {
                                var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, linqToken.Length));
                                if (tokenSpan.IntersectsWith(curSpan))
                                    yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.Number));
                            }
                        }
                        else
                        {
                            if (linqToken.Length > 0)
                            {
                                var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, linqToken.Length));
                                if (tokenSpan.IntersectsWith(curSpan))
                                    yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.Identifier));
                            }
                        }
                    }
                    else if (linqToken.StartsWith("debug") || linqToken.StartsWith("console") || linqToken.Contains("result") || linqToken.Contains("number"))
                    {
                        if (LinqToken.Length > 0)
                        {
                            var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, linqToken.Length));
                            if (tokenSpan.IntersectsWith(curSpan))
                                yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.Identifier));
                        }
                    }
                    else if (linqToken.Contains(".writeLine"))
                    {
                        if (LinqToken.Length > 0)
                        {
                            var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, linqToken.Length));
                            if (tokenSpan.IntersectsWith(curSpan))
                                yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(LinqTokenTypes.QueryFilter));
                        }
                    }
                    else if (_LinqTypes.ContainsKey(linqToken))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, linqToken.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<LinqTokenTag>(tokenSpan, new LinqTokenTag(_LinqTypes[linqToken]));
                    }
                    //add an extra char location because of the space
                    curLoc += linqToken.Length + 1;
                }
            }
        }
    }
}
