using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace QuestionApp.Extensions
{
    public static class ClaimsExtensions
    {
        public static Claim FindClaim(this IPrincipal user, string claimType)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(claimType)) throw new ArgumentNullException(nameof(claimType));

            var claimsPrincipal = user as ClaimsPrincipal;
            return claimsPrincipal?.FindFirst(claimType);
        }

        public static int? GetUserId(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(value)) return default(int?);

            int result;
            return int.TryParse(value, out result) ? result : default(int?);
        }

        public static string GetUsername(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(value)) return default(string);

            return value;

        }
    }
}
