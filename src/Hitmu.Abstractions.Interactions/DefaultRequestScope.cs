using Hitmu.Abstractions.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Hitmu.Abstractions.Interactions
{
    public sealed class DefaultRequestScope : IRequestScope
    {
        private IServiceScope? _scope;

        public object GetService(Type serviceType)
        {
            if (_scope == null) throw new InvalidOperationException("Service Scope is not defined!");
            return _scope.ServiceProvider.GetService(serviceType);
        }

        public T GetService<T>()
        {
            if (_scope == null) throw new InvalidOperationException("Service Scope is not defined!");
            return _scope.ServiceProvider.GetService<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            if (_scope == null) throw new InvalidOperationException("Service Scope is not defined!");
            return _scope.ServiceProvider.GetServices<T>();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_scope == null) throw new InvalidOperationException("Service Scope is not defined!");
            return _scope.ServiceProvider.GetServices(serviceType);
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }

        internal void SetNativeScope(IServiceScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }
}