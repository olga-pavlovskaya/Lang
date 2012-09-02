using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    class Scaner
    {
        private Int32 iterator;
        public String Text { get; set; }
        public Scaner(String text) { Text = text; iterator = 0; }
        public Token GetNextToken()
        {
            StringBuilder sb = new StringBuilder();
            while (iterator < Text.Length && Text[iterator] != ' ')
            {
                sb.Append(Text[iterator]);
                iterator++;
            }
            iterator++;
            return new Token()
            {
                Type = sb.ToString().Length > 3 ? TokenType.LITERAL : TokenType.KEYWORD,
                Value = sb.ToString()
            };
        }
        public Boolean HasNext()
        {
            return iterator < Text.Length;
        }
    }
}
