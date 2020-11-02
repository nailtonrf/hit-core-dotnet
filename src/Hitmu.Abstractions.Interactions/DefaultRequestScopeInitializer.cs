using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Initializer;
using Microsoft.Extensions.DependencyInjection;

namespace Hitmu.Abstractions.Interactions
{
    public sealed class DefaultRequestScopeInitializer : IInitializer
    {
        public string Name => "requestscope-selfmanaged";

        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IRequestScope, DefaultRequestScope>();
        }
    }
}