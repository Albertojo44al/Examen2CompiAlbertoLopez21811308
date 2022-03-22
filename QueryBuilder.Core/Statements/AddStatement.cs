using QueryBuilder.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Statements
{
    public class AddStatement: Statement
    {
        public AddStatement(IEnumerable<Expression> properties , IEnumerable<Expression> newProperties)
        {
            Properties = properties;
            NewProperties = newProperties;
            //this.ValidateSemantic();
        }
        public IEnumerable<Expression> Properties { get; }
        public IEnumerable<Expression> NewProperties { get; }
        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override void GenerateCode()
        {
            throw new NotImplementedException();
        }
 
    }
}
