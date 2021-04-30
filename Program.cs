using System;

namespace SoulLang
{
    class Program
    {
        static void Main(string[] args)
        {
            var ast = new PrintNode
            {
                ToPrint = new BinOpExpr
                {
                    Left = new NumExpr { Token = new Token(new Position(), TokenType.Integer, "3") },
                    Right = new NumExpr { Token = new Token(new Position(), TokenType.Integer, "4") },
                },
            };
            while (true)
            {
                var input = Console.ReadLine();
                var expr = new Parser(Lexer.Lex(input)).Parse_statement();
                var evaluated = new TreeEvaluator().Evaluate(expr);
                Console.WriteLine(evaluated);
            }
        }
    }
}
