using QueryBuilder.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Statements
{
    public class SelectStatement : Statement
    {
        public SelectStatement(IEnumerable<Expression> newProperties, IEnumerable<Expression> properties, Expression expression)
        {
            Properties = properties;
            NewProperties = newProperties;
            Expression = expression;
            //this.ValidateSemantic();
        }

        public IEnumerable<Expression> Properties { get; }
        public IEnumerable<Expression> NewProperties { get; }
        public Expression Expression { get; }
        public override void GenerateCode()
        {
           
        }

        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }
    }
}
