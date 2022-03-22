using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Statements
{
    public class SequenceQueriesStatement : Statement
    {
        public Statement FirstStatement { get; }
        public Statement NextStatement { get; }

        public SequenceQueriesStatement(Statement firstStatement, Statement nextStatement)
        {
            FirstStatement = firstStatement;
            NextStatement = nextStatement;
            this.ValidateSemantic();
        }
        public override void GenerateCode()
        {
            throw new NotImplementedException();
        }

        public override void ValidateSemantic()
        {
            this.FirstStatement?.ValidateSemantic();
            this.NextStatement?.ValidateSemantic();
        }
    }
}
