using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Interpreter : IVisitor<object>
    {
        public ProgramStart Root { get; private set; }
        public Class CurrentClass { get; private set; }
        public Method CurrentMethod { get; private set; }

        public Interpreter(ProgramStart root)
        {
            Root = root;
            CurrentClass = null;
            CurrentMethod = null;
        }

        public object VisitAddition(Addition basetype)
        {
            object left = basetype.left.Accept(this);
            object right = basetype.right.Accept(this);
            return (int)left + (int)right;
        }

        public object VisitAssignment(Assignment basetype)
        {
            string variableName = (string)basetype.identifier.value;
            object newValue = basetype.expression.Accept(this);

            if (CurrentClass == null)
            {            
                if (CurrentMethod == null)
                {
                    // Global scope
                    Environment.FindVariable(variableName).Value = newValue;
                }
                else
                {
                    // Method in global scope
                    CurrentMethod.FindVariable(variableName).Value = newValue;
                }
            }
            else
            {
                // In a class
                if (CurrentMethod != null)
                {
                    // In a method within a class
                    CurrentMethod.FindVariable(variableName).Value = newValue;
                }
                else
                {
                    // In a class and not in a method
                    CurrentClass.FindVariable(variableName).Value = newValue;
                }
            }

            return basetype.Value;
        }

        public object VisitBlock(Block basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitClassDecl(ClassDecl basetype)
        {
            string className = (string)basetype.className.Accept(this);
            Class c = new Class(className);

            if (CurrentClass == null)
            {
                // Global scope
                if (!Environment.Classes.Any(n => n.ClassName == c.ClassName))
                {
                    new CompilerException("Class: " + c.ClassName + " Could not be found");
                }
            }
            else
            {
                // In a class within a class
                if (!CurrentClass.Classes.Any(n => n.ClassName == c.ClassName))
                {
                    new CompilerException("Variable: " + c.ClassName + " Could not be found");
                }
            }

            CurrentClass = c;

            foreach (Declaration item in basetype.body)
            {
                item.Accept(this);
            }

            CurrentClass = c.ParentClass;

            return c;
        }

        public object VisitComparison(Comparison basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitDeclaration(Declaration basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitEquality(Equality basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitExpression(Expression basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitGrouping(Grouping basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitIdentifier(Identifier basetype)
        {
            object identifierValue;
            string identifierName = basetype.value;

            if (CurrentClass == null)
            {
                if (CurrentMethod == null)
                {
                    // Global scope
                    identifierValue = Environment.FindIdentifier(identifierName);
                }
                else
                {
                    // Method in global scope
                    identifierValue = CurrentMethod.FindIdentifier(identifierName);
                }
            }
            else
            {
                // In a class
                if (CurrentMethod != null)
                {
                    // In a method within a class
                    identifierValue = CurrentMethod.FindIdentifier(identifierName);
                }
                else
                {
                    // In a class and not in a method
                    identifierValue = CurrentClass.FindIdentifier(identifierName);
                }
            }

            if (identifierValue is Variable v)
                return v.Value;
            if (identifierValue is Method m)
                return m.MethodName;
            if (identifierValue is Class c)
                return c.ClassName;
            else
                new CompilerException("Identifier: " + identifierName + " is an unknown type");
            return null;
        }

        public object VisitIf(If @if)
        {
            throw new NotImplementedException();
        }

        public object VisitLiteral(Literal literal)
        {
            return literal.value;
        }

        public object VisitLogic(Logic basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitMethodBody(MethodBody methodBody)
        {
            throw new NotImplementedException();
        }

        public object VisitMethodDecl(MethodDecl basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitMultiplication(Multiplication basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitNumber(Number basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitProgramStart(ProgramStart basetype)
        {
            foreach (Declaration item in basetype.declarations)
            {
                item.Accept(this);
            }
            return basetype;
        }

        public object VisitStatement(Statement basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitString(String basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitType(Type basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitUnary(Unary basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitVarDecl(VarDecl basetype)
        {
            string variableName = (string)basetype.varName.value;
            Variable v = new Variable(variableName, null);

            if (CurrentClass == null)
            {
                if (CurrentMethod == null)
                {
                    // Global scope
                    if (!Environment.Variables.Any(n => n.VariableName == v.VariableName))
                    {
                        new CompilerException("Variable: " + v.VariableName + " Could not be found");
                    }
                }
                else
                {
                    // Method in global scope
                    if (!CurrentMethod.Variables.Any(n => n.VariableName == v.VariableName))
                    {
                        new CompilerException("Variable: " + v.VariableName + " Could not be found");
                    }
                }
            }
            else
            {
                // In a class
                if (CurrentMethod == null)
                {
                    // In a method within a class
                    if (!CurrentClass.Variables.Any(n => n.VariableName == v.VariableName))
                    {
                        new CompilerException("Variable: " + v.VariableName + " Could not be found");
                    }
                }
                else
                {
                    // In a class and not in a method
                    if (!CurrentMethod.Variables.Any(n => n.VariableName == v.VariableName))
                    {
                        new CompilerException("Variable: " + v.VariableName + " Could not be found");
                    }
                }
            }

            return v;            
        }

        public object VisitWhile(While @while)
        {
            throw new NotImplementedException();
        }

        public object Interprete()
        {
            return VisitProgramStart(Root);
        }
    }
}
