using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logic
{
    public class RegExpTree
    {
        public Dictionary<String, RegExpTreeItem> Roots { get; set; }
        public Dictionary<String, List<RegExpTreeItem>> Positions { get; set; }
        public Char[] Special = new Char[] { '(', ')', '[', ']', '*', '|' };

        public void LoadFromFile(String filename)
        {
            Roots = new Dictionary<string, RegExpTreeItem>();
            StreamReader r = File.OpenText(filename);
            String line = String.Empty;
            while (!r.EndOfStream)
            {
                line = r.ReadLine();
                int i = 0;
                while (line[i] != '=' && i < line.Length - 1) i++;
                if (line[i] != '=') continue;
                String name = line.Substring(0, i);
                String val = line.Substring(i + 1, line.Length - i - 1);
                Roots[name] = Load(val);
            }
            r.Close();
        }
        public RegExpTreeItem Load(String s)
        {
            if (s.Length == 0) return null;
            Char last = s[s.Length - 1];
            if (last == ']')
            {
                int i = s.Length - 2;
                while (s[i] != '[' && i > 0) i--;
                if (s[i] != '[') throw new Exception("No corresponding bracket!");
                RegExpTreeItem res = new RegExpTreeItem();
                String right = s.Substring(i + 1, s.Length - i - 2);

                RegExpTreeItem rightitem = Roots.ContainsKey(right)
                    ? Roots[right].Clone().Children[0]
                    : new RegExpTreeItem() { Op = Operation.NONE, Value = right[0] };

                if (i == 0)
                    return rightitem;
                String left = String.Empty;
                if (s[i - 1] == '|')
                {
                    res.Op = Operation.OR;
                    left = s.Substring(0, i - 1);
                }
                else
                {
                    left = s.Substring(0, i);
                    res.Op = Operation.AND;
                }
                res.Children.Add(Load(left));
                res.Children.Add(rightitem);
                return res;
            }

            if (last == ')')
            {
                int i = s.Length - 2;
                int count = 1;
                while (i > 0 && count != 0)
                {
                    if (s[i] == ')') count++;
                    if (s[i] == '(') count--;
                    if (count == 0) break;
                    i--;
                }
                if (i == 0 && s[i] == '(') count--;
                if (s[i] != '(' || count != 0)
                    throw new Exception("No corresponding bracket!");
                RegExpTreeItem res = new RegExpTreeItem();
                String right = s.Substring(i + 1, s.Length - i - 2);
                if (right == String.Empty) return new RegExpTreeItem() { Op = Operation.LAMBDA };
                if (i == 0) return Load(right);
                String left = String.Empty;
                if (s[i - 1] == '|')
                {
                    res.Op = Operation.OR;
                    left = s.Substring(0, i - 1);
                }
                else
                {
                    left = s.Substring(0, i);
                    res.Op = Operation.AND;
                }
                res.Children.Add(Load(left));
                res.Children.Add(Load(right));
                return res;
            }

            if (last == '*' && s.Length > 1)
            {
                String expr = s.Substring(0, s.Length - 1);
                RegExpTreeItem res = new RegExpTreeItem() { Op = Operation.MULT };
                res.Children.Add(Load(expr));
                return res;
            }

            if (Special.Contains(last))
                throw new Exception("Single special item " + last + "!");

            if (s.Length == 1)
                return new RegExpTreeItem() { Op = Operation.NONE, Value = last };

            if (s[s.Length - 2] == '|')
            {
                String expr = s.Substring(0, s.Length - 2);
                if (expr.Length == 0) throw new Exception("Nothing to the left of |!");
                RegExpTreeItem res = new RegExpTreeItem() { Op = Operation.OR };
                res.Children.Add(Load(expr));
                res.Children.Add(new RegExpTreeItem() { Op = Operation.NONE, Value = last });
                return res;
            }

            RegExpTreeItem rs = new RegExpTreeItem() { Op = Operation.AND };
            rs.Children.Add(Load(s.Substring(0, s.Length - 1)));
            rs.Children.Add(new RegExpTreeItem() { Op = Operation.NONE, Value = last });
            return rs;
        }
        private void Copy(RegExpTreeItem from, RegExpTreeItem to)
        {
            to.Value = from.Value;
            to.Op = from.Op;
            foreach (var child in from.Children)
            {
                var tochild = new RegExpTreeItem();
                to.Children.Add(tochild);
                Copy(child, tochild);
            }
        }
    }
}
