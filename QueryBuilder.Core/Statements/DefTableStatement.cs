using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Statements
{
    public class DefTableStatement : Statement
    {
        public DefTableStatement(Statement statement)
        {
            Statement = statement;
            //this.ValidateSemantic();
        }
        public Statement Statement { get; }
        public override void GenerateCode()
        {
            throw new NotImplementedException();
        }

        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }
    }
}
