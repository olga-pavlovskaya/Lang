using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lang;

namespace Logic
{
    public class Grammar
    {
        public HashSet<String> Terminals;
        public HashSet<String> NonTerminals;
        public List<Rule> Rules;
        public Rule Start;
        public void Reset()
        {
            Terminals = new HashSet<String>();
            NonTerminals = new HashSet<String>();
            Rules = new List<Rule>();
        }
    }
}
