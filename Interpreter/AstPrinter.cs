using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    class AstPrinter : IVisitor<string>
    {
        public string VisitAddition(Addition basetype)
        {
            return Parenthesize("Addition", basetype.left, basetype.right);
        }

        public string VisitAssignment(Assignment basetype)
        {
            return Parenthesize("Assignment", basetype.identifier, basetype.expression);
        }

        public string VisitBlock(Block basetype)
        {
            return Parenthesize("Block", basetype.statements.ToArray());
        }

        public string VisitClassDecl(ClassDecl basetype)
        {
            return Parenthesize("Class", basetype.className, basetype.body.ToArray());
        }

        public string VisitComparison(Comparison basetype)
        {
            return Parenthesize("Comparison", basetype.left, basetype.right);
        }

        public string VisitDeclaration(Declaration basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitEquality(Equality basetype)
        {
            return Parenthesize("Equality", basetype.left, basetype.right);
        }

        public string VisitExpression(Expression basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitGrouping(Grouping expression)
        {
            return Parenthesize("group", expression.expr);
        }

        public string VisitIdentifier(Identifier basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitIf(If @if)
        {
            throw new NotImplementedException();
        }

        public string VisitLiteral(Literal literal)
        {
            throw new NotImplementedException();
        }

        public string VisitLogic(Logic basetype)
        {
            return Parenthesize("Logic", basetype.left, basetype.right);
        }

        public string VisitMethodDecl(MethodDecl basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitMultiplication(Multiplication basetype)
        {
            return Parenthesize("Multiplication", basetype.left, basetype.right);
        }

        public string VisitNumber(Number basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitProgramStart(ProgramStart basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitStatement(Statement basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitString(String basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitType(Type basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitUnary(Unary expression)
        {
            return Parenthesize("Unart", expression.right);
        }

        public string VisitVarDecl(VarDecl basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitWhile(While @while)
        {
            throw new NotImplementedException();
        }

        private string Parenthesize(string name, params BaseType[] expressions)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(").Append(name);
            foreach (BaseType expr in expressions)
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(expr.Accept(this));
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }
}
