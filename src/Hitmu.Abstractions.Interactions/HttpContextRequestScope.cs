using Hitmu.Abstractions.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Hitmu.Abstractions.Interactions
{
    /// <summary>
    ///     For getting scope from HttpContext - Web Application.
    /// </summary>
    public sealed class HttpContextRequestScope : IRequestScope
    {
        private readonly IServiceProvider _serviceProvider;

        public HttpContextRequestScope(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            return _serviceProvider.GetServices<T>();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }

        public void Dispose()
        {
        }
    }
}