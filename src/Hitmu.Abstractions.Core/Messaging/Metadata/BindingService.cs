using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Hitmu.Abstractions.Core.Messaging.Commands;
using Hitmu.Abstractions.Core.Messaging.Events;
using Hitmu.Abstractions.Core.Messaging.Queries;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    public sealed class BindingService : IBindingService
    {
        private readonly Dictionary<string, Binding> _bindingMetadata;

        public BindingService()
        {
            _bindingMetadata = new Dictionary<string, Binding>();
        }

        public bool HasBindingFromMessage(string messageName)
        {
            return messageName != null && _bindingMetadata.ContainsKey(messageName.ToLower());
        }

        public bool HasBindingFromMessage(IMessage message)
        {
            return HasBindingFromMessage(message.GetType().Name.ToLower());
        }

        public Binding? GetBindingFromMessage(IMessage message)
        {
            return GetBindingFromMessage(message.GetType().Name.ToLower());
        }

        public Binding? GetBindingFromMessage(string messageName)
        {
            return _bindingMetadata.TryGetValue(messageName.ToLower(), out var binding)
                ? binding
                : null;
        }

        public void LoadFromServices(IServiceCollection services)
        {
            LoadTypeFromServices(services, BindingType.Command, typeof(ICommandHandler<,>));
            LoadTypeFromServices(services, BindingType.Event, typeof(IEventHandler<>));
            LoadTypeFromServices(services, BindingType.Query, typeof(IQueryHandler<,>));
        }

        public IEnumerable<Binding> All() => _bindingMetadata.Values;

        private void LoadTypeFromServices(IServiceCollection services, BindingType bindingType, Type genericType)
        {
            var bindingsFromScan = services
                .Where(p => p.ServiceType.IsGenericType && p.ServiceType.GetGenericTypeDefinition() == genericType)
                .Select(p => new {BindingType = bindingType, p.ServiceType})
                .Distinct()
                .Select(p => BindingFactory.Create(bindingType, p.ServiceType, p.ServiceType.GenericTypeArguments));

            foreach (var binding in bindingsFromScan)
                _bindingMetadata.Add(binding.MessageTypeName, binding);
        }
    }
}