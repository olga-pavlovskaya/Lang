using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Lang;

namespace Logic
{
    public class CodeGenerator : Lang.IGen
    {
        private delegate TReturn OneParameter<TReturn, TParam>(TParam p0);
        private static Dictionary<string, LocalBuilder> SymbolTable;
        private static String ilpath = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\x64\ildasm.exe";

        public Func<double, double> GetFunctionCount(ExpressionTreeItem Root, out string ilcode)
        {
            SymbolTable = new Dictionary<string, LocalBuilder>();
            DynamicMethod count = new DynamicMethod(
                            "Count",
                            typeof(double),
                            new Type[] { typeof(double) },
                            typeof(CodeGenerator).Module);

            ILGenerator il = count.GetILGenerator();
            SymbolTable.Add("x", il.DeclareLocal(typeof(double)));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Stloc, SymbolTable["x"]);

            EmitNode(il, Root);

            var ret = count.CreateDelegate(typeof(Func<double, double>))
                as Func<double, double>;

            ilcode = TryGetIlCode(Root);
            return ret;
        }

        public static string TryGetIlCode(ExpressionTreeItem Root)
        {
            SymbolTable = new Dictionary<string, LocalBuilder>();

            AssemblyName cg = new AssemblyName("cg");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(cg,
                AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder mb = ab.DefineDynamicModule("mg", "mod.dll");
            TypeBuilder tb = mb.DefineType("cl");
            MethodBuilder mmb = tb.DefineMethod("count",
                MethodAttributes.Static | MethodAttributes.Public);

            ILGenerator il = mmb.GetILGenerator();
            SymbolTable.Add("x", il.DeclareLocal(typeof(double)));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Stloc, SymbolTable["x"]);
            EmitNode(il, Root);
            tb.CreateType();
            ab.Save("res.dll");

            System.Diagnostics.Process ilproc = new System.Diagnostics.Process();
            ilproc.StartInfo.FileName = ilpath;
            ilproc.StartInfo.Arguments = @" /OUT=D:\1.txt /TEXT D:\study\kurs\kursach\kursach\bin\Debug\mod.dll";
            ilproc.Start();
            var f = System.IO.File.OpenText(@"D:\1.txt");
            string ilcode = f.ReadToEnd();
            f.Close();
            return ilcode;
        }

        public static void EmitNode(ILGenerator il, ExpressionTreeItem node)
        {
            if (node.Rul == null || (!node.Rul.Action.Equals("IF")
                && !node.Rul.Action.Equals("FOR")))
                for (int i = 0; i < node.Children.Count; i++)
                    EmitNode(il, node.Children[i]);

            if (node.Rul == null)
            {
                Double d;
                if (Double.TryParse(node.Value.Replace('.', ','), out d))
                    il.Emit(OpCodes.Ldc_R8, d);
                else
                {
                    if (SymbolTable.ContainsKey(node.Value))
                        il.Emit(OpCodes.Ldloc, SymbolTable[node.Value]);
                }
                return;
            }

            if (node.Rul.Action.Equals("ADD"))
            {
                if (node.Value.Equals("+"))
                    il.Emit(OpCodes.Add);
                if (node.Value.Equals("-"))
                    il.Emit(OpCodes.Sub);
            }
            else if (node.Rul.Action.Equals("MULT"))
            {
                if (node.Value.Equals("*"))
                    il.Emit(OpCodes.Mul);
                if (node.Value.Equals("/"))
                    il.Emit(OpCodes.Div);
            }
            else if (node.Rul.Action.Equals("UNARY"))
            {
                Double d = -1;
                il.Emit(OpCodes.Ldc_R8, d);
                il.Emit(OpCodes.Mul);
            }
            else if (node.Rul.Action.Equals("ASSIGN"))
            {
                String varname = node.Value;
                if (!SymbolTable.ContainsKey(varname))
                    SymbolTable.Add(varname, il.DeclareLocal(typeof(double)));
                il.Emit(OpCodes.Stloc, SymbolTable[varname]);
            }
            else if (node.Rul.Action.Equals("CALL"))
            {
                List<Type> types = new List<Type>();
                var tmp = node;
                if (tmp.Children.Count > 0)
                {
                    while (tmp.Children[0].Value.Equals(","))
                    {
                        types.Add(typeof(double));
                        tmp = tmp.Children[0];
                    }
                    types.Add(typeof(double));
                }
                String asmname = String.Empty,
                    classname = String.Empty,
                    methodname = node.Value;
                if (methodname.Contains('.'))
                {
                    classname = methodname.Substring(0, methodname.LastIndexOf('.'));
                    methodname = methodname.Substring(methodname.LastIndexOf('.') + 1);
                    if (classname.Contains('.'))
                    {
                        asmname = classname.Substring(0, classname.LastIndexOf('.'));
                    }
                }
                Assembly asm = Assembly.LoadWithPartialName(asmname);
                Type tp = asm.GetType(classname);
                if (tp == null)
                {
                    asm = Assembly.Load("mscorlib");
                    tp = asm.GetType(classname);
                }
                MethodInfo mi = tp.GetMethod(methodname, types.ToArray());
                il.Emit(OpCodes.Call, mi);
            }
            else if (node.Rul.Action.Equals("NT"))
            {
                il.Emit(OpCodes.Ldc_I4, 0);
                il.Emit(OpCodes.Ceq);
            }
            else if (node.Rul.Action.Equals("UNT"))
            {
                if (node.Value.Equals("&"))
                    il.Emit(OpCodes.And);
                if (node.Value.Equals("^"))
                    il.Emit(OpCodes.Or);
            }
            else if (node.Rul.Action.Equals("COMPARE"))
            {
                if (node.Value.Equals("<"))
                    il.Emit(OpCodes.Clt);
                if (node.Value.Equals(">"))
                    il.Emit(OpCodes.Cgt);
                if (node.Value.Equals("<="))
                {
                    il.Emit(OpCodes.Cgt);
                    il.Emit(OpCodes.Ldc_I4, 0);
                    il.Emit(OpCodes.Ceq);
                }
                if (node.Value.Equals(">="))
                {
                    il.Emit(OpCodes.Clt);
                    il.Emit(OpCodes.Ldc_I4, 0);
                    il.Emit(OpCodes.Ceq);
                }
                if (node.Value.Equals("="))
                    il.Emit(OpCodes.Ceq);
                if (node.Value.Equals("<>"))
                {
                    il.Emit(OpCodes.Ceq);
                    il.Emit(OpCodes.Not);
                }
            }
            else if (node.Rul.Action.Equals("IF"))
            {
                il.Emit(OpCodes.Ldc_I4, 0);

                Label test = il.DefineLabel();
                il.Emit(OpCodes.Br, test);

                Label body = il.DefineLabel();
                il.MarkLabel(body);
                for (int i = 1; i < node.Children.Count; i++)
                    EmitNode(il, node.Children[i]);
                Label end = il.DefineLabel();
                il.Emit(OpCodes.Br, end);

                il.MarkLabel(test);
                EmitNode(il, node.Children[0]);
                il.Emit(OpCodes.Ldc_I4, 1);
                il.Emit(OpCodes.Beq, body);
                il.MarkLabel(end);
            }
            else if (node.Rul.Action.Equals("FOR"))
            {
                String i = node.Children[0].Value;
                if (!SymbolTable.ContainsKey(i))
                    SymbolTable.Add(i, il.DeclareLocal(typeof(double)));

                EmitNode(il, node.Children[1]);
                il.Emit(OpCodes.Stloc, SymbolTable[i]);

                Label test = il.DefineLabel();
                il.Emit(OpCodes.Br, test);

                Label body = il.DefineLabel();
                il.MarkLabel(body);
                EmitNode(il, node.Children[3]);

                il.Emit(OpCodes.Ldloc, SymbolTable[i]);
                EmitNode(il, node.Children[4]);
                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Stloc, SymbolTable[i]);

                il.MarkLabel(test);
                il.Emit(OpCodes.Ldloc, SymbolTable[i]);
                EmitNode(il, node.Children[2]);
                il.Emit(OpCodes.Ble, body);
            }
            else if (node.Rul.Action.Equals("RT"))
                il.Emit(OpCodes.Ret);
        }
    }
}
