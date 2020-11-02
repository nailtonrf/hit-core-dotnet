using System;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Store
{
    public interface ITransactionAdapter : IDisposable
    {
        Task CommitAsync();
    }
}