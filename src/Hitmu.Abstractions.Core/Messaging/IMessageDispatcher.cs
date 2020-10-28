using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Messaging
{
    public interface IMessageDispatcher
    {
        Task DispatchBeforeTransactionCommitAsync(int contextId);
        Task DispatchBeforeTransactionCommitAsync(IMessage[] messages);
        Task DispatchOnSuccessRequestAsync();
        Task Clear();
        Task Clear(int contextId);
    }
}