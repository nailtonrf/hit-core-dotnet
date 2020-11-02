using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Core.Results;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Hitmu.Abstractions.Interactions
{
    public class ContainerContext : IContainer
    {
        private readonly Lazy<IDictionary<string, object>> _bag = new Lazy<IDictionary<string, object>>(()
            => new ConcurrentDictionary<string, object>());

        public void Attach<T>(string key, T value) where T : class
            => _bag.Value.TryAdd(key, value);

        public Result<T> Retrieve<T>(string key) where T : class
        {
            return _bag.Value.TryGetValue(key, out var value)
                ? Result<T>.From((T)value)
                : ErrorMessage.Error($"Key {key} not found!");
        }

        public void Dispose()
        {
            if (_bag.IsValueCreated)
            {
                _bag.Value.Clear();
            }
        }
    }
}