using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public enum TokenType
    {
        DELIMITER,
        NUMBER,
        LITERAL,
        MULTOP,
        PLUSOP,
        ASSIGNOP,
        BOOLOP,
        BOOLOP2,
        PUNCTUATOR,
        KEYWORD,
        UNKNOWN,
        Q,
        NONTERM
    }
    public class Token
    {
        public TokenType Type { get; set; }
        public String Value { get; set; }
    }
}
