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
        /// <summary>
        /// The root of the program
        /// </summary>
        public ProgramStart Root { get; private set; }

        /// <summary>
        /// The class we're currently interpreting
        /// </summary>
        public Class CurrentClass { get; private set; }

        /// <summary>
        /// The method we're currently interpreting
        /// </summary>
        public Method CurrentMethod { get; private set; }

        /// <summary>
        /// The constructor of the interpreter
        /// </summary>
        /// <param name="root">The place to start interpreting from</param>
        public Interpreter(ProgramStart root)
        {
            Root = root;
            CurrentClass = null;
            CurrentMethod = null;
        }
        
        /// <summary>
        /// Interpret an addition
        /// </summary>
        /// <param name="basetype">The addition</param>
        /// <returns>The value of the addition</returns>
        public object VisitAddition(Addition basetype)
        {
            // Get the value of the left part of the addition
            object left = basetype.left.Accept(this);
            // Get the value of the right part of the addition
            object right = basetype.right.Accept(this);
            // If the operator is the PLUS operator
            if (basetype.Operator.TokenType == TokenType.PLUS)
                // Return left PLUS right
                return (int)left + (int)right;
            // If the operator is the MINUS operator
            if (basetype.Operator.TokenType == TokenType.MINUS)
                // Return left MINUS right
                return (int)left - (int)right;
            // If the functions hasnt ended by now something happened and we should throw an error
            new CompilerException("Could not do Addition or Subtraction");
            return null;
        }

        /// <summary>
        /// Interpret an assignment
        /// </summary>
        /// <param name="basetype">The assignment</param>
        /// <returns>The value of the right side of the assignment (making it an expression)</returns>
        public object VisitAssignment(Assignment basetype)
        {
            // The name of the variable to assign to
            string variableName = (string)basetype.identifier.value;
            // The vale to assign t the variable
            object newValue = basetype.expression.Accept(this);

            // If we're not in a class (Global scope)
            if (CurrentClass == null)
            {            
                // If we're not in a Method in the global scope
                if (CurrentMethod == null)
                {
                    // Assign the value to a variable in the global scope
                    Environment.FindVariable(variableName).Value = newValue;
                }
                // If we're in a method within the global scope
                else
                {
                    // Assign the value to the variable within the method (or above)
                    CurrentMethod.FindVariable(variableName).Value = newValue;
                }
            }
            // If we're within a class
            else
            {
                // If we're not in a method within a clas
                if (CurrentMethod != null)
                {
                    // Assign the value to a variable within the class (or above)
                    CurrentMethod.FindVariable(variableName).Value = newValue;
                }
                // If we're in a method within a class
                else
                {
                    // Assign the value to the variable within the method within the class (or above)
                    CurrentClass.FindVariable(variableName).Value = newValue;
                }
            }
            
            // Return the value of the assignment
            return newValue;
        }

        /// <summary>
        /// Interprets a block
        /// </summary>
        /// <param name="basetype">The block to interpret</param>
        /// <returns>Should return nothing</returns>
        public object VisitBlock(Block basetype)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Interprets a class
        /// </summary>
        /// <param name="basetype">The class to interpret</param>
        /// <returns>Should return nothing</returns>
        public object VisitClassDecl(ClassDecl basetype)
        {
            // The name of the class
            string className = (string)basetype.className.Accept(this);

            // A class to match against
            Class c = new Class(className);

            // If we're not currently in a class (Global scope)
            if (CurrentClass == null)
            {
                // If we can't find the class within the environment
                if (!Environment.Classes.Any(n => n.ClassName == c.ClassName))
                {
                    // Throw an exception saying the class was never made
                    new CompilerException("Class: " + c.ClassName + " Could not be found");
                }
                // Set the class to the first occurence of the class
                c = Environment.Classes.First(n => n.ClassName == c.ClassName);
            }
            // If we're within a class (Sub class)
            else
            {
                // If we cannot find the class as a subclass of the current class
                if (!CurrentClass.Classes.Any(n => n.ClassName == c.ClassName))
                {
                    // Throw a nets
                    new CompilerException("Variable: " + c.ClassName + " Could not be found");
                }
                c = CurrentClass.Classes.First(n => n.ClassName == c.ClassName);
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
            object left = basetype.left.Accept(this);
            object right = basetype.right.Accept(this);
            switch (basetype.Operator.TokenType)
            {
                case TokenType.LESS:
                    return (int)left < (int)right;
                case TokenType.LESS_EQUAL:
                    return (int)left <= (int)right;
                case TokenType.GREATER:
                    return (int)left > (int)right;
                case TokenType.GREATER_EQUAL:
                    return (int)left >= (int)right;
            }
            new CompilerException("Could not do Greater or Less than");
            return null;
        }

        public object VisitDeclaration(Declaration basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitEquality(Equality basetype)
        {
            object left = basetype.left.Accept(this);
            object right = basetype.right.Accept(this);
            switch (basetype.Operator.TokenType)
            {
                case TokenType.EQUAL_EQUAL:
                    return left == right;
                case TokenType.BANG_EQUAL:
                    return left != right;
            }
            new CompilerException("Could not do Equal or Not Equal");
            return null;
        }

        public object VisitExpression(Expression basetype)
        {
            throw new NotImplementedException();
        }

        public object VisitGrouping(Grouping basetype)
        {
            return basetype.expr.Accept(this);
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
                    // Try to find it within the method
                    identifierValue = CurrentMethod.FindIdentifier(identifierName);

                    if (identifierValue == null)
                    {
                        // It could also be in the class
                        identifierValue = identifierValue = CurrentClass.FindIdentifier(identifierName);
                    }
                }
                else
                {
                    // In a class and not in a method
                    identifierValue = CurrentClass.FindIdentifier(identifierName);
                }
            }

            if (identifierValue is Variable v)
                return v.Value;
            else if (identifierValue is Method m)
                return m.MethodName;
            else if (identifierValue is Class c)
                return c.ClassName;
            else
                new CompilerException("Identifier: " + identifierName + " is an unknown type");
            return null;
        }

        public object VisitIf(If @if)
        {
            bool ifResult = (bool)@if.condition.Accept(this);
            if (ifResult)
            {
                foreach (Declaration item in @if.declarations)
                {
                    item.Accept(this);
                }
            }
            return null;
        }

        public object VisitLiteral(Literal literal)
        {
            return literal.value;
        }

        public object VisitLogic(Logic basetype)
        {
            object left = basetype.left.Accept(this);
            object right = basetype.right.Accept(this);
            switch (basetype.Operator.TokenType)
            {
                case TokenType.AND:
                    return (bool)left && (bool)right;
                case TokenType.BANG_EQUAL:
                    return (bool)left || (bool)right;
            }
            new CompilerException("Could not do And or OR");
            return null;
        }

        public object VisitMethodBody(MethodBody methodBody)
        {
            if (methodBody.Caller == null)
                return null;
            foreach (Expression item in methodBody.arguments)
            {
                if (item is Identifier id)
                {
                    Variable v = new Variable(id.value);
                    v.ParentMethod = methodBody.parentMethod;
                    v.ParentClass = methodBody.parentMethod.ParentClass;
                    methodBody.parentMethod.Variables.Add(v);
                }
            }
            if (methodBody.arguments.Where(n => n is Identifier).Count() > 0)
            {
                for (int i = 0; i < methodBody.arguments.Count; i++)
                {
                    object result = methodBody.Caller.arguments[i].Accept(this);
                    methodBody.parentMethod.Variables.ElementAt(i).Value = result;
                }
            }
            CurrentMethod = methodBody.parentMethod;
            foreach (Declaration item in methodBody.body)
            {
                item.Accept(this);
            }
            CurrentMethod = CurrentMethod.ParentMethod;

            return null; //TODO: Return returnvalue
        }

        public object VisitMethodDecl(MethodDecl basetype)
        {
            string methodName = (string)basetype.methodName.value;
            Method m = new Method(methodName, null, null);

            if (CurrentClass == null)
            {
                if (CurrentMethod == null)
                {
                    // Global scope
                    if (!Environment.Methods.Any(n => n.MethodName == m.MethodName))
                    {
                        new CompilerException("Method: " + m.MethodName + " Could not be found");
                    }
                }
                else
                {
                    // Method in global scope
                    if (!CurrentMethod.Methods.Any(n => n.MethodName == m.MethodName))
                    {
                        new CompilerException("Method: " + m.MethodName + " Could not be found");
                    }
                }
            }
            else
            {
                // In a class
                if (CurrentMethod == null)
                {
                    // In a method within a class
                    if (!CurrentClass.Methods.Any(n => n.MethodName == m.MethodName))
                    {
                        new CompilerException("Method: " + m.MethodName + " Could not be found");
                    }
                }
                else
                {
                    // In a class and not in a method
                    if (!CurrentMethod.Methods.Any(n => n.MethodName == m.MethodName))
                    {
                        new CompilerException("Method: " + m.MethodName + " Could not be found");
                    }
                }
            }

            return m;
        }

        public object VisitMultiplication(Multiplication basetype)
        {
            object left = basetype.left.Accept(this);
            object right = basetype.right.Accept(this);
            switch (basetype.Operator.TokenType)
            {
                case TokenType.AND:
                    return (int)left * (int)right;
                case TokenType.BANG_EQUAL:
                    return (int)left / (int)right;
            }
            new CompilerException("Could not do Multiplication or Division");
            return null;
        }

        public object VisitNumber(Number basetype)
        {
            return basetype.value;
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
            return basetype.value;
        }

        public object VisitType(Type basetype)
        {
            return basetype.value;
        }

        public object VisitUnary(Unary basetype)
        {
            object right = basetype.right.Accept(this);
            switch (basetype.Operator.TokenType)
            {
                case TokenType.BANG:
                    return !(bool)right;
                case TokenType.MINUS:
                    return -(int)right;
            }
            new CompilerException("Could not do Negation or Bang");
            return null;
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
            bool whileResult = bool.Parse((string)@while.condition.Accept(this));
            while(whileResult)
            {
                foreach (Declaration item in @while.declarations)
                {
                    item.Accept(this);
                }
                whileResult = bool.Parse((string)@while.condition.Accept(this));
            }
            return null;
        }

        public object Interprete()
        {
            return VisitProgramStart(Root);
        }

        public object VisitMethodCall(MethodCall methodCall)
        {
            // Print function
            if (methodCall.calledMethod.MethodName == "print")
            {
                Console.WriteLine(methodCall.arguments[0].Accept(this));
                return null;
            }

            MethodBody bestBody = null;
            foreach(MethodBody methodBody in methodCall.calledMethod.Implementations)
            {
                bool correct = true;
                //Verify arguments
                for (int i = 0; i < methodBody.arguments.Count; i++)
                {
                    object result1 = methodCall.arguments[i].Accept(this);
                    object result2 = methodBody.arguments[i].Accept(this);
                    if (!(result1.Equals(result2)) && !(methodBody.arguments[i] is Identifier))
                        correct = false;
                }
                if (correct)
                {
                    bestBody = methodBody;
                    break;
                }
            }
            if (bestBody != null)
            {
                    
                bestBody.Caller = methodCall;
                return bestBody.Accept(this);
            }
            return null;
        }
    }
}
