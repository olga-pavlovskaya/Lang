using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public enum Operation
    {
        AND,
        OR,
        MULT,
        NONE,
        LAMBDA
    }
    public class RegExpTreeItem
    {

        public Boolean Nullable { get; set; }
        public HashSet<Int32> FirstPos { get; set; }
        public HashSet<Int32> LastPos { get; set; }
        public HashSet<Int32> FollowPos { get; set; }
        public Operation Op { get; set; }
        public Char Value { get; set; }
        public List<RegExpTreeItem> Children { get; set; }
        public RegExpTreeItem()
        {
            Children = new List<RegExpTreeItem>();
            FirstPos = new HashSet<Int32>();
            LastPos = new HashSet<Int32>();
            FollowPos = new HashSet<Int32>();
        }
        public RegExpTreeItem Clone()
        {
            RegExpTreeItem res = new RegExpTreeItem();
            res.Op = Op;
            res.Value = Value;
            foreach (var child in Children)
                res.Children.Add(child.Clone());
            return res;
        }
        public void Calculate(List<RegExpTreeItem> positions)
        {
            foreach (var child in Children)
                child.Calculate(positions);
            Int32 i = positions.Count;
            positions.Add(this);
            switch (Op)
            {
                case Operation.LAMBDA:
                    Nullable = true;
                    break;
                case Operation.NONE:
                    Nullable = false;
                    FirstPos.Add(i);
                    LastPos.Add(i);
                    break;
                case Operation.AND:
                    Nullable = Children.Count(ch => !ch.Nullable) == 0;
                    int j = -1;
                    do
                    {
                        j++;
                        if (j < Children.Count)
                            FirstPos.UnionWith(Children[j].FirstPos);
                    }
                    while (Children[j].Nullable);
                    j = Children.Count;
                    do
                    {
                        j--;
                        if (j > -1)
                            LastPos.UnionWith(Children[j].LastPos);
                    }
                    while (Children[j].Nullable);
                    break;
                case Operation.OR:
                    Nullable = Children.Count(child => child.Nullable) > 0;
                    Children.ForEach(child =>
                    {
                        FirstPos.UnionWith(child.FirstPos);
                        LastPos.UnionWith(child.LastPos);
                    });
                    break;
                case Operation.MULT:
                    Nullable = true;
                    FirstPos = Children[0].FirstPos;
                    LastPos = Children[0].LastPos;
                    break;
            }
        }
        public void CalculateFollow(List<RegExpTreeItem> positions)
        {
            foreach (var child in Children)
                child.CalculateFollow(positions);
            switch (Op)
            {
                case Operation.NONE:
                    break;
                case Operation.AND:
                    for (int i = 0; i < Children.Count - 1; i++)
                        foreach (Int32 j in Children[i].LastPos)
                            positions[j].FollowPos.UnionWith(Children[i + 1].FirstPos);
                    break;
                case Operation.OR:
                    break;
                case Operation.MULT:
                    foreach (Int32 i in Children[0].LastPos)
                        positions[i].FollowPos.UnionWith(Children[0].FirstPos);
                    break;
            }
        }
    }
}
