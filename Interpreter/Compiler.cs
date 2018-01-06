using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Compiler : IVisitor<string>
    {
        private ProgramStart Root;

        public string VisitAddition(Addition basetype)
        {
            string contents = "";
            contents += basetype.left.Accept(this);
            contents += " ";
            contents += basetype.Operator.Value;
            contents += " ";
            contents += basetype.right.Accept(this);

            return contents;
        }

        public string VisitAssignment(Assignment basetype)
        {
            string contents = "";
            contents += basetype.identifier.Accept(this);
            contents += " = ";
            contents += basetype.expression.Accept(this);
            contents += ";";
            contents += "\n";

            return contents;
        }

        public string VisitBlock(Block basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitClassDecl(ClassDecl basetype)
        {
            string contents = "";
            contents += "class ";
            contents += basetype.className.Accept(this);
            contents += " ";
            contents += "{";
            contents += "\n";
            foreach (Declaration item in basetype.body)
            {
                contents += item.Accept(this);
            }
            contents += "\n";
            contents += "}";
            contents += "\n";

            return contents;
        }

        public string VisitComparison(Comparison basetype)
        {
            string contents = "";
            contents += basetype.left.Accept(this);
            contents += " ";
            contents += basetype.Operator.Value;
            contents += " ";
            contents += basetype.right.Accept(this);

            return contents;
        }

        public string VisitDeclaration(Declaration basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitEquality(Equality basetype)
        {
            string contents = "";
            contents += basetype.left.Accept(this);
            contents += " ";
            contents += basetype.Operator.Value;
            contents += " ";
            contents += basetype.right.Accept(this);

            return contents;
        }

        public string VisitExpression(Expression basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitGrouping(Grouping basetype)
        {
            string contents = "";
            contents += "(";
            contents += basetype.expr.Accept(this);
            contents += ")";

            return contents;
        }

        public string VisitIdentifier(Identifier basetype)
        {
            string contents = "";
            contents += basetype.value;

            return contents;
        }

        public string VisitIf(If @if)
        {
            throw new NotImplementedException();
        }

        public string VisitLiteral(Literal literal)
        {
            string contents = "";
            contents += literal.value;

            return contents;
        }

        public string VisitLogic(Logic basetype)
        {
            string contents = "";
            contents += basetype.left.Accept(this);
            contents += " ";
            contents += basetype.Operator.Value;
            contents += " ";
            contents += basetype.right.Accept(this);

            return contents;
        }

        public string VisitMethodBody(MethodBody methodBody)
        {
            string contents = "";
            contents += "// Method: " + methodBody.identifier;
            contents += "\n";

            return contents;
        }

        public string VisitMethodDecl(MethodDecl basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitMultiplication(Multiplication basetype)
        {
            string contents = "";
            contents += basetype.left.Accept(this);
            contents += " ";
            contents += basetype.Operator.Value;
            contents += " ";
            contents += basetype.right.Accept(this);

            return contents;
        }

        public string VisitNumber(Number basetype)
        {
            string contents = "";
            contents += basetype.value;

            return contents;
        }

        public string VisitProgramStart(ProgramStart basetype)
        {
            //TODO: Add neccesary c++ code
            string contents = "";
            foreach (Declaration item in basetype.declarations)
            {
                contents += item.Accept(this);
            }
            return contents;
        }

        public string VisitStatement(Statement basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitString(String basetype)
        {
            string contents = "";
            contents += '"' + basetype.value + '"';

            return contents;
        }

        public string VisitType(Type basetype)
        {
            string contents = "";
            contents += basetype.value.ToLower();

            return contents;
        }

        public string VisitUnary(Unary basetype)
        {
            string contents = "";
            contents += basetype.Operator.Value;
            contents += "";
            contents += basetype.right.Accept(this);

            return contents;
        }

        public string VisitVarDecl(VarDecl basetype)
        {
            string contents = "";
            contents += basetype.varType.Accept(this);
            contents += " ";
            contents += basetype.varName.Accept(this);
            contents += ";";
            contents += "\n";

            return contents;
        }

        public string VisitWhile(While @while)
        {
            throw new NotImplementedException();
        }

        public void Compile(string filePath, ProgramStart programStart)
        {
            Root = programStart;
            string contents = VisitProgramStart(Root);
            File.WriteAllText(filePath, contents);
            System.Diagnostics.Process.Start("code", filePath);
            Console.WriteLine("File Created: " + filePath);
        }
    }
}
