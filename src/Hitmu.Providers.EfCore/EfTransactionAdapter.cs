using Hitmu.Abstractions.Store;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Hitmu.Providers.EfCore
{
    public sealed class EfTransactionAdapter : ITransactionAdapter
    {
        private readonly Func<EfTransactionAdapter, Task> _beforeCommitHandler;

        private readonly IDbContextTransaction _contextTransaction;
        private readonly Action<EfTransactionAdapter> _disposingHandler;
        private readonly Action<EfTransactionAdapter> _rollbackHandler;
        private bool _disposed;
        private bool _done;

        public EfTransactionAdapter(IDbContextTransaction contextTransaction,
            Func<EfTransactionAdapter, Task> beforeCommitHandler,
            Action<EfTransactionAdapter> rollbackHandler,
            Action<EfTransactionAdapter> disposingHandler)
        {
            _contextTransaction = contextTransaction ?? throw new ArgumentNullException(nameof(contextTransaction));
            _beforeCommitHandler = beforeCommitHandler;
            _rollbackHandler = rollbackHandler;
            _disposingHandler = disposingHandler;
        }

        public async Task CommitAsync()
        {
            await _beforeCommitHandler(this).ConfigureAwait(false);
            await _contextTransaction.CommitAsync().ConfigureAwait(false);
            _done = true;
        }

        public void Dispose()
        {
            InternalDispose();
            GC.SuppressFinalize(this);
        }

        ~EfTransactionAdapter()
        {
            InternalDispose();
        }

        private void InternalDispose()
        {
            if (_disposed) return;

            if (!_done)
            {
                _contextTransaction?.Rollback();
                _rollbackHandler?.Invoke(this);
            }

            _contextTransaction?.Dispose();
            _disposingHandler?.Invoke(this);
            _disposed = true;
        }
    }
}