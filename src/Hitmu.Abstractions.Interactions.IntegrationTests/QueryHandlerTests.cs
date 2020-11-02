using FluentAssertions;
using Hitmu.Abstractions.Core.Messaging.Queries;
using Hitmu.Abstractions.Core.Results;
using Hitmu.Abstractions.Interactions.IntegrationTests.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hitmu.Abstractions.Interactions.IntegrationTests
{
    public class QueryHandlerTests
    {
        [Fact]
        public void Should_Run_QueryHandler()
        {
            using var applicationContext = ApplicationContextExtensions.CreateApplicationContext();
            applicationContext.LoadAllFromAssembly(GetType().Assembly);
            applicationContext.InitializeMediators();
            applicationContext.Start();
            using (var scope = applicationContext.BeginScope())
            {
                var queryMediator = scope.GetService<IIteratorContext>();
                var result = queryMediator.RequestAsync(new QueryRequest(), new CancellationToken()).GetAwaiter()
                    .GetResult();
                result.Should().NotBeNull();
                result.IsValid.Should().BeTrue();
                result.Value.Name.Should().Be("query_result");
            }
        }
    }

    #region Setup

    public class QueryResult : IQueryResult
    {
        public string Name { get; set; }
    }

    public class QueryRequest : IQuery<QueryResult>
    {
        public int Id { get; set; }
    }

    public class QueryHandler : IQueryHandler<QueryRequest, QueryResult>
    {
        public Task<Result<QueryResult>> HandleAsync(QueryRequest query, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result<QueryResult>.From(new QueryResult { Name = "query_result" }));
        }
    }

    #endregion
}