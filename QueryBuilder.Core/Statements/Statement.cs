using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Core.Statements
{
    public abstract class Statement
    {
        public abstract void ValidateSemantic();

        public abstract void GenerateCode(); 
    }
}
