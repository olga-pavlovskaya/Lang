using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class ComplexMachine
    {
        public TokenManager tm;
        private HashSet<String> allowed = new HashSet<string>(){"literal","delimiter","keyword",
            "punctuator","decimal","boolop","mathop","boolop2","plusop","multop","assignop"};
        public Dictionary<String, Machine> Machines { get; set; }
        private Dictionary<String, State> CurStates = new Dictionary<string, State>();
        private Dictionary<String, State> NowFinal = new Dictionary<string, State>();
        public string Text { get; set; }
        private Int32 iterator;
        public void SetText(String text)
        {
            Text = text;
        }
        public void Reset(String text)
        {
            if (tm == null) tm = new TokenManager();
            tm.Reset();
            iterator = 0;
            Text = text;
        }
        private Boolean IsFinal()
        {
            return CurStates.Count == 0;
        }
        private Boolean MoveForward(Char c)
        {
            NowFinal.Clear();
            foreach (var key in CurStates.Where(k => k.Value.IsFinal).Select(k => k.Key))
                NowFinal.Add(key, CurStates[key]);
            List<String> keys = CurStates.Keys.ToList();
            foreach (var key in keys)
            {
                State s = Machines[key].MoveForward(CurStates[key], c);
                if (s == null) CurStates.Remove(key);
                else
                    CurStates[key] = s;
            }
            return CurStates.Count > 0;
        }
        private String Type()
        {
            String res = CurStates.FirstOrDefault(s => s.Value.IsFinal).Key;
            if (res != null) return res;
            return NowFinal.Count() > 0 ? NowFinal.Keys.First() : String.Empty;
        }

        public Token GetNextToken()
        {
            CurStates.Clear();
            NowFinal.Clear();
            if (Machines == null) return null;
            foreach (var key in Machines.Keys) CurStates.Add(key, Machines[key].First);

            StringBuilder sb = new StringBuilder();
            while (!IsFinal() && iterator < Text.Length && CurStates.Count > 0)
            {
                if (MoveForward(Text[iterator]))
                    sb.Append(Text[iterator++]);
                else break;
            }
            Token res = new Token() { Value = sb.ToString() };
            String type = Type();
            if (type == String.Empty)
            {
                res.Type = TokenType.UNKNOWN;
                if (iterator < Text.Length)
                    sb.Append(Text[iterator]);
                res.Value = sb.ToString();
                iterator++;
            }
            else if (type == "delimiter")
                res.Type = TokenType.DELIMITER;
            else if (type == "multop")
                res.Type = TokenType.MULTOP;
            else if (type == "plusop")
                res.Type = TokenType.PLUSOP;
            else if (type == "assignop")
                res.Type = TokenType.ASSIGNOP;
            else if (type == "boolop")
                res.Type = TokenType.BOOLOP;
            else if (type == "boolop2")
                res.Type = TokenType.BOOLOP2;
            else if (type == "punctuator")
                res.Type = TokenType.PUNCTUATOR;
            else if (type == "decimal")
                res.Type = TokenType.NUMBER;
            else if (type == "literal")
                if (NowFinal.Keys.Contains("keyword") ||
                    (CurStates.ContainsKey("keyword") && CurStates["keyword"].IsFinal))
                    res.Type = TokenType.KEYWORD;
                else
                    res.Type = TokenType.LITERAL;
            else if (type == "keyword")
                res.Type = TokenType.KEYWORD;
            if (res.Type != TokenType.UNKNOWN && res.Type != TokenType.DELIMITER)
                tm.AddToken(res);
            return res;
        }
        public Boolean HasNext()
        {
            return iterator < Text.Length;
        }
        public ComplexMachine(RegExpTree tree)
        {
            tree.Positions = new Dictionary<String, List<RegExpTreeItem>>();
            Machines = new Dictionary<string, Machine>();
            int currentState = 0;
            foreach (String name in tree.Roots.Keys)
            {
                if (!allowed.Contains(name)) continue;
                tree.Positions.Add(name, new List<RegExpTreeItem>());
                tree.Roots[name].Calculate(tree.Positions[name]);
                tree.Roots[name].CalculateFollow(tree.Positions[name]);
                Machine machine = new Machine(tree.Positions[name]);
                machine.STATE = currentState;
                machine.TryAdd(tree.Positions[name][tree.Positions[name].Count - 1].FirstPos);
                while (machine.HasUnmarked())
                    machine.GetUnmarked().MoveForward();
                Machines.Add(name, machine);
                currentState = machine.STATE;
            }
        }
    }
}
