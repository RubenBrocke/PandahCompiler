using System;
using System.Collections.Generic;

namespace Interpreter
{
    public interface IVisitor<T>
    {
        T VisitProgramStart(ProgramStart basetype);
        T VisitDeclaration(Declaration basetype);
        T VisitClassDecl(ClassDecl basetype);
        T VisitMethodDecl(MethodDecl basetype);
        T VisitVarDecl(VarDecl basetype);
        T VisitBlock(Block basetype);
        T VisitStatement(Statement basetype);
        T VisitAssignment(Assignment basetype);
        T VisitExpression(Expression basetype);
        T VisitLogic(Logic basetype);
        T VisitEquality(Equality basetype);
        T VisitComparison(Comparison basetype);
        T VisitAddition(Addition basetype);
        T VisitMultiplication(Multiplication basetype);
        T VisitUnary(Unary basetype);
        T VisitNumber(Number basetype);
        T VisitString(String basetype);
        T VisitType(Type basetype);
        T VisitIdentifier(Identifier basetype);
        T VisitGrouping(Grouping basetype);
        T VisitLiteral(Literal literal);
        T VisitWhile(While @while);
        T VisitIf(If @if);
    }
    public abstract class BaseType
    {
        public abstract T Accept<T>(IVisitor<T> visitor);
    }

    public class ProgramStart : BaseType
    {
        public List<Declaration> declarations;
        public ProgramStart(List<Declaration> declarations)
        {
            this.declarations = declarations;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitProgramStart(this);
        }
    }

    public class Declaration : BaseType
    {
        public Declaration()
        {
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitDeclaration(this);
        }
    }

    public class ClassDecl : Declaration
    {
        public Identifier className;

        public List<Declaration> body;

        public ClassDecl(Identifier className, List<Declaration> body)
        {
            this.className = className;
            this.body = body;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitClassDecl(this);
        }


    }

    public class MethodDecl : Declaration
    {
        public Identifier methodName;
        public Type returnType;
        public List<Type> arguments;
        public MethodDecl(Identifier methodName, Type returnType, List<Type> arguments)
        {
            this.methodName = methodName;
            this.returnType = returnType;
            this.arguments = arguments;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitMethodDecl(this);
        }
    }

    public class VarDecl : Declaration
    {
        public Identifier varName;
        public Type varType;
        public VarDecl(Identifier varName, Type varType)
        {
            this.varName = varName;
            this.varType = varType;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitVarDecl(this);
        }
    }

    public class Block : BaseType
    {
        public List<Statement> statements;
        public Block(List<Statement> statements)
        {
            this.statements = statements;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBlock(this);
        }
    }

    public class Statement : Declaration
    {
        public Statement()
        {
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitStatement(this);
        }
    }

    public class Assignment : Expression
    {
        public Identifier identifier;
        public Expression expression;
        public Assignment(Identifier identifier, Expression expression)
        {
            this.identifier = identifier;
            this.expression = expression;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitAssignment(this);
        }
    }

    public class Expression : Statement
    {
        public object Value;

        public Expression()
        {

        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitExpression(this);
        }
    }

    public class Logic : Expression
    {
        public Expression left;
        public Token Operator;
        public Expression right;
        public Logic(Expression left, Token Operator, Expression right)
        {
            this.left = left;
            this.Operator = Operator;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLogic(this);
        }
    }

    public class Equality : Expression
    {
        public Expression left;
        public Token Operator;
        public Expression right;
        public Equality(Expression left, Token Operator, Expression right)
        {
            this.left = left;
            this.Operator = Operator;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitEquality(this);
        }
    }

    public class Comparison : Expression
    {
        public Expression left;
        public Token Operator;
        public Expression right;
        public Comparison(Expression left, Token Operator, Expression right)
        {
            this.left = left;
            this.Operator = Operator;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitComparison(this);
        }
    }

    public class Addition : Expression
    {
        public Expression left;
        public Token Operator;
        public Expression right;
        public Addition(Expression left, Token Operator, Expression right)
        {
            this.left = left;
            this.Operator = Operator;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitAddition(this);
        }
    }

    public class Multiplication : Expression
    {
        public Expression left;
        public Token Operator;
        public Expression right;
        public Multiplication(Expression left, Token Operator, Expression right)
        {
            this.left = left;
            this.Operator = Operator;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitMultiplication(this);
        }
    }

    public class Unary : Expression
    {
        public Token Operator;
        public Expression right;
        public Unary(Token Operator, Expression right)
        {
            this.Operator = Operator;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnary(this);
        }
    }

    public class Number : BaseType
    {
        public string value;
        public Number(string value)
        {
            this.value = value;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitNumber(this);
        }
    }

    public class String : BaseType
    {
        public string value;
        public String(string value)
        {
            this.value = value;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitString(this);
        }
    }

    public class Type : BaseType
    {
        public string value;
        public Type(string value)
        {
            this.value = value;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitType(this);
        }
    }

    public class Identifier : BaseType
    {
        public string value;
        public Identifier(string value)
        {
            this.value = value;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitIdentifier(this);
        }
    }

    public class Grouping : Expression
    {
        public Expression expr;
        public Grouping(Expression expr)
        {
            this.expr = expr;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGrouping(this);
        }
    }

    public class Literal : Expression
    {
        public object value;

        public Literal(object value)
        {
            this.value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteral(this);
        }
    }

    public class While : Statement
    {
        public Expression condition;
        public List<Declaration> declarations;

        public While(Expression condition, List<Declaration> declarations)
        {
            this.condition = condition;
            this.declarations = declarations;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitWhile(this);
        }
    }

    public class If : Statement
    {
        public Expression condition;
        public List<Declaration> declarations;

        public If(Expression condition, List<Declaration> declarations)
        {
            this.condition = condition;
            this.declarations = declarations;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitIf(this);
        }
    }
}
