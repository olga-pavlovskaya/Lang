using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class State
    {
        public static int NEXTID;
        Machine machine { get; set; }
        public List<RegExpTreeItem> CurPositions { get; set; }
        public HashSet<Int32> Values { get; set; }
        public Dictionary<Char, State> Moves;
        public Boolean Marked { get; set; }
        public string Id { get; set; }
        public State(Machine mach)
        {
            Id = "q" + NEXTID.ToString();
            NEXTID++;
            machine = mach;
            Moves = new Dictionary<Char, State>();
            Values = new HashSet<Int32>();
            CurPositions = new List<RegExpTreeItem>();
        }
        public Boolean IsFinal
        {
            get
            {
                return CurPositions.Where(p => p.Value == '#').Count() > 0;
            }
        }
        public void MoveForward()
        {
            foreach (var group in CurPositions.GroupBy(p => p.Value))
            {
                if (group.Key == '\0') continue;
                HashSet<Int32> dest = new HashSet<Int32>();
                group.ToList().ForEach(g => dest.UnionWith(g.FollowPos));
                if (dest.Count > 0)
                    Moves[group.Key] = machine.TryAdd(dest);
            }
            Marked = true;
        }
    }
}
