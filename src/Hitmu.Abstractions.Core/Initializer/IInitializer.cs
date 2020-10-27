using Microsoft.Extensions.DependencyInjection;

namespace Hitmu.Abstractions.Core.Initializer
{
    /// <summary>
    ///     Initialize a feature in the application.
    /// </summary>
    public interface IInitializer
    {
        string Name { get; }
        void Initialize(IServiceCollection services);
    }
}