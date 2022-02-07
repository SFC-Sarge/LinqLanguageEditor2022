using System.Text;

namespace LinqLanguageEditor2022.Lexical
{
    public class LinqLexicalAnalysis
    {
        public string Parse(string item)
        {
            StringBuilder str = new();
            int ok;
            if (Int32.TryParse(item, out ok))
            {
                str.Append(item);
                return str.ToString();
            }

            if (item.Equals("\r\n"))
            {
                return "\r\n";
            }

            if (CheckKeyword(item) == true)
            {
                str.Append(item);
                return str.ToString();
            }
            if (CheckSpecialCharacters(item) == true)
            {
                str.Append(item);
                return str.ToString();
            }

            if (CheckOperator(item) == true)
            {
                str.Append(item);
                return str.ToString();
            }
            if (CheckDelimiter(item) == true)
            {
                str.Append(item);
                return str.ToString();
            }

            str.Append(item);
            return str.ToString();
        }

        private bool CheckOperator(string str)
        {
            if (Array.IndexOf(LinqOperators.Operators, str) > -1)
                return true;
            return false;
        }
        private bool CheckKeyword(string str)
        {
            if (Array.IndexOf(LinqKeywords.Keywords, str) > -1)
                return true;
            return false;
        }
        private bool CheckDelimiter(string str)
        {
            if (Array.IndexOf(LinqSeparators.Separators, str) > -1)
                return true;
            return false;
        }
        private bool CheckComments(string str)
        {
            if (Array.IndexOf(LinqComments.Comments, str) > -1)
                return true;
            return false;
        }

        private bool CheckSpecialCharacters(string str)
        {
            if (Array.IndexOf(LinqSpecialCharacters.SpecialCharacters, str) > -1)
                return true;
            return false;
        }


        public string GetNextLexicalAtom(ref string item)
        {
            StringBuilder token = new StringBuilder();
            for (int i = 0; i < item.Length; i++)
            {
                if (CheckDelimiter(item[i].ToString()))
                {
                    if (i + 1 < item.Length && CheckDelimiter(item.Substring(i, 2)))
                    {
                        token.Append(item.Substring(i, 2));
                        item = item.Remove(i, 2);
                        return Parse(token.ToString());
                    }
                    else
                    {
                        token.Append(item[i]);
                        item = item.Remove(i, 1);
                        return Parse(token.ToString());
                    }
                }
                else if (CheckOperator(item[i].ToString()))
                {
                    if (i + 1 < item.Length && (CheckOperator(item.Substring(i, 2))))
                        if (i + 2 < item.Length && CheckOperator(item.Substring(i, 3)))
                        {
                            token.Append(item.Substring(i, 3));
                            item = item.Remove(i, 3);
                            return Parse(token.ToString());
                        }
                        else
                        {
                            token.Append(item.Substring(i, 2));
                            item = item.Remove(i, 2);
                            return Parse(token.ToString());
                        }
                    else if (CheckComments(item.Substring(i, 2)))
                    {
                        if (item.Substring(i, 2).Equals("//"))
                        {
                            do
                            {
                                i++;
                            } while (item[i] != '\n');
                            item = item.Remove(0, i + 1);
                            item = item.Trim(' ', '\t', '\r', '\n');
                            i = -1;
                        }
                        else
                        {
                            do
                            {
                                i++;
                            } while (item.Substring(i, 2).Equals("*/") == false);
                            item = item.Remove(0, i + 2);
                            item = item.Trim(' ', '\t', '\r', '\n');
                            i = -1;
                        }
                    }
                    else
                    {
                        int ok;
                        if (item[i] == '-' && Int32.TryParse(item[i + 1].ToString(), out ok))
                            continue;
                        token.Append(item[i]);
                        item = item.Remove(i, 1);
                        return Parse(token.ToString());
                    }
                }
                else
                    if (item[i] == '\'')
                {
                    int j = i + 1;
                    if (item[j] == '\\')
                        j += 2;
                    else
                        j++;
                    token.Append("(, ").Append(item.Substring(i, j - i + 1)).Append(") ");
                    item = item.Remove(i, j - i + 1);
                    return token.ToString();
                }
                else
                    if (item[i] == '"')
                {
                    int j = i + 1;
                    while (item[j] != '"')
                        j++;
                    token.Append("(, ").Append(item.Substring(i, j - i + 1)).Append(") ");
                    item = item.Remove(i, j - i + 1);
                    return token.ToString();
                }
                else
                    if (item[i + 1].ToString().Equals(" ") || CheckDelimiter(item[i + 1].ToString()) == true || CheckOperator(item[i + 1].ToString()) == true)
                {
                    if (Parse(item.Substring(0, i + 1)).Contains("") && item[i + 1] == '.')
                    {
                        int j = i + 2;
                        while (item[j].ToString().Equals(" ") == false && CheckDelimiter(item[j].ToString()) == false && CheckOperator(item[j].ToString()) == false)
                            j++;
                        int ok;
                        if (Int32.TryParse(item.Substring(i + 2, j - i - 2), out ok))
                        {
                            token.Append("(, ").Append(item.Substring(0, j)).Append(") ");
                            item = item.Remove(0, j);
                            return token.ToString();
                        }
                    }
                    token.Append(item.Substring(0, i + 1));
                    item = item.Remove(0, i + 1);
                    return Parse(token.ToString());
                }
            }
            return null;
        }
    }
}
