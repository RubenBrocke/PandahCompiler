using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTGenerator
{
    class Program
    {
        static string OutputDir;
        static string BaseType;
        static List<string> Types = new List<string>();

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(1);
            }
            OutputDir = args[0];
            DefineAst();
            CreateAst();
            Console.WriteLine("AST Created at: " + OutputDir + "/" + BaseType + ".cs");
            Console.ReadLine();
        }

        static void DefineAst()
        {
            BaseType = "BaseType";
            Types.Add("ProgramStart | List<Declaration> declarations");
            Types.Add("Declaration : BaseType");
            Types.Add("ClassDecl : Declaration | Identifier className");
            Types.Add("MethodDecl : Declaration | Identifier methodName, Type returnType, List<Type> arguments");
            Types.Add("VarDecl : Declaration | Identifier varName, Type varType");
            Types.Add("Block | List<Statement> statements");
            Types.Add("Statement : Declaration");
            Types.Add("Assignment : Expression | Identifier identifier, Expression expression");
            Types.Add("Expression : Statement");
            Types.Add("Logic : Expression | Equality left, Token Operator, Equality right");
            Types.Add("Equality : Expression | Comparison left, Token Operator, Comparison right");
            Types.Add("Comparison : Expression | Addition left, Token Operator, Addition right");
            Types.Add("Addition : Expression | Multiplication left, Token Operator, Multiplication right");
            Types.Add("Multiplication : Expression | Unary left, Token Operator, Unary right");
            Types.Add("Unary : Expression | Token Operator, Unary right");
            Types.Add("Number | string value");
            Types.Add("String | string value");
            Types.Add("Type | string value");
            Types.Add("Identifier | string value");
            Types.Add("Grouping | Expression expr");
        }

        static void CreateAst()
        {
            string path = OutputDir + "/" + BaseType + ".cs";
            string contents = "";
            contents += "using System; \n";
            contents += "using System.Collections.Generic; \n";
            contents += "\n";
            contents += "namespace Interpreter \n";
            contents += "{ \n";
            WriteVisitor(ref contents);
            contents += "   public abstract class " + BaseType + "\n";
            contents += "   { \n";
            contents += "   public abstract T Accept<T>(IVisitor<T> visitor); \n";
            contents += "   } \n";
            foreach (string s in Types)
            {
                contents += "\n";
                string className = "";
                string fields = "";
                if (s.Contains("|"))
                {
                    className = s.Split('|')[0].Trim();
                    fields = s.Split('|')[1].Trim();
                }
                else
                {
                    className = s;
                }
                WriteClass(ref contents, className, fields);
            }
            contents += "} \n";
            File.WriteAllText(path, contents);
        }

        static void WriteVisitor(ref string contents)
        {
            contents += "public interface IVisitor<T>";
            contents += "   { \n";
            foreach (string s in Types)
            {
                string id = s.Split('|')[0].Split(':')[0];
                contents += "T Visit" + id + "( " + id + BaseType.ToLower() + "); \n";
            }
            contents += "   } \n";
        }

        static void WriteClass(ref string contents, string classname, string fields)
        {
            //Class
            if (!classname.Contains(":"))
                contents += "public class " + classname + " : " + BaseType + '\n';
            else
                contents += "public class " + classname + '\n';
            contents += "{ \n";
            //Fields
            foreach (string s in fields.Split(','))
            {
                if (!String.IsNullOrEmpty(s))
                    contents += "public " + s + "; \n";
            }
            //Constructor
            contents += "public " + classname.Split(':')[0] + "(" + fields + ") \n";
            contents += "{ \n";
            //Set Fields
            foreach (string s in fields.Split(','))
            {
                if (!String.IsNullOrEmpty(s))
                {
                    string ID = s.Split(' ').Last();
                    contents += "this." + ID + " = " + ID + "; \n";
                }
            }            
            contents += "} \n";
            //Visitor Pattern
            contents += "public override T Accept<T>(IVisitor<T> visitor)";
            contents += "{ \n";
            contents += "return visitor.Visit" + classname.Split(':')[0] + "(this); \n";
            contents += "} \n";
            contents += "} \n";
        }
    }
}
