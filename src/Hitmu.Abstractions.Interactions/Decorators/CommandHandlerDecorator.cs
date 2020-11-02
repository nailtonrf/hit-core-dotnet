using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Messaging;
using Hitmu.Abstractions.Core.Messaging.Commands;
using Hitmu.Abstractions.Core.Resilience;
using Hitmu.Abstractions.Core.Results;
using Hitmu.Abstractions.Store;
using Hitmu.Abstractions.Store.Contexts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Interactions.Decorators
{
    /// <summary>
    ///     Command Decorator.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TCommandResult"></typeparam>
    public sealed class CommandHandlerDecorator<TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
        where TCommand : ICommand<TCommandResult>
        where TCommandResult : ICommandResult
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly ConcurrentDictionary<string, TransactionalContextAttribute> CommandHandlersDataContext =
            new ConcurrentDictionary<string, TransactionalContextAttribute>();

        private readonly TransactionalContextAttribute? _contextAttribute;

        private readonly ICommandHandler<TCommand, TCommandResult> _decoratedHandler;
        private readonly ILogger _logger;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IRequestScope _requestScope;

        public CommandHandlerDecorator(ICommandHandler<TCommand, TCommandResult> decoratedHandler,
            ILoggerFactory loggerFactory, IMessageDispatcher messageDispatcher, IRequestScope requestScope)
        {
            _decoratedHandler = decoratedHandler;
            _messageDispatcher = messageDispatcher;
            _requestScope = requestScope;
            _logger = loggerFactory.CreateLogger(nameof(TCommand));

            _contextAttribute = CommandHandlersDataContext.GetOrAdd(
                decoratedHandler.ToString(),
                key =>
                {
                    if (decoratedHandler.GetType().GetCustomAttribute(typeof(TransactionalContextAttribute)) is TransactionalContextAttribute attribute) return attribute;
                    return null!;
                });
        }


        public async Task<Result<TCommandResult>> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (_contextAttribute == null)
                    return await _decoratedHandler
                        .HandleAsync(command, cancellationToken)
                        .DoAsync(_ => _messageDispatcher.DispatchOnSuccessRequestAsync());

                if (!(_requestScope.GetService(_contextAttribute.ContextType) is ITransactionalDataContext transactionalDataContext))
                    return ErrorMessage.Error($"Context {_contextAttribute.ContextType.Name} is not transactional.");


                using var transaction = transactionalDataContext.BeginTransaction(_contextAttribute.IsolationLevel);
                {
                    _logger.LogInformation("{contextName}-begin transaction", _contextAttribute.ToString());
                    return await _decoratedHandler
                        .HandleAsync(command, cancellationToken)
                        .DoAsync(_ => transaction.CommitAsync())
                        .DoAsync(_ => _messageDispatcher.DispatchOnSuccessRequestAsync())
                        .ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{commanddecorator}", nameof(TCommand));
                return ErrorMessage.Error(e.Message, ResilienceErrorType.Transient);
            }
        }
    }
}