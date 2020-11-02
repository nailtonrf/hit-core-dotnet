using System.Data;

namespace Hitmu.Abstractions.Store.Contexts
{
    public interface ITransactionalDataContext : IDataContext
    {
        ITransactionAdapter BeginTransaction(IsolationLevel isolationLevel);
    }
}