using Hitmu.Abstractions.Core.Messaging.Commands;
using Hitmu.Abstractions.Core.Messaging.Events;
using Hitmu.Abstractions.Core.Messaging.Queries;
using Hitmu.Abstractions.Core.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Interactions
{
    public sealed class IteratorContext : ContainerContext, IIteratorContext
    {
        private readonly ICommandIterator _commandIterator;
        private readonly IEventIterator _eventIterator;
        private readonly IQueryIterator _queryIterator;

        public Guid Id { get; }
        public Guid CorrelationId { get; set; }

        public IteratorContext(ICommandIterator commandIterator, IEventIterator eventIterator, IQueryIterator queryIterator)
        {
            Id = Guid.NewGuid();
            CorrelationId = Id;

            _commandIterator = commandIterator ?? throw new ArgumentNullException(nameof(commandIterator));
            _eventIterator = eventIterator ?? throw new ArgumentNullException(nameof(eventIterator));
            _queryIterator = queryIterator ?? throw new ArgumentNullException(nameof(queryIterator)); ;
        }

        public Task<Result<TCommandResult>> ProcessAsync<TCommandResult>(ICommand<TCommandResult> command, CancellationToken cancellationToken) where TCommandResult : ICommandResult
        {
            return _commandIterator.ProcessAsync(command, cancellationToken);
        }

        public Task<Result<TCommandResult>> SendAsync<TCommandResult>(ICommand<TCommandResult> command) where TCommandResult : ICommandResult, new()
        {
            return _commandIterator.SendAsync(command);
        }

        public Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return _eventIterator.Publish(@event);
        }

        public Task<Result<TQueryResult>> RequestAsync<TQueryResult>(IQuery<TQueryResult> query, CancellationToken cancellationToken) where TQueryResult : IQueryResult
        {
            return _queryIterator.RequestAsync(query, cancellationToken);
        }
    }
}