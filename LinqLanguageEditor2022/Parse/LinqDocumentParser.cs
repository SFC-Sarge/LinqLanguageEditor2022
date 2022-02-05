
using Microsoft.VisualStudio.Package;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LinqLanguageEditor2022.Parse
{
    public partial class LinqDocument
    {
        private TokenInfo tokenInfo = new();

        //private static readonly Regex _regexProperty = new(@"^(?<name>""[^""]+""|@)(\s)*(?<equals>=)\s*(?<value>((dword:|hex).+|"".+))", RegexOptions.Compiled);
        //private static readonly Regex _regexRef = new(@"\$[\w]+\$?", RegexOptions.Compiled);
        public void Parse()
        {
            int start = 0;

            List<LinqParseItem> items = new();

            foreach (string line in _lines)
            {
                tokenInfo = new();
                IEnumerable<LinqParseItem> current = ParseLine(start, line, items, tokenInfo);
                items.AddRange(current);
                start += line.Length;
            }

            Items = items;

        }

        private IEnumerable<LinqParseItem> ParseLine(int start, string line, List<LinqParseItem> tokens, TokenInfo tokenInfo)
        {
            string trimmedLine = line.Trim();
            List<LinqParseItem> items = new();
            string[] myTokens = trimmedLine.Split(new[] { ' ' });

            foreach (string myToken in myTokens)
            {
                if (LinqNamespaceKeywords.NamespaceKeywords.Any(myToken.Contains))
                {
                    tokenInfo.Color = TokenColor.Keyword;
                    tokenInfo.Type = TokenType.Keyword;
                    tokenInfo.StartIndex = start;
                    items.Add(ToParseItem(line, start, LinqItemType.Keywords, tokenInfo));
                }
                else if (LinqOperatorKeywords.OperatorKeywords.Any(myToken.Contains))
                {
                    tokenInfo.Color = TokenColor.Keyword;
                    tokenInfo.Type = TokenType.Operator;
                    tokenInfo.StartIndex = start;
                    items.Add(ToParseItem(line, start, LinqItemType.Operator, tokenInfo));
                }
                else if (LinqModifierKeywords.ModifierKeywords.Any(myToken.Contains))
                {
                    tokenInfo.Color = TokenColor.Keyword;
                    tokenInfo.Type = TokenType.Keyword;
                    tokenInfo.StartIndex = start;
                    items.Add(ToParseItem(line, start, LinqItemType.Keywords, tokenInfo));
                }
                else if (LinqOperators.Operators.Any(myToken.Contains))
                {
                    tokenInfo.Color = TokenColor.Keyword;
                    tokenInfo.Type = TokenType.Operator;
                    tokenInfo.StartIndex = start;
                    items.Add(ToParseItem(line, start, LinqItemType.Operator, tokenInfo));
                }
                else if (LinqSeparators.Separators.Any(myToken.Contains))
                {
                    tokenInfo.Color = TokenColor.Keyword;
                    tokenInfo.Type = TokenType.Text;
                    tokenInfo.StartIndex = start;
                    items.Add(ToParseItem(line, start, LinqItemType.Identifier, tokenInfo));
                }
                else if (LinqStatementModifierKeywords.StatementModifierKeywords.Any(myToken.Contains))
                {
                    tokenInfo.Color = TokenColor.Keyword;
                    tokenInfo.Type = TokenType.Keyword;
                    tokenInfo.StartIndex = start;
                    items.Add(ToParseItem(line, start, LinqItemType.Keywords, tokenInfo));
                }
                else if (LinqSpecialCharacters.SpecialCharacters.Any(myToken.Contains))
                {
                    tokenInfo.Color = TokenColor.Keyword;
                    tokenInfo.Type = TokenType.String;
                    tokenInfo.StartIndex = start;
                    items.Add(ToParseItem(line, start, LinqItemType.String, tokenInfo));
                }
                else
                {
                    continue;
                }
            }

            return items;
        }


        public static bool IsMatch(Regex regex, string line, out Match match)
        {
            match = regex.Match(line);
            return match.Success;
        }

        public LinqParseItem ToParseItem(string line, int start, LinqItemType type, TokenInfo tokenInfo)
        {
            LinqParseItem item = new(start, line, this, type, tokenInfo);

            return item;
        }

        public LinqParseItem ToParseItem(Match match, int start, string groupName, LinqItemType type, TokenInfo tokenInfo)
        {
            Group group = match.Groups[groupName];
            return ToParseItem(group.Value, start + group.Index, type, tokenInfo);
        }

        public LinqParseItem ToParseItem(Match match, int start, string groupName, TokenInfo tokenInfo)
        {
            Group group = match.Groups[groupName];
            LinqItemType type = group.Value.StartsWith("\"") ? LinqItemType.String : LinqItemType.Literal;
            return ToParseItem(group.Value, start + group.Index, type, tokenInfo);
        }

    }
}
