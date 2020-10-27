namespace Hitmu.Abstractions.Core.Messaging.Commands
{
    public interface ICommand<TCommandResult> : IMessage
        where TCommandResult : ICommandResult
    {
    }

    public interface ICommand : ICommand<CommandResult>
    {
    }
}