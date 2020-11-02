using System.Threading.Tasks;

namespace Hitmu.Abstractions.Store
{
    public sealed class NestedTransactionAdapter : ITransactionAdapter
    {
        public void Dispose()
        {
            //NullPattern
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public static NestedTransactionAdapter CreateNestedTransaction()
        {
            return new NestedTransactionAdapter();
        }
    }
}