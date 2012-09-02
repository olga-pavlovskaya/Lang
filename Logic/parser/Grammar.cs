using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class Rule
    {
        public String Left;
        public List<String> Right;
        public String Action;
        public List<Int32> Params;
        public Rule() { Right = new List<String>(); Params = new List<int>(); }
        public Rule(String l) : this() { Left = l; }
        public bool IsEqual(Rule nr)
        {
            if (!Left.Equals(nr.Left) || (Right.Count != nr.Right.Count)) return false;
            bool eq = true;
            for (int i = 0; i < Right.Count; i++)
                if (!Right[i].Equals(nr.Right[i])) { eq = false; break; }
            return eq;
        }
    }
    public class Situation
    {
        public Rule Rul;
        public Int32 Pos;
        public String Term;
        public Boolean IsEqual(Situation nsit)
        {
            if ((nsit.Pos != Pos) || (!nsit.Term.Equals(Term))
                || (!nsit.Rul.Left.Equals(Rul.Left) || (nsit.Rul.Right.Count != Rul.Right.Count)))
                return false;
            bool ret = true;
            for (int i = 0; i < Rul.Right.Count; i++)
                if (!Rul.Right[i].Equals(nsit.Rul.Right[i])) ret = false;
            return ret;
        }
    }
}
