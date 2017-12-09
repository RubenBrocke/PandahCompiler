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

        public static Class FindClass(string className)
        {
            return Classes.First(n => n.ClassName == className);
        }
    }

    public class Class
    {
        public string ClassName { get; set; }
        public Class ParentClass { get; set; }

        public static List<Class> Classes = new List<Class>();
        public static List<Method> Methods = new List<Method>();
        public static List<Variable> Variables = new List<Variable>();

        public Class(string className)
        {
            ClassName = className;
        }
    }

    public class Method
    {
        public string MethodName { get; set; }
        public Class ParentClass { get; set; }
        public Method ParentMethod { get; set; }

        public static List<Method> Methods = new List<Method>();
        public static List<Variable> Variables = new List<Variable>();
    }

    public class Variable
    {
        public string VariableName { get; set; }
        public Class ParentClass { get; set; }
        public Method ParentMethod { get; set; }
    }
}
