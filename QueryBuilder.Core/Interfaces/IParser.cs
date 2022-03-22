using QueryBuilder.Core.Statements;

namespace QueryBuilder.Core.Interfaces
{

    public interface IParser
    {
        Statement Parse();
    }
}