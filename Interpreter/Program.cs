using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = File.ReadAllText("../../Input.txt");
            Scanner scanner = new Scanner(code);
            List<Token> tokens = scanner.Scan();
            scanner.PrintTokens();
            Parser parser = new Parser(tokens);
            parser.Parse();
            AstPrinter astPrinter = new AstPrinter();
            astPrinter.VisitProgramStart((ProgramStart)parser.Root);
            Console.ReadLine();
        }
    }
}
