using System.Linq;

namespace LinqLanguageEditor2022.Lexical
{
    public class LinqOperators
    {
        public static readonly string[] Operators = "+ - * / % & ( ) [ ] | ^ ! ~ && || , ++ -- << >> == != < > <= >= = += -= *= /= %= &= |= ^= <<= >>= . [] () ?: => ??".Split().ToArray();
    }
}
