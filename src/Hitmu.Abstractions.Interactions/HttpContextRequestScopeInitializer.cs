using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Initializer;
using Microsoft.Extensions.DependencyInjection;

namespace Hitmu.Abstractions.Interactions
{
    public sealed class HttpContextRequestScopeInitializer : IInitializer
    {
        public string Name => "requestscope-httpcontext";

        public void Initialize(IServiceCollection services)
        {
            services.AddTransient<IRequestScope, HttpContextRequestScope>();
        }
    }
}