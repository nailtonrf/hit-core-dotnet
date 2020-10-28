using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging.Events
{
    public interface IEventIterator
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}