using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Interpreter
{
    class Scanner
    {
        private int _index;
        private string _code;
        private List<Token> _tokenList = new List<Token>();
        private int _line = 1;
        private bool _isAtEnd
        {
            get
            {
                return _index >= _code.Length;
            }
        }

        private char[] _chars
        {
            get
            {
                return _code.ToCharArray();
            }
        }

        public Scanner(string code)
        {
            _code = code;
        }

        public List<Token> Scan()
        {

            while(!_isAtEnd)
            {
                //Check single character Tokens
                Char c = NextChar();

                switch (c)
                {
                    case '(': AddToken(TokenType.LEFT_PAREN, "("); break;
                    case ')': AddToken(TokenType.RIGHT_PAREN, ")"); break;
                    case ',': AddToken(TokenType.COMMA, ","); break;
                    case '.': AddToken(TokenType.DOT, "."); break;
                    case '-': AddToken(TokenType.MINUS, "-"); break;
                    case '+': AddToken(TokenType.PLUS, "+"); break;
                    case '*': AddToken(TokenType.STAR, "*"); break;
                    case '=': if (PeekChar() == '=') { AddToken(TokenType.EQUAL_EQUAL, "=="); } else { AddToken(TokenType.EQUAL, "="); } break;
                    case '!': if (PeekChar() == '=') { AddToken(TokenType.BANG_EQUAL, "!="); } else { AddToken(TokenType.BANG, "!"); } break;
                    case '>': if (PeekChar() == '=') { AddToken(TokenType.GREATER_EQUAL, ">="); } else { AddToken(TokenType.GREATER, ">"); } break;
                    case '<': if (PeekChar() == '=') { AddToken(TokenType.LESS_EQUAL, "<="); } else if (PeekChar() == '-') { AddToken(TokenType.ARROW, "<-"); _index++;} else { AddToken(TokenType.LESS, "<"); } break;
                    case ':': if (PeekChar() == ':') { AddToken(TokenType.TYPE, "::"); } else { new CompilerException("Expected :: at line " + _line); } break;
                    case '#': while(PeekChar() != '\n') { NextChar(); } break;
                    case '/':
                        if (PeekChar() == '/')
                            TakeWhile(n => n != '\n');
                        else
                            AddToken(TokenType.SLASH, "/");
                        break;                    
                    case ' ': break;
                    case '\r': break;
                    case '\t': break;
                    case '\n': _line++; break;
                    case '"': String(); break;
                    default:
                        //Check for Literal
                        if (char.IsDigit(c))
                        {
                            string number = c.ToString();
                            number += new string(TakeWhile(n => char.IsDigit(n)));
                            object literal = Int32.Parse(number);
                            if (!_isAtEnd && PeekChar() == '.' && char.IsDigit(PeekNextChar()))
                            {
                                number += NextChar();
                                number += new string(TakeWhile(n => char.IsDigit(n)));
                                literal = Double.Parse(number);
                            }                            
                            //Create a new token
                            _tokenList.Add(new Token(TokenType.NUMBER, number, literal, _line));
                        }
                        //Check for Identifier
                        else if (Regex.IsMatch(c.ToString(), Syntax.VarRegex))
                        {
                            string IdString = new string(TakeWhile(n => Regex.IsMatch(n.ToString(), Syntax.VarRegex)));
                            if (Syntax.Keywords.ContainsKey(c + IdString))
                                //It's a keyword
                                _tokenList.Add(new Token(Syntax.Keywords[c + IdString], c + IdString, c + IdString, _line));
                            else
                            //Create a new token
                            _tokenList.Add(new Token(TokenType.IDENTIFIER, c + IdString, null, _line));
                        }
                        else
                        {
                            new CompilerException(string.Format("Unknown character {0} at line {1}", c, _line));
                        }
                        break;
                }
            }

            _tokenList.Add(new Token(TokenType.EOF, "", null, _line));
            return _tokenList;
        }

        private void String()
        {
            string newString = "\"";
            newString += new string(TakeWhile(n => n != '"'));

            if (_isAtEnd)
            {
                new CompilerException("String not terminated at line: " + _line);
                return;
            }

            _line += newString.Where(n => n == '\n').Count();

            newString += NextChar();

            AddToken(TokenType.STRING, newString);
        }

        private void AddToken(TokenType tokenType, string value)
        {
            _tokenList.Add(new Token(tokenType, value, null, _line));
            for (int i = 0; i < value.Length - 1; i++)
            {
                NextChar();
            }
        }

        private Char NextChar()
        {
            return _code.ElementAt(_index++);
        }

        private Char PeekChar()
        {
            return _code.ElementAt(_index);
        }

        private Char PeekNextChar()
        {
            if (_index + 1 > _code.Length - 1) return '\0';
            return _code.ElementAt(_index + 1);
        }

        private Char[] TakeWhile(Func<char, bool> func)
        {
            List<Char> returnList = new List<char>();
            while (!_isAtEnd && func(PeekChar()))
            {
                returnList.Add(NextChar());
            }
            return returnList.ToArray();
        }

        public void PrintTokens()
        {
            foreach (Token item in _tokenList)
            {
                Console.WriteLine(item.TokenType + " : " + item.Value);
            }
        }
    }
}
