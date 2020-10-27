using Hitmu.Abstractions.Core.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging.Queries
{
    public interface IQueryMediator
    {
        Task<Result<TQueryResult>> RequestAsync<TQueryResult>(IQuery<TQueryResult> query,
            CancellationToken cancellationToken) where TQueryResult : IQueryResult;
    }
}