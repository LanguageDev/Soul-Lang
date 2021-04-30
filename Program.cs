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

            // Ight lets test it
            // Just change the parser function
            // Alright, so this is test ready now?
            // yeah alr this should be okay
            // works perfectly!
            // brb in like 2 minutes
            // I think we're good to start working on the other nodes like "if", and functions
            // Yeah, lets go add that
            // Not 100%, you are calling Parse_expr here, don't you want to parse a sequence now?
            // Yep
            // ye kk
            // For if we will need a bool value in the runtime
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
