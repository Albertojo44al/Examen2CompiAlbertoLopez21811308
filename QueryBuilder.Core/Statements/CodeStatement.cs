using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Statements
{
    public class CodeStatement : Statement
    {
        public CodeStatement(Statement statement)
        {
            Statement = statement;   
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
