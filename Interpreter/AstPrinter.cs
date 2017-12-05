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
            throw new NotImplementedException();
        }

        public string VisitAssignment(Assignment basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitBlock(Block basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitClassDecl(ClassDecl basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitComparison(Comparison basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaration(Declaration basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitEquality(Equality basetype)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string VisitMethodDecl(MethodDecl basetype)
        {
            throw new NotImplementedException();
        }

        public string VisitMultiplication(Multiplication basetype)
        {
            throw new NotImplementedException();
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
            return Parenthesize(expression.Operator.Value, expression.right);
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
