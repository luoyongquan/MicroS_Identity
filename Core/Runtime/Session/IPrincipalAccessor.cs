using System.Security.Claims;

namespace Core.Runtime.Session
{
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}
