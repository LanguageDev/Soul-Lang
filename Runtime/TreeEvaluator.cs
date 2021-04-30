using System;

namespace SoulLang
{
    public class TreeEvaluator
    {
        public void Execute(Statement statement)
        {
            switch (statement)
            {
                case PrintNode print:
                    Console.WriteLine(Evaluate(print.ToPrint).ToString());
                    break;
            }
        }

        public IValue Evaluate(Expression expression)
        {
            switch (expression)
            {
                case NumExpr num:
                    return new IntegerValue(int.Parse(num.Token.Value));

                case StringExpr str:
                    return new StringValue(str.Token.Value);

                case BinOpExpr binOp:
                    {
                        if (binOp.Operator == TokenType.Add)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                return new IntegerValue(i1.Value + i2.Value);
                            }
                            return new StringValue(left.ToString() + right.ToString());
                        }
                        else if (binOp.Operator == TokenType.Sub)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                return new IntegerValue(i1.Value - i2.Value);
                            }
                            else
                            {
                                throw new NotImplementedException($"Values {left} and {right}, were not of type int, or float, and could not be subtracted");
                            }
                        }
                        else if (binOp.Operator == TokenType.Mul)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                return new IntegerValue(i1.Value * i2.Value);
                            }
                            else
                            {
                                throw new NotImplementedException($"Values {left} and {right}, were not of type int, or float, and could not be multiplied");
                            }
                        }
                        else if (binOp.Operator == TokenType.Div)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                return new IntegerValue(i1.Value / i2.Value);
                            }
                            else
                            {
                                throw new NotImplementedException($"Values {left} and {right}, were not of type int, or float, and could not be divided");
                            }
                        }
                        else if (binOp.Operator == TokenType.Equal)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                // Should be bool value
                                return new BoolValue(i1.Value == i2.Value);
                            }
                            // Should be bool value
                            return new BoolValue(left.ToString() == right.ToString());
                        } 
                        else if (binOp.Operator == TokenType.Greater)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                return new BoolValue(i1.Value > i2.Value);
                            }
                            else
                            {
                                throw new NotImplementedException($"Values {left} and {right}, were not of type int, or float, and could not be divided");
                            }
                        }
                        else if (binOp.Operator == TokenType.GreatEq)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                return new BoolValue(i1.Value >= i2.Value);
                            }
                            else
                            {
                                throw new NotImplementedException($"Values {left} and {right}, were not of type int, or float, and could not be divided");
                            }
                        }
                        else if (binOp.Operator == TokenType.Less)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                return new BoolValue(i1.Value < i2.Value);
                            }
                            else
                            {
                                throw new NotImplementedException($"Values {left} and {right}, were not of type int, or float, and could not be divided");
                            }
                        }
                        else if (binOp.Operator == TokenType.LessEq)
                        {
                            var left = Evaluate(binOp.Left);
                            var right = Evaluate(binOp.Right);
                            if (left is IntegerValue i1 && right is IntegerValue i2)
                            {
                                return new BoolValue(i1.Value <= i2.Value);
                            }
                            else
                            {
                                throw new NotImplementedException($"Values {left} and {right}, were not of type int, or float, and could not be divided");
                            }
                        }
                        else if (binOp.Operator == TokenType.Or)
                        {
                            var left = Evaluate(binOp.Left);
                            if (left is BoolValue bv)
                            {
                                if (bv.Value) return bv;
                                return Evaluate(binOp.Right);
                            }
                            else
                            {
                                throw new NotImplementedException("type error");
                            }
                        }
                        else if (binOp.Operator == TokenType.And)
                        {
                            var left = Evaluate(binOp.Left);
                            if (left is BoolValue bv)
                            {
                                if (!bv.Value) return bv;
                                return Evaluate(binOp.Right);
                            }
                            else
                            {
                                throw new NotImplementedException("type error");
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }

                case SequenceExpr seq:
                    {
                        // In case of sequences we first execute the statements
                        foreach (var statement in seq.Statements)
                        {
                            Execute(statement);
                        }
                        // Finally evaluate the expression
                        return Evaluate(seq.Result);
                    }

                case IfExpr ifExpr:
                    {
                        var cond = Evaluate(ifExpr.Cond);
                        var then = ifExpr.Then;
                        var els = ifExpr.Else;
                        if (cond is BoolValue bv && bv.Value)
                        {
                            return Evaluate(ifExpr.Then);
                        }
                        else if (els != null)
                        {
                            return Evaluate(ifExpr.Else);
                        }
                        return null;
                    }

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
