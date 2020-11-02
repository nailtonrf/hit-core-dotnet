using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Messaging;
using Hitmu.Abstractions.Core.Messaging.Events;
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
    public sealed class EventHandlerDecorator<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly ConcurrentDictionary<string, TransactionalContextAttribute> EventHandlersDataContext =
            new ConcurrentDictionary<string, TransactionalContextAttribute>();

        private readonly TransactionalContextAttribute? _contextAttribute;

        private readonly IEventHandler<TEvent> _decoratedHandler;
        private readonly ILogger _logger;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IRequestScope _requestScope;

        public EventHandlerDecorator(IEventHandler<TEvent> decoratedHandler, ILoggerFactory logger,
            IMessageDispatcher messageDispatcher, IRequestScope requestScope)
        {
            _decoratedHandler = decoratedHandler;
            _logger = logger.CreateLogger(nameof(TEvent));
            _messageDispatcher = messageDispatcher;
            _requestScope = requestScope;

            _contextAttribute = EventHandlersDataContext.GetOrAdd(
                decoratedHandler.ToString(),
                key =>
                {
                    if (decoratedHandler.GetType().GetCustomAttribute(typeof(TransactionalContextAttribute)) is TransactionalContextAttribute attribute)
                        return attribute;
                    return null!;
                });
        }

        public async Task<Result<bool>> HandleAsync(TEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                if (_contextAttribute == null)
                {
                    return await _decoratedHandler
                        .HandleAsync(@event, cancellationToken)
                        .DoAsync(_ => _messageDispatcher.DispatchOnSuccessRequestAsync());
                }

                if (!(_requestScope.GetService(_contextAttribute.ContextType) is ITransactionalDataContext transactionalDataContext))
                    return ErrorMessage.Error($"Context {_contextAttribute.ContextType.Name} is not transactional.");

                using var transaction = transactionalDataContext.BeginTransaction(_contextAttribute.IsolationLevel);
                return await _decoratedHandler
                    .HandleAsync(@event, cancellationToken)
                    .DoAsync(_ => transaction.CommitAsync())
                    .DoAsync(_ => _messageDispatcher.DispatchOnSuccessRequestAsync())
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{eventdecorator}", nameof(TEvent));
                return ErrorMessage.Error(e.Message, ResilienceErrorType.Transient);
            }
        }
    }
}