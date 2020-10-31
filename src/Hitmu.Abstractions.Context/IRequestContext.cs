using System;

namespace Hitmu.Abstractions.Context
{
    public interface IRequestContext : IContainer
    {
        Guid Id { get; }
        Guid CorrelationId { get; }
    }
}