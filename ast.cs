using System;
using System.Collections.Generic;

namespace SoulLang
{
	public class AST { }

	public class Expression : AST { }

	public class Statement : AST { }

	public class ExpressionStatement : Statement
	{
		public Expression Expr { get; set; }
	}

	public class PrintNode : Statement
	{
		public Expression ToPrint { get; set; }
	}

	public class IfExpr : Expression
	{
		public Expression Cond { get; set; }
		public Expression Then { get; set; }
		public Expression Else { get; set; }
	}

	public class SequenceExpr : Expression
	{
		public List<Statement> Statements { get; set; }
		public Expression Result { get; set; }
	}

	public class BinOpExpr : Expression
	{
		public Expression Left { get; set; }
		public TokenType Operator { get; set; }
		public Expression Right { get; set; }
	}

	public class NumExpr : Expression
	{
		public Token Token { get; set; }
	}

	public class StringExpr : Expression
	{
		public Token Token { get; set; }
	}
}