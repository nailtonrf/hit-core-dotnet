using System.Security.Claims;

namespace Hitmu.Abstractions.Context
{
    public interface IIdentityContext
    {
        ClaimsPrincipal User { get; }
    }
}