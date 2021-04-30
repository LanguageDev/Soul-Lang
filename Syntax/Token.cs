
namespace SoulLang
{
    public class Token
    {
        public Position Position { get; }
        public TokenType Type { get; }
        public string Value { get; }
        
        public Token(Position position, TokenType type, string value)
        {
            Position = position;
            Type = type;
            Value = value;
        }
    }
}
