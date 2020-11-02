using Hitmu.Abstractions.Core.Initializer;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Hitmu.Abstractions.Context
{
    public interface IApplicationContext : IDisposable
    {
        Guid ApplicationId { get; }
        string ApplicationName { get; }
        IConfiguration Configuration { get; }

        IApplicationContext Load(IInitializer module);
        IApplicationContext LoadAllFromAssembly(Assembly assembly);
        void InitializeMediators();

        IRequestScope BeginScope();

        void Start();
        void StartWithProvider(IServiceProvider serviceProvider);
    }
}