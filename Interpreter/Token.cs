namespace Interpreter
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public object Literal { get; set; }
        public int Line { get; set; }

        public Token(TokenType tokentype, string value, object literal, int line)
        {
            TokenType = tokentype;
            Value = value;
            Literal = literal;
            Line = line;
        }

        public override string ToString()
        {
            return TokenType + " " + Value + " " + Literal;
        }
    }

}
