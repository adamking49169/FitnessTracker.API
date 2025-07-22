using System.Security.Claims;

namespace FitnessTracker.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public static Guid? GetUserId(this ClaimsPrincipal user, bool allowDevFallback = false)
    {
        var claim = user.FindFirst("oid");
        if (claim != null && Guid.TryParse(claim.Value, out var guid))
        {
            return guid;
        }
        return allowDevFallback ? DevUserId : null;
    }
}
