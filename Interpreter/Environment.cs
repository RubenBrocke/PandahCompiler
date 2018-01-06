using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public static class Environment
    {
        public static List<Class> Classes = new List<Class>();
        public static List<Method> Methods = new List<Method>();
        public static List<Variable> Variables = new List<Variable>();
        public static StringBuilder stringBuilder = new StringBuilder();

        public static Class FindClass(string className)
        {
            Class c = Classes.FirstOrDefault(n => n.ClassName == className);
            if (c == null)
            {
                throw new CompilerException("Could not find class: " + c.ClassName);
            }
            return c;
        }

        public static Method FindMethod(string methodName)
        {
            Method m = Methods.FirstOrDefault(n => n.MethodName == methodName);
            if (m == null)
            {
                throw new CompilerException("Could not find method: " + m.MethodName);
            }
            return m;
        }

        public static Variable FindVariable(string varName)
        {
            Variable v = Variables.FirstOrDefault(n => n.VariableName == varName);
            if (v == null)
            {
                new CompilerException("Could not find variable: " + varName);
            }
            return v;
        }

        public static object FindIdentifier(string idName)
        {
            object returnObject;
            returnObject = Classes.FirstOrDefault(n => n.ClassName == idName);
            if (returnObject != null) return returnObject;
            returnObject = Methods.FirstOrDefault(n => n.MethodName == idName);
            if (returnObject != null) return returnObject;
            returnObject = Variables.FirstOrDefault(n => n.VariableName == idName);
            return returnObject;
        }

        public static string PrintEnvironment()
        {
            stringBuilder.Append("Environment: \n");
            if (Classes.Count > 0)
            {
                stringBuilder.Append("Global Classes: " + Classes.Count + '\n');
                foreach (Class c in Classes)
                {
                    stringBuilder.Append("    ");
                    c.PrintClass();
                }
            }
            if (Methods.Count > 0)
            {
                stringBuilder.Append("Global Methods: " + Methods.Count + '\n');
                foreach (Method m in Methods)
                {
                    stringBuilder.Append("    ");
                    m.PrintMethod();
                }
            }
            if (Variables.Count > 0)
            { 
                stringBuilder.Append("Global Variables: " + Variables.Count + '\n');
                foreach (Variable v in Variables)
                {
                    stringBuilder.Append("    ");
                    v.PrintVariable();
                }
            }

            return stringBuilder.ToString();
        }
    }

    public class Class
    {
        public string ClassName { get; set; }
        public Class ParentClass { get; set; }

        public List<Class> Classes = new List<Class>();
        public List<Method> Methods = new List<Method>();
        public List<Variable> Variables = new List<Variable>();

        public Class(string className)
        {
            ClassName = className;
        }

        public Class FindClass(string className)
        {
            Class c = Classes.FirstOrDefault(n => n.ClassName == className);
            if (c == null)
            {
                throw new CompilerException("Could not find class: " + c.ClassName);
            }
            return c;
        }

        public Method FindMethod(string methodName)
        {
            Method m = Methods.FirstOrDefault(n => n.MethodName == methodName);
            if (m == null)
            {
                throw new CompilerException("Could not find method: " + m.MethodName);
            }
            return m;
        }

        public Variable FindVariable(string varName)
        {
            Variable v = Variables.FirstOrDefault(n => n.VariableName == varName);
            if (v == null)
            {
                throw new CompilerException("Could not find variable: " + v.VariableName);
            }
            return v;
        }

        public object FindIdentifier(string idName)
        {
            object returnObject;
            returnObject = Classes.FirstOrDefault(n => n.ClassName == idName);
            if (returnObject != null) return returnObject;
            returnObject = Methods.FirstOrDefault(n => n.MethodName == idName);
            if (returnObject != null) return returnObject;
            returnObject = Variables.FirstOrDefault(n => n.VariableName == idName);
            return returnObject;
        }

        public void PrintClass()
        {
            Environment.stringBuilder.Append("Class: " + ClassName + '\n');
            if (Classes.Count > 0)
            {
                Environment.stringBuilder.Append("Subclasses: " + Classes.Count + '\n');
                foreach (Class c in Classes)
                {
                    Environment.stringBuilder.Append("    ");
                    c.PrintClass();
                }
            }
            if (Methods.Count > 0)
            {
                Environment.stringBuilder.Append("Methods: " + Methods.Count + '\n');
                foreach (Method m in Methods)
                {
                    Environment.stringBuilder.Append("    ");
                    m.PrintMethod();
                }
            }
            if (Variables.Count > 0)
            {
                Environment.stringBuilder.Append("Variables: " + Variables.Count + '\n');
                foreach (Variable v in Variables)
                {
                    Environment.stringBuilder.Append("    ");
                    v.PrintVariable();
                }
            }
        }
    }

    public class Method
    {
        public string MethodName { get; set; }
        public Type[] ArgumentTypes { get; set; }
        public Class ParentClass { get; set; }
        public Method ParentMethod { get; set; }

        public List<Method> Methods = new List<Method>();
        public List<Variable> Variables = new List<Variable>();

        public Method(string name, Type[] arguments)
        {
            MethodName = name;
            ArgumentTypes = arguments;
        }

        public Method FindMethod(string methodName)
        {
            Method m = Methods.FirstOrDefault(n => n.MethodName == methodName);
            if (m == null)
            {
                throw new CompilerException("Could not find method: " + m.MethodName);
            }
            return m;
        }

        public Variable FindVariable(string varName)
        {
            Variable v = Variables.FirstOrDefault(n => n.VariableName == varName);
            if (v == null)
            {
                throw new CompilerException("Could not find variable: " + v.VariableName);
            }
            return v;
        }

        public object FindIdentifier(string idName)
        {
            object returnObject;
            returnObject = Methods.FirstOrDefault(n => n.MethodName == idName);
            if (returnObject != null) return returnObject;
            returnObject = Variables.FirstOrDefault(n => n.VariableName == idName);
            return returnObject;
        }

        public void PrintMethod()
        {
            Environment.stringBuilder.Append(MethodName + '\n');
            Environment.stringBuilder.Append("Argument types: " + '\n');
            foreach (Type T in ArgumentTypes)
            {
                Environment.stringBuilder.Append(T.ToString());
            }
            if (Methods.Count > 0)
            {
                Environment.stringBuilder.Append("Submethods: " + Methods.Count + '\n');
                foreach (Method m in Methods)
                {
                    Environment.stringBuilder.Append("    ");
                    m.PrintMethod();
                }
            }
            if (Variables.Count > 0)
            {
                Environment.stringBuilder.Append("Variables: " + Variables.Count + '\n');
                foreach (Variable v in Variables)
                {
                    Environment.stringBuilder.Append("    ");
                    v.PrintVariable();
                }
            }


        }
    }

    public class Variable
    {
        public string VariableName { get; set; }
        public Class ParentClass { get; set; }
        public Method ParentMethod { get; set; }
        public object Value { get; set; }

        public Variable(string variableName, object value = null)
        {
            VariableName = variableName;
        }

        public void PrintVariable()
        {
            Environment.stringBuilder.Append(VariableName + ": " + Value + '\n');
        }
    }
}
