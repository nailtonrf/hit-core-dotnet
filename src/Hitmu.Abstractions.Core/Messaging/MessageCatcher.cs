using Hitmu.Abstractions.Core.Messaging.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging
{
    public sealed class MessageCatcher : IMessageCatcher
    {
        private readonly object _catcherLocker = new object();
        private readonly Dictionary<int, List<IMessage>> _messagesBeforeCommit = new Dictionary<int, List<IMessage>>();
        private readonly List<IMessage> _messagesOnSuccessRequest = new List<IMessage>();

        public IMessage[] AllMessagesToPublishBeforeTransactionCommit(int contextId)
        {
            lock (_catcherLocker)
            {
                var contextMessages = _messagesBeforeCommit.TryGetValue(contextId, out var messages)
                    ? messages.ToArray()
                    : new IMessage[0];
                _messagesBeforeCommit.Remove(contextId);
                return contextMessages;
            }
        }

        public IMessage[] AllMessagesToPublishOnSuccessRequest()
        {
            lock (_catcherLocker)
            {
                var successRequestMessages = _messagesOnSuccessRequest.ToArray();
                Clear();
                return successRequestMessages;
            }
        }

        public void Clear()
        {
            _messagesBeforeCommit.Clear();
            _messagesOnSuccessRequest.Clear();
        }

        public void Clear(int contextId)
        {
            if (_messagesBeforeCommit.ContainsKey(contextId)) _messagesBeforeCommit.Remove(contextId);
        }

        public Task Push(IMessage message)
        {
            _messagesOnSuccessRequest.Add(message);
            return Task.CompletedTask;
        }

        public Task Push<TMessage>(int contextId, TMessage message) where TMessage : IEvent
        {
            if (_messagesBeforeCommit.TryGetValue(contextId, out var messages))
                messages.Add(message);
            else
                _messagesBeforeCommit.Add(contextId, new List<IMessage> {message});
            _messagesOnSuccessRequest.Add(message);
            return Task.CompletedTask;
        }

        public Task PushAll(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages) Push(message);
            return Task.CompletedTask;
        }

        public Task PushAll<TMessage>(int contextId, IEnumerable<TMessage> messages) where TMessage : IEvent
        {
            foreach (var message in messages) Push(contextId, message);
            return Task.CompletedTask;
        }
    }
}