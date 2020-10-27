using Hitmu.Abstractions.Core.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging.Commands
{
    public interface ICommandMediator
    {
        /// <summary>
        ///     Process a command in the current service.
        /// </summary>
        /// <typeparam name="TCommandResult"></typeparam>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<TCommandResult>> ProcessAsync<TCommandResult>(ICommand<TCommandResult> command,CancellationToken cancellationToken)
            where TCommandResult : ICommandResult;

        /// <summary>
        ///     Send the command to other service.
        /// </summary>
        /// <typeparam name="TCommandResult"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<Result<TCommandResult>> SendAsync<TCommandResult>(ICommand<TCommandResult> command)
            where TCommandResult : ICommandResult, new();
    }
}