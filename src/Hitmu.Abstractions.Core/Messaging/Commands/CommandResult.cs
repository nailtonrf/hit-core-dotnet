using System;

namespace Hitmu.Abstractions.Core.Messaging.Commands
{
    public sealed class CommandResult : ICommandResult
    {
        public CommandResult()
        {
        }

        public CommandResult(Guid requestId)
        {
            RequestId = requestId;
        }

        public Guid RequestId { get; }
    }
}