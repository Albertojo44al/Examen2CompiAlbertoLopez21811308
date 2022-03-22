using QueryBuilder.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Statements
{
    public class UpdateStatement : Statement
    {

        public UpdateStatement(IEnumerable<Expression> newProperties, IEnumerable<Expression> properties ,  Expression expression)
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
            throw new NotImplementedException();
        }

        public override void ValidateSemantic()
        {
            
        }
    }
}
