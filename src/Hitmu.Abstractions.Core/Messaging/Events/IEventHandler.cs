using System.Threading;
using System.Threading.Tasks;
using Hitmu.Abstractions.Core.Results;

namespace Hitmu.Abstractions.Core.Messaging.Events
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task<Result<bool>> HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}