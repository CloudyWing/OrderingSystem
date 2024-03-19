using System.Security.Claims;
using System.Security.Principal;

namespace CloudyWing.OrderingSystem.Web.Infrastructure.Extensions {
    public static class IdentityExtensions {
        public static string GetEmail(this IIdentity identity) {
            return (identity as ClaimsIdentity)?.FindFirst(x => x.Type == ClaimTypes.Email)?.Value ?? "";
        }
    }
}
