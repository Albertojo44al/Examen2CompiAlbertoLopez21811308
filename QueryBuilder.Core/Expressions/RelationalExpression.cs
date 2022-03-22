using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Expressions
{
    public class RelationalExpression : BinaryExpression
    {
        private readonly Dictionary<(CompilerType, CompilerType), CompilerType> _typeRules;

        public RelationalExpression(Token token, Expression leftexpression, Expression rightExpresion) : base(token, leftexpression, rightExpresion, null)
        {
            _typeRules = new Dictionary<(CompilerType, CompilerType), CompilerType>
            {
                {(CompilerType.Int, CompilerType.Int), CompilerType.Bool},
                {(CompilerType.Int, CompilerType.Float), CompilerType.Bool},
                {(CompilerType.Float, CompilerType.Int), CompilerType.Bool},
                {(CompilerType.Float, CompilerType.Float), CompilerType.Bool},
                {(CompilerType.String, CompilerType.String), CompilerType.Bool},
                {(CompilerType.Bool, CompilerType.Bool), CompilerType.Bool},
            };
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }

        public override CompilerType GetExpressionType()
        {
            var leftType = LeftExpression.GetExpressionType();
            var rightType = RightExpression.GetExpressionType();
            if (_typeRules.TryGetValue((leftType, rightType), out var resultType))
            {
                return resultType;
            }
            throw new ApplicationException($"Cannot perform relational operation on types {leftType} and {rightType}");
        }
    }
}
