using QueryBuilder.Core.Enums;
using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Expressions
{
    public class ArithmeticExpression : BinaryExpression
    {

        private readonly Dictionary<(CompilerType, CompilerType, TokenType), CompilerType> _typeRules;
        public ArithmeticExpression(Token token, Expression leftExpression, Expression rightExpression)
            : base(token, leftExpression, rightExpression, null)
        {
            _typeRules = new Dictionary<(CompilerType, CompilerType, TokenType), CompilerType>
            {
                {(CompilerType.String, CompilerType.String, TokenType.Plus), CompilerType.String },
                {(CompilerType.Float, CompilerType.String, TokenType.Plus), CompilerType.String },
                {(CompilerType.String, CompilerType.Float, TokenType.Plus), CompilerType.String },
                {(CompilerType.Int, CompilerType.String, TokenType.Plus), CompilerType.String },
                {(CompilerType.String, CompilerType.Int, TokenType.Plus), CompilerType.String }, 

                {(CompilerType.Int, CompilerType.Int, TokenType.Multiplication), CompilerType.Int },
                {(CompilerType.Int, CompilerType.Int, TokenType.Plus), CompilerType.Int },
                {(CompilerType.Int, CompilerType.Int, TokenType.Division), CompilerType.Int },
                {(CompilerType.Int, CompilerType.Int, TokenType.Minus), CompilerType.Int },

                {(CompilerType.Float, CompilerType.Float, TokenType.Plus), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Float, TokenType.Multiplication), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Float, TokenType.Division), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Float, TokenType.Minus), CompilerType.Float },

                {(CompilerType.Int, CompilerType.Float, TokenType.Plus), CompilerType.Float },
                {(CompilerType.Int, CompilerType.Float, TokenType.Multiplication), CompilerType.Float },
                {(CompilerType.Int, CompilerType.Float, TokenType.Division), CompilerType.Float },
                {(CompilerType.Int, CompilerType.Float, TokenType.Minus), CompilerType.Float },

                {(CompilerType.Float, CompilerType.Int, TokenType.Plus), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Int, TokenType.Multiplication), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Int, TokenType.Division), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Int, TokenType.Minus), CompilerType.Float },

            };
        }

        public override string GenerateCode()
        {
            return string.Empty;
        }

    }
}
