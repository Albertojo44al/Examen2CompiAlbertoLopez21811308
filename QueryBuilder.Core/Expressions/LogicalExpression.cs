using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Expressions
{
    public class LogicalExpression : BinaryExpression
    {
        private readonly Dictionary<(CompilerType, CompilerType), CompilerType> _typeRules;
        public LogicalExpression(Token token, Expression leftexpression, Expression rightExpresion) : base(token, leftexpression, rightExpresion, null)
        {
            _typeRules = new Dictionary<(CompilerType, CompilerType), CompilerType>
            {
                {(CompilerType.Bool, CompilerType.Bool), CompilerType.Bool},
               
            };
        }

        public override string GenerateCode()
        {
            var leftCode = this.LeftExpression.GenerateCode();
            var rightCode = this.RightExpression.GenerateCode();
            return $"{leftCode}{this.Token.Lexeme}{rightCode}";
        }

        public override CompilerType GetExpressionType()
        {
            var leftType = LeftExpression.GetExpressionType();
            var rightType = RightExpression.GetExpressionType();
            if (_typeRules.TryGetValue((leftType, rightType), out var resultType))
            {
                return resultType;
            }
            throw new ApplicationException($"Cannot perform logical operation on types {leftType} and {rightType}");
        }
    }
}
