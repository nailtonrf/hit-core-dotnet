using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging.Events
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}