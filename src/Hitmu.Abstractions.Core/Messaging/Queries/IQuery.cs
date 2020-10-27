namespace Hitmu.Abstractions.Core.Messaging.Queries
{
    public interface IQuery<TQueryResult> : IMessage where TQueryResult : IQueryResult
    {
    }
}