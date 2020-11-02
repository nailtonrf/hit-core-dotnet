using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Messaging.Metadata;
using Hitmu.Abstractions.Core.Messaging.Queries;
using Hitmu.Abstractions.Core.Resilience;
using Hitmu.Abstractions.Core.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Interactions.Messaging
{
    public sealed class QueryIterator : IQueryIterator
    {
        private readonly IBindingService _bindingService;
        private readonly IRequestScope _requestScope;

        public QueryIterator(IRequestScope requestScope, IBindingService bindingService)
        {
            _requestScope = requestScope;
            _bindingService = bindingService;
        }

        public async Task<Result<TQueryResult>> RequestAsync<TQueryResult>(IQuery<TQueryResult> query,
            CancellationToken cancellationToken)
            where TQueryResult : IQueryResult
        {
            try
            {
                var queryBinding = _bindingService.GetBindingFromMessage(query);
                if (queryBinding == null)
                    return ErrorMessage.Error($"Query {nameof(query)} has no binding!", ResilienceErrorType.Permanent);

                var queryHandler = _requestScope.GetService(queryBinding.HandlerType);
                return await (Task<Result<TQueryResult>>)queryBinding.HandlerMethod.Invoke(queryHandler,
                    new object[] { query, cancellationToken });
            }
            catch (Exception e)
            {
                return ErrorMessage.Error(e.Message, ResilienceErrorType.Permanent);
            }
        }
    }
}