using System.Security.Claims;
using System.Security.Principal;

namespace Ecommerce.Extention
{
    public static class IdentityExtensions
    {
        public static string GetAccountID(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("AccountId");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
