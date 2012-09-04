using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Lang;

namespace Logic
{
    public class MagazineMachine
    {
        public void LoadFromFile(String filename)
        {
            Grammar g = new Logic.Grammar();
            g.Reset();
            StreamReader r = File.OpenText(filename);
            String line = String.Empty;
            while (!r.EndOfStream)
            {
                line = r.ReadLine();
                LoadRule(g,line);
            }
            g.Terminals = new HashSet<String>(g.Terminals.Except(g.NonTerminals));
            g.Terminals.Add("lambda");
            if (g.Rules.Count > 0) g.Start = g.Rules[0];
            Helper h = new Helper();
            h.CountAllQs(g);
            r.Close();
        }
        public void AnalyzeString(Grammar g, ComplexMachine m, ExpressionTree e, String s, Helper h)
        {
            List<Int32> rules = new List<int>();
            List<Token> usedTokens = new List<Token>();
            System.Diagnostics.Debug.WriteLine("Analyzing [" + s + "]");
            Stack<Token> st = new Stack<Token>();
            st.Push(new Token() { Type = TokenType.Q, Value = "0" });

            m.Reset(s);
            Token linetok = null;
            if (m.HasNext()) linetok = m.GetNextToken();
            while (linetok.Type == TokenType.DELIMITER) linetok = m.GetNextToken();

            while (st.Count > 0)
            {
                //Q
                Token stacktok = st.Peek();
                if (stacktok.Type != TokenType.Q) { System.Diagnostics.Debug.WriteLine("Unexpected token in stack"); return; }

                int q = Int32.Parse(stacktok.Value);

                String x = h.TableF.ContainsKey(linetok.Value)
                    ? linetok.Value : linetok.Type.ToString();

                if (!h.TableF.ContainsKey(x))
                {
                    System.Diagnostics.Debug.WriteLine("Unknown token " + x);
                    return;
                }

                String command = h.TableF[x][q];
                if (command.StartsWith("shift"))
                {
                    st.Push(linetok);
                    linetok = m.GetNextToken();
                    while (linetok.Type == TokenType.DELIMITER) linetok = m.GetNextToken();
                    if (linetok.Value == String.Empty) linetok.Value = "lambda";
                }
                else if (command.StartsWith("reduce"))
                {
                    int rulenum = Int32.Parse(command.Substring(6));
                    Rule rule = g.Rules[rulenum];
                    for (int r = rule.Right.Count - 1; r >= 0; r--)
                    {
                        st.Pop();
                        Token right = st.Pop();
                        if (!right.Value.Equals(rule.Right[r]) &&
                            !right.Type.ToString().Equals(rule.Right[r]))
                            System.Diagnostics.Debug.WriteLine("Error while reducing according to rule " + rulenum +
                                ": expected " + rule.Right[r] + ", got " + right);
                    }
                    st.Push(new Token() { Type = TokenType.NONTERM, Value = rule.Left });
                    //Console.WriteLine("Reduced according to rule " + rulenum);
                    System.Diagnostics.Debug.WriteLine("--------- " + rule.Action);
                    rules.Add(rulenum);
                }
                else if (command.StartsWith("accept"))
                {
                    st.Pop();
                    Token start = st.Pop();
                    Token q0 = st.Pop();
                    if ((st.Count == 0) && start.Value.Equals(g.Start.Right[0])
                        && q0.Value.Equals("0") && (q0.Type == TokenType.Q))
                    {
                        System.Diagnostics.Debug.WriteLine("ACCEPTED");
                        e.BuildByRules(g,rules, usedTokens);
                    }
                    else
                        System.Diagnostics.Debug.WriteLine("Unexpected end of file");
                    return;
                }
                else
                {
                    String allowed = String.Empty;
                    foreach (var key in h.TableF.Keys)
                    {
                        if (h.TableF[key][q] != String.Empty)
                        {
                            allowed = key;
                            break;
                        }
                    }
                    throw new Exception("Error: got " + linetok.Value + " while expecting " + allowed);

                }

                //not q
                stacktok = st.Pop();
                Token stacktok2 = st.Pop();
                q = Int32.Parse(stacktok2.Value);
                st.Push(stacktok2);
                st.Push(stacktok);

                x = h.TableG.ContainsKey(stacktok.Value)
                    ? stacktok.Value : stacktok.Type.ToString();

                if (!h.TableG.ContainsKey(x))
                {
                    System.Diagnostics.Debug.WriteLine("Unknown token " + x);
                    return;
                }

                if (stacktok.Type != TokenType.NONTERM)
                {
                    usedTokens.Add(stacktok);
                    System.Diagnostics.Debug.WriteLine("---PUSH " + stacktok.Value);
                }

                int gt = h.TableG[x][q];
                st.Push(new Token() { Type = TokenType.Q, Value = gt.ToString() });
            }
        }

        public void LoadRule(Grammar g, String s)
        {
            if (!s.Contains("->")) return;
            String left = s.Substring(0, s.IndexOf("->"));
            String right = s.Substring(s.IndexOf("->") + 2, s.Length - s.IndexOf("->") - 2);
            String action = String.Empty;
            if (s.Contains('`')) action = s.Substring(s.IndexOf('`') + 1);
            List<Int32> p = new List<int>();
            foreach (var param in action.Split('{'))
                if (param.Contains('}')) p.Add(Int32.Parse(param.Substring(0, param.IndexOf('}'))));
            if (action.Contains('{')) action = action.Substring(0, action.IndexOf('{'));

            var terms = left.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries);
            String lft = String.Empty;
            if (terms.Count() > 0 && terms[0].Contains("]"))
            {
                lft = terms[0].Substring(0, terms[0].IndexOf(']'));
                g.NonTerminals.Add(lft);
            }
            var rls = right.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String rule in rls)
            {
                Rule r = new Rule(lft);
                r.Action = action;
                r.Params = p;
                foreach (String term in rule.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (term.Contains("]"))
                    {
                        var t = term.Substring(0, term.IndexOf(']'));
                        g.Terminals.Add(t);
                        r.Right.Add(t);
                    }
                }
                g.Rules.Add(r);
            }
        }
    }
}
