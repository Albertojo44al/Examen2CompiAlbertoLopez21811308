using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Expressions
{
    public class BinaryExpression : Expression
    {
        public BinaryExpression(Token token, Expression leftexpression, Expression rightExpresion, CompilerType compilerType) : base(token, compilerType)
        {
            LeftExpression = leftexpression;
            RightExpression = rightExpresion;
        }
        public Expression LeftExpression { get; }
        public Expression RightExpression { get; }


        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }

        public override CompilerType GetExpressionType()
        {
            throw new NotImplementedException();
        }
    }
}
