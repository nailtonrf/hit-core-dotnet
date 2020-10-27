using Hitmu.Abstractions.Core.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging.Commands
{
    public interface ICommandHandler<in TCommand, TCommandResult>
        where TCommand : ICommand<TCommandResult>
        where TCommandResult : ICommandResult
    {
        Task<Result<TCommandResult>> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}