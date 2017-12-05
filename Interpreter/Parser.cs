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
        private BaseType Root;
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
            if (Match(TokenType.WHILE))
            {
                return CreateWhile();
            }
            else
            {
                return CreateExpression();
            }
        }

        private Statement CreateWhile()
        {
            Token whileToken = NextToken();
            Expression Condition = CreateExpression();
            if (PeekToken().Value != "DO")
            {
                new CompilerException("EXPECTED DO");
            }
            List<Declaration> declarations = new List<Declaration>();
            while(PeekToken().Value != "END")
            {
                declarations.Add(CreateDeclaration());
            }

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
            else if (_tokens[_index + 1].TokenType == TokenType.TYPE)
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
            return new ClassDecl(identifier);
        }

        private VarDecl CreateVarDecl()
        {
            Identifier identifier = new Identifier(NextToken().Value);
            Token typeOp = NextToken();
            Type type = new Type(NextToken().Value);

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

            return new MethodDecl(identifier, type, arguments);
        }

        private Expression CreateExpression()
        {
            //Check for Assignment
            if (_tokens[_index + 1].TokenType == TokenType.EQUAL)
            {
                return CreateAssignment();
            }
            //Check for Logic
            else
            {
                return CreateLogic();
            }

        }

        private Expression CreateAssignment()
        {
            if (Match(TokenType.IDENTIFIER))
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
            if (type == TokenType.NULL) { return new Literal(null); }
            if (type == TokenType.TRUE) { return new Literal(true); }
            if (type == TokenType.FALSE) { return new Literal(false); }

            if (type == TokenType.NUMBER) { return new Literal(NextToken().Literal); }
            if (type == TokenType.STRING) { return new Literal(NextToken().Literal); }

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

            new CompilerException("COULD NOT PARSE: " + NextToken().Value);
            return new Expression();
        }

        private Token NextToken()
        {
            return _tokens[_index++];
        }

        private Token PeekToken()
        {
            return _tokens[_index];
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
