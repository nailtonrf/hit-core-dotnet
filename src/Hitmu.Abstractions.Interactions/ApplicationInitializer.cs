using Hitmu.Abstractions.Core.Initializer;
using Hitmu.Abstractions.Core.Messaging;
using Hitmu.Abstractions.Core.Messaging.Commands;
using Hitmu.Abstractions.Core.Messaging.Events;
using Hitmu.Abstractions.Core.Messaging.Metadata;
using Hitmu.Abstractions.Core.Messaging.Queries;
using Hitmu.Abstractions.Interactions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Hitmu.Abstractions.Interactions
{
    public sealed class ApplicationInitializer : IInitializer
    {
        public string Name => "application-initializer";
        public void Initialize(IServiceCollection services)
        {
            services
                .AddSingleton<IBindingService, BindingService>()
                .AddScoped<IMessageCatcher, MessageCatcher>()
                .AddScoped<IMessageDispatcher, MessageDispatcher>()
                .AddScoped<ICommandIterator, CommandIterator>()
                .AddScoped<IEventIterator, EventIterator>()
                .AddScoped<IQueryIterator, QueryIterator>();

            services.AddScoped<IIteratorContext, IteratorContext>();
        }
    }
}