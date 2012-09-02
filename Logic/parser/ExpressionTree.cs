using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class ExpressionTree
    {
        public static ExpressionTreeItem Root = new ExpressionTreeItem("Root");
        public static List<Token> Tokens;

        public void BuildByRules(Grammar g, List<int> rules, List<Token> tokens)
        {
            List<ExpressionTreeItem> Leafs = new List<ExpressionTreeItem>();
            Tokens = tokens;
            Leafs.Add(Root);
            for (int i = rules.Count - 1; i >= 0; i--)
            {
                Rule rule = g.Rules[rules[i]];
                int idx = Leafs.Count - 1;
                while (!Leafs[idx].Value.Equals(rule.Left) && idx > 0)
                    idx--;
                if (idx < 0) { Console.WriteLine("Error while tree building"); return; }

                var leaf = Leafs[idx];
                Leafs.RemoveAt(idx);
                leaf.Value = rule.Left;
                leaf.Rul = rule;
                foreach (var child in rule.Right)
                {
                    ExpressionTreeItem item = new ExpressionTreeItem(child);
                    leaf.Children.Add(item);
                    Leafs.Add(item);
                }
            }
            SetTrueLeafs(Root);
            LeaveOnlyNessesary(null, 0, ref Root);
        }
        private void SetTrueLeafs(ExpressionTreeItem node)
        {
            for (int i = node.Children.Count - 1; i >= 0; i--)
                SetTrueLeafs(node.Children[i]);
            if (node.Children.Count == 0)
            {
                node.Value = Tokens.Last().Value;
                Tokens.RemoveAt(Tokens.Count - 1);
            }
        }
        private void LeaveOnlyNessesary(ExpressionTreeItem parent, Int32 index,
            ref ExpressionTreeItem node)
        {
            if (node.Children.Count == 0) return;

            if (node.Rul != null && node.Rul.Params.Count < 2)
            {
                while (node.Children.Count > 0 && node.Rul != null && node.Rul.Params.Count < 2)
                {
                    if (node.Rul.Params.Count == 1)
                        node = node.Children[node.Rul.Params[0]];
                    else
                        node = node.Children[0];
                }
                if (parent != null)
                    parent.Children[index] = node;
            }

            if (node.Rul != null && node.Rul.Params.Count > 0)
            {
                node.Value = node.Children[node.Rul.Params[0]].Value;
                List<ExpressionTreeItem> tmp = new List<ExpressionTreeItem>();
                foreach (int idx in node.Rul.Params.Where((x, i) => i > 0))
                    tmp.Add(node.Children[idx]);
                node.Children = tmp;
            }

            for (int i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                LeaveOnlyNessesary(node, i, ref child);
            }
        }
    }
    public class ExpressionTreeItem
    {
        public String Value;
        public List<ExpressionTreeItem> Children;
        public Rule Rul;
        public ExpressionTreeItem(String s)
        {
            Value = s;
            Children = new List<ExpressionTreeItem>();
        }
    }
}
