using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Messaging;
using Hitmu.Abstractions.Core.Messaging.Commands;
using Hitmu.Abstractions.Core.Messaging.Metadata;
using Hitmu.Abstractions.Core.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Interactions.Messaging
{
    public sealed class CommandIterator : ICommandIterator
    {
        private readonly IBindingService _bindingService;
        private readonly IRequestScope _requestScope;

        public CommandIterator(IRequestScope requestScope, IBindingService bindingService)
        {
            _requestScope = requestScope ?? throw new ArgumentNullException(nameof(requestScope));
            _bindingService = bindingService ?? throw new ArgumentNullException(nameof(bindingService));
        }

        public async Task<Result<TCommandResult>> ProcessAsync<TCommandResult>(ICommand<TCommandResult> command,
            CancellationToken cancellationToken) where TCommandResult : ICommandResult
        {
            var commandBinding = _bindingService.GetBindingFromMessage(command);
            if (commandBinding == null)
                return ErrorMessage.Error($"Command {nameof(command)} has no binding!");

            var commandHandler = _requestScope.GetService(commandBinding.HandlerType);
            return await ((Task<Result<TCommandResult>>)commandBinding.HandlerMethod.Invoke(commandHandler,
                new object[] { command, cancellationToken })).ConfigureAwait(false);
        }

        public async Task<Result<TCommandResult>> SendAsync<TCommandResult>(ICommand<TCommandResult> command)
            where TCommandResult : ICommandResult, new()
        {
            await _requestScope.GetService<IMessageCatcher>().Push(command).ConfigureAwait(false);
            return Result<TCommandResult>.From(new TCommandResult());
        }
    }
}