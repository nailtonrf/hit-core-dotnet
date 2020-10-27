using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    public interface IBindingService
    {
        Binding? GetBindingFromMessage(IMessage message);
        Binding? GetBindingFromMessage(string messageName);
        bool HasBindingFromMessage(string messageName);
        bool HasBindingFromMessage(IMessage message);
        void LoadFromServices(IServiceCollection injector);
        IEnumerable<Binding> All();
    }
}