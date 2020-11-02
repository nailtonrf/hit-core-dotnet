using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Initializer;
using Hitmu.Abstractions.Core.Messaging.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hitmu.Abstractions.Interactions
{
    public sealed class ApplicationContext : IApplicationContext
    {
        private static readonly object Locker = new object();
        private static ApplicationContext? _currentContext;
        private readonly List<IInitializer> _components;
        private readonly List<Assembly> _componentsAssemblies;

        private readonly IServiceCollection _serviceCollection;

        private IServiceProvider? _serviceProvider;

        internal ApplicationContext(string applicationName, IServiceCollection serviceCollection)
        {
            ApplicationName = applicationName ?? throw new ArgumentNullException(nameof(applicationName));
            _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            ApplicationId = Guid.NewGuid();
            _components = new List<IInitializer>();
            _componentsAssemblies = new List<Assembly>();
            Configuration = new ConfigurationBuilder().Build();
        }

        internal ApplicationContext(string applicationName, IConfiguration configuration, IServiceCollection serviceCollection)
            : this(applicationName, serviceCollection)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public static ApplicationContext Instance
        {
            get
            {
                lock (Locker)
                {
                    if (_currentContext == null)
                        throw new InvalidOperationException("ApplicationContext was not initialized!");
                    return _currentContext;
                }
            }
        }

        public Guid ApplicationId { get; }
        public string ApplicationName { get; }
        public IConfiguration Configuration { get; }

        public IApplicationContext Load(IInitializer module)
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            module.Initialize(_serviceCollection);
            _components.Add(module);

            var assemblyComponent = module.GetType().Assembly;
            AddAssemblyToLoad(assemblyComponent);
            return this;
        }

        public void InitializeMediators()
        {
            InitializeModuleMediators();
        }

        public IRequestScope BeginScope()
        {
            var nativeScope = _serviceProvider.CreateScope();
            var requestScope = nativeScope.ServiceProvider.GetService<IRequestScope>();
            if (requestScope is DefaultRequestScope defaultRequestScope)
                defaultRequestScope.SetNativeScope(nativeScope);
            return requestScope;
        }

        public void Start()
        {
            StartWithProvider(_serviceCollection.BuildServiceProvider(true));
        }

        public void StartWithProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.GetService<IBindingService>().LoadFromServices(_serviceCollection);

            InitializeModulesWithStarterContract();
        }

        public void Dispose()
        {
            FinalizeModulesWithFinalizerContract();
            _currentContext = null;
        }

        private void AddAssemblyToLoad(Assembly assemblyToLoad)
        {
            if (!_componentsAssemblies.Contains(assemblyToLoad)) _componentsAssemblies.Add(assemblyToLoad);
        }

        private void InitializeModuleMediators()
        {
            //_serviceCollection.InitializeDecorator(typeof(ICommandHandler<,>), typeof(CommandHandlerDecorator<,>));
            //_serviceCollection.InitializeDecorator(typeof(IIntegrationEventHandler<>), typeof(IntegrationEventHandlerDecorator<>));
        }

        private void InitializeModulesWithStarterContract()
        {
            if (_serviceProvider == null) return;
            _components.ForEach(module => { (module as IStartWithApplicationContext)?.Start(this, _serviceProvider); });
        }

        private void FinalizeModulesWithFinalizerContract()
        {
            _components.ForEach(module => { (module as IStopWithApplicationContext)?.Stop(); });
        }

        public static ApplicationContext CreateContext(string applicationName, IServiceCollection services)
        {
            lock (Locker)
            {
                if (services == null) throw new ArgumentNullException(nameof(services));
                _currentContext = new ApplicationContext(applicationName, services);
                return _currentContext;
            }
        }

        public static ApplicationContext CreateContext(string applicationName, IConfiguration configuration,
            IServiceCollection services)
        {
            lock (Locker)
            {
                if (configuration == null) throw new ArgumentNullException(nameof(configuration));
                if (services == null) throw new ArgumentNullException(nameof(services));
                _currentContext = new ApplicationContext(applicationName, configuration, services);
                return _currentContext;
            }
        }
    }
}