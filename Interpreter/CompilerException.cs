using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    class CompilerException : Exception
    {
        public CompilerException(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("COMPILER ERROR THROWN!");
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
