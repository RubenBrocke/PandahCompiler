using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public static class Syntax
    {
        public static string VarRegex = "[a-zA-Z_][a-zA-Z0-9_]*";

        public static Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>()
        {
            { "and",    TokenType.AND },
            { "or",     TokenType.OR },
            { "class",  TokenType.CLASS },
            { "else",   TokenType.ELSE },
            { "false",  TokenType.FALSE },
            { "for",    TokenType.FOR },
            { "fun",    TokenType.FUN },
            { "if",     TokenType.IF },
            { "null",   TokenType.NULL },
            { "print",  TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "this",   TokenType.THIS },
            { "true",   TokenType.TRUE },
            { "while",  TokenType.WHILE },
            { "end",    TokenType.END}
        };
    }
}
