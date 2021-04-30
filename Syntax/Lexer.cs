using System;
using System.Collections.Generic;

namespace SoulLang
{
    public class Lexer
    {
        // A wrapper function for a nicer lexer interface
        // Collects all tokens into a list for a piece of source code
        public static List<Token> Lex(string source)
        {
            var result = new List<Token>();
            var lexer = new Lexer(source);
            while (true)
            {
                var token = lexer.NextToken();
                result.Add(token);
                // The EOF token was hit, there will be no more tokens, break loop
                if (token.Type == TokenType.EndOfFile) break;
            }
            return result;
        }

        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            { "if", TokenType.IfKeyword },
            { "else", TokenType.ElseKeyword },
            { "func", TokenType.FuncKeyword },
        };
        // Wait! dont do parse statements quiet yet lol, im sorry
        private string source;
        private int index;
        private Position position;
        
        private Lexer(string source)
        {
            this.source = source;
        }

        // The main functionality, that lexes a single token
        private Token NextToken()
        {
            begin:
            // EOF
            if (index >= source.Length) return new Token(position, TokenType.EndOfFile, "");
            
            // Whitespace
            if (char.IsWhiteSpace(Peek()))
            {
                Skip();
                // Goto might feel cheeky here, it's actually a bit more sensical here then a while loop
                goto begin;
            }
            // Comments
            if (Peek() == '/' && Peek(1) == '/')
            {
                // This is basically: skip until end of line or end of file
                Skip(2);
                for (; Peek(0, '\n') != '\n'; Skip())  ;
                goto begin;
            }
            
            // Actual tokens come here

            // Numbers
            if (char.IsDigit(Peek()))
            {
                int length;
                for (length = 0; char.IsDigit(Peek(length)); ++length)  ;
                return Consume(TokenType.Integer, length);
            }
            // Yeah
            if (Peek() == '"')
            {
                int length = 1;
                while (true)
                {
                    if (Peek(length, '"') == '"')
                    {
                        // Close quote found
                        ++length;
                        break;
                    }
                    if (Peek(length) == '\\')
                    {
                        // Escaped thing
                        length += 2;
                        continue;
                    }
                    // Any other character
                    length += 1;
                }
                return Consume(TokenType.String, length);
            }
            // Identifiers
            if (char.IsLetterOrDigit(Peek()))
            {
                int length;
                for (length = 0; char.IsLetterOrDigit(Peek(length)); ++length) ;
                var result = Consume(TokenType.Identifier, length);
                if (keywords.TryGetValue(result.Value, out var tokenType))
                {
                    return new Token(result.Position, tokenType, result.Value);
                }
                return result;
            }
            switch (Peek())
            {
            case '(': return Consume(TokenType.OpenParen, 1);
            case ')': return Consume(TokenType.CloseParen, 1);
            case '+': return Consume(TokenType.Add, 1);
            case '-': return Consume(TokenType.Sub, 1);
            case '/': return Consume(TokenType.Div, 1);
            case '*': return Consume(TokenType.Mul, 1);
            case '{': return Consume(TokenType.OpenBrace, 1);
            case '}': return Consume(TokenType.CloseBrace, 1);
            case ';': return Consume(TokenType.SemiColon, 1);
                // Yeah here you need to look ahead 1 character to see if its also a =
                // You do it with condition ? then : else instead of if-else
            case '=': return (Peek(1) == '=') ? Consume(TokenType.Equal, 2) : Consume(TokenType.Assign, 1);
            case '!': return (Peek(1) == '=') ? Consume(TokenType.NotEqual, 2) : Consume(TokenType.Excl, 1);
            case '>': return (Peek(1) == '=') ? Consume(TokenType.GreatEq, 2) : Consume(TokenType.Greater, 1);
            case '<': return (Peek(1) == '=') ? Consume(TokenType.LessEq, 2) : Consume(TokenType.Less, 1);
            case '&': return (Peek(1) == '&') ? Consume(TokenType.And, 2) : Consume(TokenType.Unkown, 1);
            case '|': return (Peek(1) == '|') ? Consume(TokenType.Or, 2) : Consume(TokenType.Unkown, 1);
                // Cool, looks fine
                // So we just add functionality now?
                // Yeah, after the parser can recognize these as binary tokens, you handle them in the evaluator
            }
            throw new InvalidOperationException($"Could not recognize token starting with '{Peek()}'");
        }

        private Token Consume(TokenType tokenType, int length)
        {
            var value = source.Substring(index, length);
            index += length;
            var result = new Token(position, tokenType, value);
            foreach (var ch in value) AdvancePosition(ch);
            return result;
        }

        private void Skip(int amount = 1)
        {
            var value = source.Substring(index, amount);
            index += amount;
            foreach (var ch in value) AdvancePosition(ch);
        }

        private void AdvancePosition(char ch)
        {
            if (ch == '\n')
            {
                // Newline
                position = new Position(line: position.Line + 1, 0);
            }
            else if (!char.IsControl(ch))
            {
                // Non-control characters are characters that take up some space at least, horizontal increment
                position = new Position(line: position.Line, column: position.Column + 1);
            }
        }

        private char Peek(int offset = 0, char def = '\0') =>
            index + offset >= source.Length ? def : source[index + offset];
    }
}
