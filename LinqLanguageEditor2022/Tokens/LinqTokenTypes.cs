
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqLanguageEditor2022.Tokens
{
    public enum LinqTokenTypes
    {
        comment,
        literal,
        @operator,
        @string,
        unknown,
        identifier,
        keyword,
        number,
        whitespace,
        punctuation,
        separator,

    }
}
