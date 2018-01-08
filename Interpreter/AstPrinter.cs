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
            return Parenthesize("Class: " + basetype.className.value, basetype.body.ToArray());
        }

        public string VisitComparison(Comparison basetype)
        {
            return Parenthesize("Comparison", basetype.left, basetype.right);
        }

        public string VisitDeclaration(Declaration basetype)
        {
            return Parenthesize("Declaration");
        }

        public string VisitEquality(Equality basetype)
        {
            return Parenthesize("Equality", basetype.left, basetype.right);
        }

        public string VisitExpression(Expression basetype)
        {
            return Parenthesize("Expression");
        }

        public string VisitGrouping(Grouping expression)
        {
            return Parenthesize("group", expression.expr);
        }

        public string VisitIdentifier(Identifier basetype)
        {
            return Parenthesize("Identifier " + basetype.value);
        }

        public string VisitIf(If @if)
        {
            return Parenthesize("IF " + VisitExpression(@if.condition), @if.declarations.ToArray());
        }

        public string VisitLiteral(Literal literal)
        {
            return Parenthesize("Literal " + literal.value);
        }

        public string VisitLogic(Logic basetype)
        {
            return Parenthesize("Logic", basetype.left, basetype.right);
        }

        public string VisitMethodBody(MethodBody methodBody)
        {
            return Parenthesize("MethodBody " + VisitIdentifier(methodBody.identifier), methodBody.body.ToArray());
        }

        public string VisitMethodCall(MethodCall methodCall)
        {
            return Parenthesize("Method Call " + methodCall.identifier);
        }

        public string VisitMethodDecl(MethodDecl basetype)
        {
            return Parenthesize("MethodDecl " + VisitIdentifier(basetype.methodName) + " " + VisitType(basetype.returnType), basetype.arguments.ToArray());
        }

        public string VisitMultiplication(Multiplication basetype)
        {
            return Parenthesize("Multiplication", basetype.left, basetype.right);
        }

        public string VisitNumber(Number basetype)
        {
            return Parenthesize("Number " + basetype.value);
        }

        public string VisitProgramStart(ProgramStart basetype)
        {
            return Parenthesize("Program", basetype.declarations.ToArray());
        }

        public string VisitStatement(Statement basetype)
        {
            return Parenthesize("Statement ");
        }

        public string VisitString(String basetype)
        {
            return Parenthesize("String " + basetype.value);
        }

        public string VisitType(Type basetype)
        {
            return Parenthesize("Type " + basetype.value);
        }

        public string VisitUnary(Unary expression)
        {
            return Parenthesize("Unary", expression.right);
        }

        public string VisitVarDecl(VarDecl basetype)
        {
            return Parenthesize("VarDecl " + basetype.varName, basetype.varType);
        }

        public string VisitWhile(While @while)
        {
            return Parenthesize("While " + VisitExpression(@while.condition), @while.declarations.ToArray());
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
