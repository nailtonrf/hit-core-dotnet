using System;

namespace Hitmu.Abstractions.Context
{
    public interface IStartWithApplicationContext
    {
        void Start(IApplicationContext applicationContext, IServiceProvider serviceProvider);
    }
}