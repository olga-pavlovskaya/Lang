using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using Logic;

namespace Lang
{
    public partial class _Default : System.Web.UI.Page
    {
        public String t1, t2, t3;
        protected void Page_Load(object sender, EventArgs e)
        {
            var f = File.OpenText(@"D:\lexic.txt");
            t1 = f.ReadToEnd();
            f.Close();
            f = File.OpenText(@"D:\grammar.txt");
            t2 = f.ReadToEnd();
            f.Close();
            f = File.OpenText(@"D:\func.txt");
            t3 = f.ReadToEnd();
            f.Close();
        }

        [WebMethod]
        public static Object SetLexic(String text)
        {
            RegExpTree tree = new RegExpTree();
            tree.Roots = new Dictionary<string, RegExpTreeItem>();
            ComplexMachine machine = new ComplexMachine(tree);
            
            try
            {
                foreach (string line in text.Split('\n').Select(l => l.Trim() + "#"))
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    int i = 0;
                    while (line[i] != '=' && i < line.Length - 1) i++;
                    if (line[i] != '=') continue;
                    String name = line.Substring(0, i);
                    String val = line.Substring(i + 1, line.Length - i - 1);
                    tree.Roots[name] = tree.Load(val);
                }
                machine = new ComplexMachine(tree);
            }
            catch (Exception e)
            {
                return null;
            }
            UserSession us = HttpContext.Current.Session["UserSession"] as UserSession;
            if (us == null) 
            {
                us = new UserSession();
                HttpContext.Current.Session.Add("UserSession", us);
            }
            us.Tree = tree;
            us.Machine = machine;

            string ret = String.Empty;
            foreach (var m in machine.Machines.Keys)
                ret = ret + "<br/>" + machine.Machines[m].ToTable(m);
            return ret;
        }

        [WebMethod]
        public static Object SetGrammar(String text)
        {
            try
            {
                UserSession us = HttpContext.Current.Session["UserSession"] as UserSession;
                if (us == null || us.Tree == null || us.Machine == null)
                {
                    throw new Exception("No lexic found");
                }
                Grammar g = new Grammar();
                g.Reset();
                MagazineMachine m = new MagazineMachine();
                
                foreach (string line in text.Split('\n').Select(l => l.Trim()))
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    m.LoadRule(g, line);
                }
                g.Terminals = new HashSet<String>(g.Terminals.Except(g.NonTerminals));
                g.Terminals.Add("lambda");
                if (g.Rules.Count > 0) g.Start = g.Rules[0];
                Helper h = new Helper();
                h.CountAllQs(g);

                us.Helper = h;
                us.MagMachine = m;
                us.Grammar = g;

                return h.ToTable();
            }
            catch (Exception e)
            {
                return "";
            }
        }

        [WebMethod]
        public static Object DoFunction(String text)
        {
            try
            {
                UserSession us = HttpContext.Current.Session["UserSession"] as UserSession;
                if (us == null || us.Machine == null || us.MagMachine == null || us.Helper == null)
                {
                    throw new Exception("No lexic found");
                }

                List<Object> ret = new List<object>();
                String il = "";
                int i = 0;
                us.Machine.Reset(text.Replace("\n", " "));
                while (us.Machine.HasNext())
                {
                    Token token = us.Machine.GetNextToken();
                    us.Machine.tm.AddToAll(i, token);
                    i += token.Value.Length;
                }
                foreach (Token t in us.Machine.tm.Tokens)
                    ret.Add(new { type = t.Type, value = t.Value });

                ExpressionTree tree = new ExpressionTree();
                us.MagMachine.AnalyzeString(us.Grammar, us.Machine, tree, text.Replace("\n", " "), us.Helper);

                //CodeGenerator.TryGetIlCode();
                //CodeGenerator.Count(0, 2, 1);
                string ilcode = "";
                CodeGenerator cg = new CodeGenerator();
                var func = cg.GetFunctionCount(tree.Root, out ilcode);

                DrawTree(tree);

                return new { dict = ret, il = il };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [WebMethod]
        public static void GetImage(int userid)
        {
            HttpContext.Current.Response.ContentType = "image/png";
            HttpContext.Current.Response.WriteFile(@"d:\2.png");
        }

        public static void DrawTree(ExpressionTree tree)
        {
            var brush = new SolidBrush(Color.Red);
            System.Drawing.Image im = System.Drawing.Image.FromFile(@"d:\1.png");
            var graphics = Graphics.FromImage(im);
            DrawNode(graphics, im.Width / 2 - 100, 30, im.Width / 2 + 300, tree.Root);
            graphics.Save();
            im.Save(@"d:\2.png");
        }

        private static void DrawNode(Graphics g, int x, int y, int width, ExpressionTreeItem node)
        {
            String name = node.Value;

            g.DrawString(name, new Font("Arial", 14),
                new SolidBrush(node.Children.Count > 0 ? Color.RoyalBlue : Color.SandyBrown), x, y - 30);
            if (node.Children.Count > 0)
            {
                int childspace = width / node.Children.Count;
                if (childspace < 20) childspace = 20;
                for (int i = node.Children.Count - 1; i >= 0; i--)
                {
                    DrawNode(g, x - width / 5 + i * childspace, y + 50, childspace, node.Children[i]);
                    g.DrawLine(new Pen(Color.Salmon), x + 5, y + 5, x - width / 5 + i * childspace + 5, y + 25);
                }
            }
        }
    }
}
