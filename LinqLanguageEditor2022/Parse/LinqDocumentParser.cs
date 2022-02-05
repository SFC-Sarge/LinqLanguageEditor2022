
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LinqLanguageEditor2022.Parse
{
    public partial class LinqDocument
    {

        public void Parse()
        {
            int start = 0;

            List<LinqParseItem> items = new();

            foreach (string line in _lines)
            {
                IEnumerable<LinqParseItem> current = ParseLine(start, line, items);
                items.AddRange(current);
                start += line.Length;
            }

            Items = items;

        }

        private IEnumerable<LinqParseItem> ParseLine(int start, string line, List<LinqParseItem> tokens)
        {
            string trimmedLine = line.Trim();
            List<LinqParseItem> items = new();
            string[] myTokens = trimmedLine.Split(new[] { ' ' });

            foreach (string myToken in myTokens)
            {
                if (LinqNamespaceKeywords.NamespaceKeywords.Any(myToken.Contains))
                {
                    items.Add(ToParseItem(line, start, LinqTokenTypes.Keyword));
                }
                else if (LinqOperatorKeywords.OperatorKeywords.Any(myToken.Contains))
                {
                    items.Add(ToParseItem(line, start, LinqTokenTypes.Operator));
                }
                else if (LinqModifierKeywords.ModifierKeywords.Any(myToken.Contains))
                {
                    items.Add(ToParseItem(line, start, LinqTokenTypes.Keyword));
                }
                else if (LinqOperators.Operators.Any(myToken.Contains))
                {
                    items.Add(ToParseItem(line, start, LinqTokenTypes.Operator));
                }
                else if (LinqSeparators.Separators.Any(myToken.Contains))
                {
                    items.Add(ToParseItem(line, start, LinqTokenTypes.Identifier));
                }
                else if (LinqStatementModifierKeywords.StatementModifierKeywords.Any(myToken.Contains))
                {
                    items.Add(ToParseItem(line, start, LinqTokenTypes.Keyword));
                }
                else if (LinqSpecialCharacters.SpecialCharacters.Any(myToken.Contains))
                {
                    items.Add(ToParseItem(line, start, LinqTokenTypes.String));
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

        public LinqParseItem ToParseItem(string line, int start, LinqTokenTypes type)
        {
            LinqParseItem item = new(start, line, this, type);

            return item;
        }

        public LinqParseItem ToParseItem(Match match, int start, string groupName, LinqTokenTypes type)
        {
            Group group = match.Groups[groupName];
            return ToParseItem(group.Value, start + group.Index, type);
        }

        public LinqParseItem ToParseItem(Match match, int start, string groupName)
        {
            Group group = match.Groups[groupName];
            LinqTokenTypes type = group.Value.StartsWith("\"") ? LinqTokenTypes.String : LinqTokenTypes.Literal;
            return ToParseItem(group.Value, start + group.Index, type);
        }

    }
}
