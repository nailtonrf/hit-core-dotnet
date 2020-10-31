using Hitmu.Abstractions.Core.Results;
using System;

namespace Hitmu.Abstractions.Context
{
    public interface IContainer : IDisposable
    {
        void Attach<T>(string key, T value) where T : class;
        Result<T> Retrieve<T>(string key) where T : class;
    }
}