using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Messaging;
using Hitmu.Abstractions.Core.Messaging.Metadata;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Interactions.Messaging
{
    public sealed class MessageDispatcher : IMessageDispatcher
    {
        private readonly IBindingService _bindingService;
        private readonly IMessageCatcher _messageCatcher;
        private readonly IRequestScope _requestScope;

        public MessageDispatcher(IRequestScope requestScope, IMessageCatcher messageCatcher,
            IBindingService bindingService)
        {
            _requestScope = requestScope;
            _messageCatcher = messageCatcher;
            _bindingService = bindingService;
        }

        public async Task DispatchBeforeTransactionCommitAsync(IMessage[] messages)
        {
            foreach (var message in messages)
            {
                var eventBinding = _bindingService.GetBindingFromMessage(message);
                await DispatchInternalEvent(eventBinding, message).ConfigureAwait(false);
            }
        }

        public async Task DispatchBeforeTransactionCommitAsync(int contextId)
        {
            var allMessages = _messageCatcher.AllMessagesToPublishBeforeTransactionCommit(contextId);
            foreach (var message in allMessages)
            {
                var eventBinding = _bindingService.GetBindingFromMessage(message);
                await DispatchInternalEvent(eventBinding, message).ConfigureAwait(false);
            }
        }

        public async Task DispatchOnSuccessRequestAsync()
        {
            var allMessages = _messageCatcher.AllMessagesToPublishOnSuccessRequest();
            if (!allMessages.Any()) return;

            foreach (var messageProducer in _requestScope.GetServices<IMessageProducer>())
                await messageProducer.ProduceAll(allMessages.ToArray()).ConfigureAwait(false);
        }

        public Task Clear()
        {
            _messageCatcher.Clear();
            return Task.CompletedTask;
        }

        public Task Clear(int contextId)
        {
            _messageCatcher.Clear(contextId);
            return Task.CompletedTask;
        }

        private async Task DispatchInternalEvent(Binding? binding, IMessage message)
        {
            if (binding == null || binding.Type != BindingType.Event) return;
            foreach (var handler in _requestScope.GetServices(binding.HandlerType))
                await ((Task) binding.HandlerMethod.Invoke(handler, new object[] {message, new CancellationToken()}))
                    .ConfigureAwait(false);
        }
    }
}