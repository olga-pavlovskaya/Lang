using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Machine
    {
        public List<RegExpTreeItem> AllPositions { get; set; }

        public int STATE = 0;

        HashSet<State> States = new HashSet<State>();
        public State First;
        public Machine(List<RegExpTreeItem> positions)
        {
            AllPositions = positions;
        }
        public State MoveForward(State state, Char c)
        {
            if (!state.Moves.ContainsKey(c)) return null;
            return state.Moves[c];
        }
        public Boolean Check(String s)
        {
            State state = First;
            for (int i = 0; i < s.Length; i++)
            {
                if (!state.Moves.ContainsKey(s[i])) return false;
                state = state.Moves[s[i]];
            }
            return state.IsFinal;
        }
        public State TryAdd(HashSet<Int32> positions)
        {
            var found = States.FirstOrDefault(st =>
                st.Values.Intersect(positions).Count() == st.Values.Union(positions).Count());
            if (found == null)
            {
                found = new State(this) { Values = positions };
                found.CurPositions = new List<RegExpTreeItem>();
                found.Values.ToList().ForEach(v => found.CurPositions.Add(AllPositions[v]));
                if (States.Count == 0) First = found;
                States.Add(found);
            }
            return found;
        }
        public Boolean HasUnmarked()
        {
            return (States.FirstOrDefault(s => !s.Marked) != null);
        }
        public State GetUnmarked()
        {
            return States.FirstOrDefault(s => !s.Marked);
        }

        public String ToTable(string name)
        {
            var ret = new Dictionary<string, Dictionary<char, string>>();
            List<char> chars = new List<char>();
            foreach (var state in States)
            {
                var dict = new Dictionary<char, string>();
                foreach (var pos in state.Moves)
                {
                    dict.Add(pos.Key, pos.Value.Id);
                    if (!chars.Contains(pos.Key))
                        chars.Add(pos.Key);
                }

                ret.Add(state.Id + (state.IsFinal ? " F" : ""), dict);
            }

            var r = new StringBuilder();
            r.Append("<table>");

            r.Append("<tr>");
            r.Append("<td>" + name + "</td>");
            foreach (var ch in chars) r.Append("<td>" + ch + "</td>");
            r.Append("</tr>");
            foreach (var state in ret.Keys)
            {
                r.Append("<tr>");
                r.Append("<td>" + state + "</td>");
                foreach (var ch in chars)
                    r.Append("<td>" + (ret[state].ContainsKey(ch) ? ret[state][ch] : "-") + "</td>");
                r.Append("</tr>");
            }
            r.Append("</table>");
            return r.ToString();
        }
    }
}
