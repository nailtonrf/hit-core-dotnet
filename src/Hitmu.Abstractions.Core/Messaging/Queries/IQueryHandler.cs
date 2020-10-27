using Hitmu.Abstractions.Core.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging.Queries
{
    public interface IQueryHandler<in TQuery, TQueryResult>
        where TQuery : IQuery<TQueryResult>
        where TQueryResult : IQueryResult
    {
        Task<Result<TQueryResult>> HandleAsync(TQuery query, CancellationToken cancellationToken);
    }
}