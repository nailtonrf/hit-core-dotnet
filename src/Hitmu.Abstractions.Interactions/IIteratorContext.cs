using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Messaging.Commands;
using Hitmu.Abstractions.Core.Messaging.Events;
using Hitmu.Abstractions.Core.Messaging.Queries;
using System;

namespace Hitmu.Abstractions.Interactions
{
    public interface IIteratorContext : ICommandIterator, IEventIterator, IQueryIterator, IContainer
    {
        Guid Id { get; }
        Guid CorrelationId { get; set; }
    }
}