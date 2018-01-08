using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Parser
    {
        private List<Token> _tokens;
        private int _index;
        public ProgramStart Root;
        private Class currentClass;
        private Method currentMethod;

        public Parser(List<Token> inputTokens)
        {
            _tokens = inputTokens;
            _index = 0;
        }

        public void Parse()
        {
            Root = CreateProgram();
        }

        private ProgramStart CreateProgram()
        {
            List<Declaration> declarations = new List<Declaration>();
            while(PeekToken().TokenType != TokenType.EOF)
            {
                declarations.Add(CreateDeclaration());
            }
            return new ProgramStart(declarations);
        }

        private Statement CreateStatement()
        {
            if (Match(TokenType.IDENTIFIER))
            {
                Token ident = NextToken();
                if (Match(TokenType.LEFT_PAREN))
                {
                    if (_tokens[_tokens.FindIndex(_index, n => n.Value == ")") + 1].Value == "do")
                    {
                        return CreateMethodBody(ident);
                    }
                    else
                    {
                        return CreateMethodCall(ident);
                    }
                }
                if (Match(TokenType.EQUAL))
                {
                    return CreateAssignment(ident);
                }
                return null;
            }
            else if (Match(TokenType.WHILE))
            {
                return CreateWhile();
            }
            else
            {
                return CreateExpression();
            }
        }

        private Statement CreateMethodBody(Token ident)
        {
            Identifier identifier = new Identifier(ident.Value);
            Token leftParen = NextToken();
            List<Expression> arguments = new List<Expression>();
            if (PeekToken().TokenType != TokenType.RIGHT_PAREN)
            do
            {
                if (PeekToken().TokenType == TokenType.COMMA)
                    NextToken();
                arguments.Add(CreateExpression());
            }
            while (PeekToken().TokenType == TokenType.COMMA);
            Token rightParen = NextToken();
            Token doToken = NextToken();
            List<Declaration> body = new List<Declaration>();
            while(PeekToken().TokenType != TokenType.END)
            {
                body.Add(CreateDeclaration());
            }
            NextToken(); // End token

            Method m;
            //Find Parent Method
            if (currentMethod == null)
            {
                if (currentClass == null)
                {
                    m = Environment.FindMethod(identifier.value);
                }
                else
                {
                    m = currentClass.FindMethod(identifier.value);
                }
            }
            else
            {
                m = currentMethod.FindMethod(identifier.value);
            }

            if (m == null)
                new CompilerException("Cannot find parent method of: " + identifier.value);
            MethodBody methodBody = new MethodBody(identifier, arguments, body, m);
            m.Implementations.Add(methodBody);
            return methodBody;
        }

        private Statement CreateWhile()
        {
            Token whileToken = NextToken();
            Expression Condition = CreateExpression();
            if (PeekToken().Value != "do")
            {
                new CompilerException("EXPECTED DO");
            }
            NextToken();
            List<Declaration> declarations = new List<Declaration>();
            while(PeekToken().Value != "end")
            {
                declarations.Add(CreateDeclaration());
            }
            NextToken();

            return new While(Condition, declarations); 
        }

        private Declaration CreateDeclaration()
        {
            //Check for class declaration
            if (PeekToken().TokenType == TokenType.CLASS)
            {
                return CreateClassDecl();
            }
            //Check for Method Declaration
            else if (_index < _tokens.Count - 3 && _tokens[_index + 3].TokenType == TokenType.ARROW)
            {
                return CreateMethodDecl();
            }
            //Check for Var declaration
            else if (PeekToken(1).TokenType == TokenType.TYPE)
            {
                return CreateVarDecl();
            }
            //Check for statement
            else
            {
                return CreateStatement();
            }
        }

        private ClassDecl CreateClassDecl()
        {
            Token classToken = NextToken();
            Identifier identifier = new Identifier(NextToken().Value);

            Class c = new Class(identifier.value);
            if (currentClass != null)
            {
                // Make a sub class
                Class tempClass = currentClass;
                currentClass = c;
                currentClass.ParentClass = tempClass;
                tempClass.Classes.Add(currentClass);
            }
            else
            {
                currentClass = c;
                Environment.Classes.Add(currentClass);
            }

            List<Declaration> body = new List<Declaration>();
            while (!Match(TokenType.END))
            {
                body.Add(CreateDeclaration());
            }
            if (currentClass != null)
                currentClass = currentClass.ParentClass;
            Token end = NextToken();


            return new ClassDecl(identifier, body);
        }

        private VarDecl CreateVarDecl()
        {
            Identifier identifier = new Identifier(NextToken().Value);
            Token typeOp = NextToken();
            Type type = new Type(NextToken().Value);

            Variable variable = new Variable(identifier.value);

            if (currentMethod == null)
            {
                if (currentClass == null)
                {
                    Environment.Variables.Add(variable);
                }
                else
                {
                    currentClass.Variables.Add(variable);
                }
            }
            else
            {
                currentMethod.Variables.Add(variable);
            }

            return new VarDecl(identifier, type);
        }

        private MethodDecl CreateMethodDecl()
        {
            Identifier identifier = new Identifier(NextToken().Value);
            Token typeOp = NextToken();
            Type type = new Type(NextToken().Value);
            Token arrow = NextToken();
            List<Type> arguments = new List<Type>();
            do
            {
                if (PeekToken().TokenType == TokenType.COMMA)
                    NextToken();
                arguments.Add(new Type(NextToken().Value));
            }
            while (PeekToken().TokenType == TokenType.COMMA);

            Method m = new Method(identifier.value, type, arguments.ToArray());

            if (currentMethod == null)
            {
                if (currentClass == null)
                {
                    Environment.Methods.Add(m);
                }
                else
                {
                    m.ParentClass = currentClass;
                    currentClass.Methods.Add(m);
                }
            }
            else
            {
                m.ParentMethod = currentMethod;
                m.ParentClass = currentMethod.ParentClass;
                currentMethod.Methods.Add(m);
            }

            return new MethodDecl(identifier, type, arguments);
        }

        private Expression CreateExpression()
        {
            //Check for Assignment
            if (PeekToken().TokenType == TokenType.EQUAL)
            {
                return CreateAssignment();
            }
            //Check for Logic
            else
            {
                return CreateLogic();
            }

        }

        private Expression CreateAssignment(Token ident = null)
        {
            if (ident != null)
            {
                Identifier identifier = new Identifier(ident.Value);
                Token Operator = NextToken();
                Assignment assignment = new Assignment(identifier, CreateExpression());
                return assignment;
            }
            else if (Match(TokenType.IDENTIFIER))
            {
                Identifier identifier = new Identifier(NextToken().Value);
                Token Operator = NextToken();
                Assignment assignment = new Assignment(identifier, CreateExpression());
                return assignment;
            }
            else
            {
                new CompilerException("Can't assign to: " + PeekToken().Value);
            }
            return null;
        }

        private Expression CreateLogic()
        {
            Expression expression = CreateEquality();
            while (Match(TokenType.AND, TokenType.OR))
            {
                Token Operator = NextToken();
                Expression right = CreateEquality();
                expression = new Logic(expression, Operator, right);
            }

            return expression;
        }

        private Expression CreateEquality()
        {
            Expression expression = CreateComparison();
            while (Match(TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL))
            {
                Token Operator = NextToken();
                Expression right = CreateComparison();
                expression = new Equality(expression, Operator, right);
            }

            return expression;
        }

        private Expression CreateComparison()
        {
            Expression expression = CreateAddition();
            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token Operator = NextToken();
                Expression right = CreateAddition();
                expression = new Comparison(expression, Operator, right);                
            }

            return expression;
        }

        private Expression CreateAddition()
        {
            Expression expression = CreateMultiplication();
            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                Token Operator = NextToken();
                Expression right = CreateMultiplication();
                expression = new Addition(expression, Operator, right);
            }

            return expression;
        }

        private Expression CreateMultiplication()
        {
            Expression expression = CreateUnary();
            while (Match(TokenType.STAR, TokenType.SLASH))
            {
                Token Operator = NextToken();
                Expression right = CreateUnary();
                expression = new Multiplication(expression, Operator, right);
            }

            return expression;
        }

        private Expression CreateUnary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token Operator = NextToken();
                Expression right = CreateUnary();

                return new Unary(Operator, right);
            }

            return CreatePrimary();
        }

        private Expression CreatePrimary()
        {
            TokenType type = PeekToken().TokenType;
            if (type == TokenType.NULL) { return new Literal(NextToken().Literal); }
            if (type == TokenType.TRUE) { return new Literal(NextToken().Literal); }
            if (type == TokenType.FALSE) { return new Literal(NextToken().Literal); }

            if (type == TokenType.NUMBER) { return new Literal(NextToken().Literal); }
            if (type == TokenType.STRING) { return new Literal(NextToken().Literal); }
            if (type == TokenType.IDENTIFIER) { return new Identifier(NextToken().Value); }

            if (type == TokenType.LEFT_PAREN)
            {
                NextToken();
                Expression grouping = CreateExpression();
                if (PeekToken().TokenType != TokenType.RIGHT_PAREN)
                {
                    new CompilerException("Expected ) after (");
                }
                NextToken();
                return new Grouping(grouping);
            }

            new CompilerException("COULD NOT PARSE: " + NextToken().TokenType.ToString());
            return new Expression();
        }

        private Expression CreateMethodCall(Token token)
        {
            NextToken();
            List<Expression> arguments = new List<Expression>();
            if (PeekToken().TokenType != TokenType.RIGHT_PAREN)
                do
                {
                    if (PeekToken().TokenType == TokenType.COMMA)
                        NextToken();
                    Expression o = CreateExpression();
                    arguments.Add(o);
                }
                while (PeekToken().TokenType == TokenType.COMMA);
            Match(TokenType.RIGHT_PAREN);
            NextToken();

            Method method;

            if (currentMethod == null)
            {
                if (currentClass == null)
                {
                    method = Environment.FindMethod(token.Value);
                }
                else
                {
                    method = currentClass.FindMethod(token.Value);
                }
            }
            else
            {
                method = currentMethod.FindMethod(token.Value);
            }

            if (method == null)
                new CompilerException("Could not find method: " + token.Value);

            // It's a function call
            MethodCall m = new MethodCall(token.Value, arguments.ToArray())
            {
                calledMethod = method
            };
            return m;
        }

        private Token NextToken()
        {
            return _tokens[_index++];
        }

        private Token PeekToken(int lookahead = 0)
        {
            if (_index < _tokens.Count - lookahead)
                return _tokens[_index + lookahead];
            else
                return new Token(TokenType.NONE, "", null, -1);
        }

        private Token[] TakeWhile(Func<Token, bool> func)
        {
            List<Token> returnList = new List<Token>();
            while (_index < _tokens.Count && func(PeekToken()))
            {
                returnList.Add(NextToken());
            }
            return returnList.ToArray();
        }

        private bool Match(params TokenType[] types)
        {
            return types.Any(n => n == PeekToken().TokenType);
        }
    }
}
