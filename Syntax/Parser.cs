using System;
using System.Collections.Generic;
using System.Linq;

namespace SoulLang
{
    public class Parser
    {

        private List<Token> tokens;
        private int index;

        private TokenType[] Operators = { 
            TokenType.Add, TokenType.Sub, TokenType.Mul, TokenType.Div, 
            TokenType.Equal, TokenType.NotEqual, TokenType.Greater, TokenType.GreatEq,
            TokenType.Less, TokenType.LessEq, TokenType.Or, TokenType.And
        };

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private Token Consume()
        {
            var current = tokens[index];
            index += 1;
            return current;
        }

        private Token Expect(TokenType kind)
        {
            var current = tokens[index];

            if (current.Type == kind)
            {
                return Consume();
            }
            else
            {
                throw new NotImplementedException($"Expected {kind} token");
            }
        }

        private bool Accept(TokenType kind) => Accept(kind, out var _);

        private bool Accept(TokenType kind, out Token result)
        {
            var current = tokens[index];

            if (current.Type == kind)
            {
                result = Consume();
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        private int Prece(TokenType op)
        {
            switch (op)
            {
                case TokenType.Add: return 1;
                case TokenType.Sub: return 1;
                case TokenType.Mul: return 2;
                case TokenType.Div: return 2;

                default: return 0;
            }
        }

        private bool Next_is_operator()
        {
            var current = tokens[index];
            return Operators.Contains(current.Type);
        }

        private Expression Parse_operator_expr()
        {
            var first = ParseTerm();
            if (Next_is_operator())
            {
                var op = Parse_Operator();
                return ParseBinOp(first, op);
            }
            else
            {
                return first;
            }
        }

        private Expression ParseTerm()
        {
            var t = tokens[index];
            return t.Type switch
            {
                TokenType.Integer => new NumExpr { Token = Consume() },
                TokenType.String => new StringExpr { Token = Consume() },
                TokenType.OpenParen => ParseParenthesizedExpr(),
                _ => throw new NotImplementedException($"Unexpected token {t.Value}"),
            };
        }

        private Expression ParseParenthesizedExpr()
        {
            Expect(TokenType.OpenParen);
            var inside = Parse_operator_expr();
            Expect(TokenType.CloseParen);
            return inside;
        }

        private Token ExpectAny(TokenType[] kinds)
        {
            var current = tokens[index];
            if (kinds.Contains(current.Type))
            {
                return Consume();
            }
            else
            {
                throw new NotImplementedException($"Token {current} was not in {string.Join(", ", kinds)}");
            }
        }

        private TokenType Parse_Operator()
        {
            return ExpectAny(Operators).Type;
        }

        private Expression ParseBinOp(Expression first, TokenType op)
        {
            var second = ParseTerm();
            if (Next_is_operator())
            {
                var next_ = Parse_Operator();
                if (Prece(op) >= Prece(next_))
                {
                    return ParseBinOp(BinOp(first, op, second), next_);
                }
                else
                {
                    return BinOp(first, op, ParseBinOp(second, next_));
                }
            }
            else
            {
                return BinOp(first, op, second);
            }
        }

        private Expression Parse_block()
        {
            Expect(TokenType.OpenBrace);
            var a = Parse_statement();
            Expect(TokenType.CloseBrace);
            return a;
        }

        private Expression Parse_if()
        {
            Expect(TokenType.IfKeyword);
            var cond = Parse_statement();
            var then = Parse_block();
            // C# can't deduce var for null, since it can be any nullable type
            Expression els = null;
            if (Accept(TokenType.ElseKeyword))
            {
                els = Parse_block();
            }
            // Yeah lets test it
            // I cant since this is on your machine.
            // I've already uploaded a repo to our org, you can copy that gitignore and just upload this to the org
            // You have rights to create repos there I think
            return new IfExpr { Cond = cond, Then = then, Else = els };
            // gonna save first
        }

        public Expression Parse_expr()
        {
            var current = tokens[index];
            if (current.Type == TokenType.IfKeyword)
            {
                return Parse_if();
            }
            else
            {
                return Parse_operator_expr();
            }
        }

        public Expression Parse_statement()
        {
            var result = new SequenceExpr { Statements = new List<Statement>() };
            result.Result = Parse_expr(); ;
            while (Accept(TokenType.SemiColon))
            {
                result.Statements.Add(new ExpressionStatement { Expr = result.Result });
                result.Result = Parse_expr();
            }
            return result;
        }

        private Expression BinOp(Expression left, TokenType op, Expression right)
        {
            return new BinOpExpr { Left = left, Operator = op, Right = right };
        }
    }

}