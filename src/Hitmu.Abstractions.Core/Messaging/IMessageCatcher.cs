using Hitmu.Abstractions.Core.Messaging.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging
{
    public interface IMessageCatcher
    {
        Task Push(IMessage message);
        Task Push<TMessage>(int contextId, TMessage message) where TMessage : IEvent;

        Task PushAll(IEnumerable<IMessage> messages);
        Task PushAll<TMessage>(int contextId, IEnumerable<TMessage> messages) where TMessage : IEvent;

        IMessage[] AllMessagesToPublishBeforeTransactionCommit(int contextId);
        IMessage[] AllMessagesToPublishOnSuccessRequest();

        void Clear();
        void Clear(int contextId);
    }
}