using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging
{
    public interface IMessageProducer
    {
        Task Produce(IMessage message);
        Task ProduceAll(params IMessage[] messages);
    }
}