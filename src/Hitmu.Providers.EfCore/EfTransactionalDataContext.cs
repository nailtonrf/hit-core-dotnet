using Hitmu.Abstractions.Core.Messaging;
using Hitmu.Abstractions.Store;
using Hitmu.Abstractions.Store.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Hitmu.Providers.EfCore
{
    public abstract class EfTransactionalDataContext : DbContext, ITransactionalDataContext
    {
        private readonly object _locker = new object();
        private readonly ILogger _logger;
        private readonly IMessageDispatcher _messageDispatcher;

        private EfTransactionAdapter? _currentTransaction;

        protected EfTransactionalDataContext(DbContextOptions options, IMessageDispatcher messageDispatcher,
            ILoggerFactory loggerFactory) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            _messageDispatcher = messageDispatcher ?? throw new ArgumentNullException(nameof(messageDispatcher));
            _logger = loggerFactory.CreateLogger(nameof(EfTransactionalDataContext));
        }

        public ITransactionAdapter BeginTransaction(IsolationLevel isolationLevel)
        {
            lock (_locker)
            {
                if (_currentTransaction != null) return NestedTransactionAdapter.CreateNestedTransaction();
                _currentTransaction = new EfTransactionAdapter(Database.BeginTransaction(isolationLevel),
                    CurrentTransaction_OnBeforeCommit, CurrentTransaction_OnRollback, CurrentTransaction_OnDisposing);
                return _currentTransaction;
            }
        }

        public override void Dispose()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "disposing currentTransaction");
            }

            base.Dispose();
        }

        private async Task CurrentTransaction_OnBeforeCommit(EfTransactionAdapter transactionAdapter)
        {
            _logger.LogInformation("dispatching-domainEvents");
            await _messageDispatcher.DispatchBeforeTransactionCommitAsync(GetHashCode()).ConfigureAwait(false);
            _logger.LogInformation("dispatched-domainEvents");
            var saveChangesResult = await SaveChangesAsync().ConfigureAwait(false);
            _logger.LogInformation("saveChanges-{saveChangesResultCount}", saveChangesResult);
        }

        private void CurrentTransaction_OnRollback(EfTransactionAdapter transactionAdapter)
        {
            _messageDispatcher.Clear(GetHashCode());
        }

        private void CurrentTransaction_OnDisposing(EfTransactionAdapter transactionAdapter)
        {
            _currentTransaction = null;
        }
    }
}