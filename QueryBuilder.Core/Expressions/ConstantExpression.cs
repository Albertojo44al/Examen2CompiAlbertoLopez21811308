using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Expressions
{
    public class ConstantExpression : Expression
    {
        public ConstantExpression(Token token, CompilerType compilerType) : base(token, compilerType)
        {
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }

        public override CompilerType GetExpressionType()
        {
            return CompilerType;
        }
    }
}
