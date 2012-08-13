using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

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
           /* RegExpTree.Roots = new Dictionary<string, RegExpTreeItem>();
            try
            {
                foreach (string line in text.Split('\n'))
                {
                    int i = 0;
                    while (line[i] != '=' && i < line.Length - 1) i++;
                    if (line[i] != '=') continue;
                    String name = line.Substring(0, i);
                    String val = line.Substring(i + 1, line.Length - i - 1);
                    RegExpTree.Roots[name] = RegExpTree.Load(val);
                }
                ComplexMachine.Create();
            }
            catch (Exception e)
            {
                return null;
            }*/
            string ret = String.Empty;
            //foreach (var m in ComplexMachine.Machines.Values)
            //    ret = ret + "<br/>" + m.ToTable();
            return ret;
        }

        [WebMethod]
        public static Object SetGrammar(String text)
        {
           /* try
            {
                Grammar.Reset();
                foreach (string line in text.Split('\n'))
                {
                    MagazineMachine.LoadRule(line);
                }
                Grammar.Terminals = new HashSet<String>(Grammar.Terminals.Except(Grammar.NonTerminals));
                Grammar.Terminals.Add("lambda");
                if (Grammar.Rules.Count > 0) Grammar.Start = Grammar.Rules[0];
                Helper.CountAllQs();

                DrawTree();
            }
            catch (Exception e)
            {
                return null;
            }*/
            return "";
        }

        [WebMethod]
        public static Object DoFunction(String text)
        {
            /*List<Object> ret = new List<object>();
            String il = "";
            try
            {
                int i = 0;
                ComplexMachine.Reset(text.Replace("\n", " "));
                TokenManager.Reset();
                while (ComplexMachine.HasNext())
                {
                    Token token = ComplexMachine.GetNextToken();
                    TokenManager.AddToAll(i, token);
                    i += token.Value.Length;
                }
                foreach (Token t in TokenManager.Tokens)
                    ret.Add(new { type = t.Type, value = t.Value } );
                
                ExpressionTree.Root = new ExpressionTreeItem(String.Empty);
                MagazineMachine.AnalyzeString(text.Replace("\n", " "));

                CodeGenerator.TryGetIlCode();
                //CodeGenerator.Count(0, 2, 1);
                var f = File.OpenText(@"D:\1.txt");
                il = f.ReadToEnd();
                f.Close();

                DrawTree();
            }
            catch (Exception e)
            {
                return null;
            }*/
            return null;//new { dict = ret, il = il };
        }

        public static void DrawTree()
        {
          /*  var brush = new SolidBrush(Color.Red);
            System.Drawing.Image im = System.Drawing.Image.FromFile(@"d:\1.png");
            var graphics = Graphics.FromImage(im);
            DrawNode(graphics, im.Width / 2 - 100, 30, im.Width / 2 + 300, ExpressionTree.Root);
            graphics.Save();
            im.Save(@"d:\2.png");*/
        }

      /*  private static void DrawNode(Graphics g, int x, int y, int width, ExpressionTreeItem node)
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
        }*/
    }
}
