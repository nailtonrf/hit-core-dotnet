using System;
using System.Collections.Generic;

namespace Hitmu.Abstractions.Context
{
    public interface IRequestScope : IDisposable
    {
        object GetService(Type serviceType);
        T GetService<T>();
        IEnumerable<T> GetServices<T>();
        IEnumerable<object> GetServices(Type serviceType);
    }
}