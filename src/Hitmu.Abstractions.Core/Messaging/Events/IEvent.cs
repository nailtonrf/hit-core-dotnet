using System;

namespace Hitmu.Abstractions.Core.Messaging.Events
{
    public interface IEvent : IMessage
    {
        Guid Id { get; }
        string SourceId { get; }
        DateTime CreationDate { get; }
    }
}