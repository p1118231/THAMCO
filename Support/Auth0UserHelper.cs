using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;


namespace THAMCOMVC.Support
{
    public static class Auth0UserHelper
    {
        /// <summary>
        /// Retrieves the Auth0 User ID from the authenticated user's claims.
        /// </summary>
        /// <param name="user">ClaimsPrincipal representing the authenticated user.</param>
        /// <returns>The Auth0 User ID if found, otherwise null.</returns>
        public static string? GetAuth0UserId(ClaimsPrincipal user)
        {
            // 'sub' is the claim type used for the Auth0 User ID
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Retrieves the email address from the authenticated user's claims.
        /// </summary>
        /// <param name="user">ClaimsPrincipal representing the authenticated user.</param>
        /// <returns>The email address if found, otherwise null.</returns>
        public static string? GetEmail(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}
