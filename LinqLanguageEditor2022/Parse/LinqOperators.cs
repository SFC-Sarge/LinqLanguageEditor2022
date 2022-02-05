using System.Linq;

namespace LinqLanguageEditor2022.Parse
{
    public class LinqOperators
    {
        public static readonly string[] Operators = "+ - * / % & ( ) [ ] | ^ ! ~ && || , ++ -- << >> == != < > <= >= = += -= *= /= %= &= |= ^= <<= >>= . [] () ?: => ??".Split().ToArray();
    }
}
