using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Helper
    {
        public Dictionary<String, HashSet<String>> Firsts;
        public Dictionary<String, List<Int32>> TableG;
        public Dictionary<String, List<String>> TableF;
        public bool OneRound(Grammar grammar)
        {
            Boolean wasAdded = false;
            foreach (String term in grammar.Terminals)
            {
                if (!Firsts.ContainsKey(term))
                {
                    Firsts.Add(term, new HashSet<string>() { term });
                    wasAdded = true;
                }
            }
            foreach (String nonterm in grammar.NonTerminals)
            {
                if (!Firsts.ContainsKey(nonterm)) Firsts.Add(nonterm, new HashSet<string>());
                if (grammar.Rules.Count(r => r.Left.Equals(nonterm) && (r.Right.Count == 1)
                    && (r.Right[0].Equals("lambda")) && (!Firsts[nonterm].Contains("lambda"))) > 0)
                {
                    Firsts[nonterm].Add("lambda");
                    wasAdded = true;
                }
                foreach (Rule r in grammar.Rules.Where(r => r.Left.Equals(nonterm)))
                {
                    if (r.Right.Count(s => !Firsts.ContainsKey(s)) == 0)
                    {
                        int lastl = -1;
                        for (int i = 0; i < r.Right.Count; i++)
                            if (Firsts[r.Right[i]].Contains("lambda")) lastl = i;
                            else break;
                        if ((lastl == r.Right.Count) && !Firsts[nonterm].Contains("lambda"))
                        {
                            Firsts[nonterm].Add("lambda");
                            wasAdded = true;
                        }
                        else if (lastl < r.Right.Count - 1) lastl++;
                        for (int i = 0; i <= lastl; i++)
                        {
                            foreach (var s in Firsts[r.Right[i]])
                                if (!Firsts[nonterm].Contains(s))
                                {
                                    Firsts[nonterm].Add(s);
                                    wasAdded = true;
                                }
                        }

                    }
                }
                if (Firsts[nonterm].Count == 0) Firsts.Remove(nonterm);
            }
            return wasAdded;
        }
        public void GetFirstForAll(Grammar grammar)
        {
            Firsts = new Dictionary<String, HashSet<String>>();
            Firsts.Add("lambda", new HashSet<string>() { "lambda" });
            while (OneRound(grammar)) ;
        }
        public HashSet<String> GetFirstForChain(List<String> chain)
        {
            HashSet<String> ret = new HashSet<String>();
            int i = 0;
            for (i = 0; i < chain.Count; i++)
            {
                if (Firsts.ContainsKey(chain[i]))
                {
                    foreach (String s in Firsts[chain[i]])
                        if (!s.Equals("lambda") && !ret.Contains(s)) ret.Add(s);
                }
                if (Firsts.ContainsKey(chain[i]) && !Firsts[chain[i]].Contains("lambda")) break;
            }
            if (i == chain.Count && (Firsts[chain[chain.Count - 1]].Contains("lambda")))
                ret.Add("lambda");
            return ret;
        }
        public HashSet<Situation> GetClosure(Grammar grammar, HashSet<Situation> input)
        {
            Stack<Situation> st = new Stack<Situation>(input);
            while (st.Count > 0)
            {
                Situation sit = st.Pop();
                String B = (sit.Pos < sit.Rul.Right.Count) ? sit.Rul.Right[sit.Pos] : "lambda",
                    beta = (sit.Pos + 1 < sit.Rul.Right.Count) ? sit.Rul.Right[sit.Pos + 1] : "lambda",
                    a = sit.Term;
                if (!grammar.NonTerminals.Contains(B)) continue;
                HashSet<Situation> temp = new HashSet<Situation>();
                foreach (var rule in grammar.Rules.Where(r => r.Left.Equals(B)))
                    foreach (var term in GetFirstForChain(new List<String>() { beta, a }))
                    {
                        if (!grammar.Terminals.Contains(term)) continue;
                        Situation nsit = new Situation();
                        nsit.Term = term;
                        nsit.Pos = 0;
                        nsit.Rul = rule;
                        bool cont = false;
                        foreach (var s in input)
                            if (s.IsEqual(nsit)) cont = true;
                        if (!cont)
                        {
                            temp.Add(nsit);
                            st.Push(nsit);
                        }
                    }
                input = new HashSet<Situation>(input.Union(temp));
            }
            return input;
        }
        public HashSet<Situation> GoTo(Grammar grammar, HashSet<Situation> I, String X)
        {
            HashSet<Situation> J = new HashSet<Situation>();
            foreach (var sit in I.Where(s => (s.Pos <= s.Rul.Right.Count) &&
                ((s.Pos < s.Rul.Right.Count) ? s.Rul.Right[s.Pos] : "lambda").Equals(X)))
            {
                Situation ns = new Situation();
                ns.Rul = sit.Rul;
                ns.Term = sit.Term;
                ns.Pos = sit.Pos + 1;
                foreach (var s in GetClosure(grammar, new HashSet<Situation>() { ns }))
                    if (J.Count(j => j.IsEqual(s)) == 0) J.Add(s);
            }
            return J;
        }
        public void CountAllQs(Grammar grammar)
        {
            GetFirstForAll(grammar);
            TableG = new Dictionary<string, List<int>>();
            TableF = new Dictionary<string, List<String>>();
            foreach (var term in grammar.Terminals.Union(grammar.NonTerminals))
            {
                if (term.Equals("lambda")) continue;
                TableG.Add(term, new List<int>());
            }
            foreach (var term in grammar.Terminals)
                TableF.Add(term, new List<String>());
            Situation start = new Situation();
            start.Term = "lambda";
            start.Pos = 0;
            start.Rul = grammar.Start;

            List<HashSet<Situation>> allQ = new List<HashSet<Situation>>();
            allQ.Add(GetClosure(grammar, new HashSet<Situation>() { start }));
            int i = 0;
            while (i < allQ.Count)
            {
                foreach (var list in TableG.Values) list.Add(-1);
                foreach (var list in TableF.Values) list.Add(String.Empty);

                var cursit = allQ[i];
                foreach (var nonterm in grammar.NonTerminals.Union(grammar.Terminals))
                {
                    if (nonterm.Equals("lambda") || nonterm.Equals("start")) continue;
                    var gt = GoTo(grammar, cursit, nonterm);
                    int idx = -1;
                    for (int j = 0; j < allQ.Count; j++)
                        if (allQ[j].IsEqual(gt)) { idx = j; break; }
                    if ((idx == -1) && (gt.Count > 0))
                    {
                        allQ.Add(gt);
                        idx = allQ.Count - 1;
                    }
                    TableG[nonterm][i] = idx;
                }
                i++;
            }
            for (int j = 0; j < allQ.Count; j++)
            {
                foreach (var s in allQ[j])
                {
                    if (s.Pos == s.Rul.Right.Count)
                    {
                        if (s.Rul.Left.Equals("start") && s.Term.Equals("lambda"))
                            TableF[s.Term][j] = "accept";
                        if (!s.Rul.Left.Equals("start"))
                            TableF[s.Term][j] = "reduce " + grammar.Rules.FindIndex(r => r.IsEqual(s.Rul));
                    }
                    else
                    {
                        var a = s.Rul.Right[s.Pos];
                        if (grammar.Terminals.Contains(a))
                        {
                            int jj = TableG[a][j];
                            if (jj != -1) TableF[a][j] = "shift " + jj;
                        }
                    }
                }
            }
        }
    }
    public static class SitSetComp
    {
        public static bool IsEqual(this HashSet<Situation> fset, HashSet<Situation> sset)
        {
            if (fset.Count != sset.Count) return false;
            foreach (var sit in fset)
                if (sset.Count(s => s.IsEqual(sit)) == 0) return false;
            foreach (var sit in sset)
                if (fset.Count(s => s.IsEqual(sit)) == 0) return false;
            return true;
        }
    }
}
