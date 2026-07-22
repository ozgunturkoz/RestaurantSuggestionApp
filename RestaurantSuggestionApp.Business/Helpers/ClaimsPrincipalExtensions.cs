using System.Security.Claims;

namespace RestaurantSuggestionApp.Business.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// JWT Token içerisindeki NameIdentifier veya sub claim'ini okuyarak kullanıcı ID'sini int olarak döndürür.
        /// Token yoksa veya geçerli bir ID bulunamazsa null döndürür.
        /// </summary>
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            if (user == null) return null;

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            return null;
        }

        /// <summary>
        /// JWT Token içerisindeki Name veya name claim'ini okur.
        /// </summary>
        public static string? GetUserName(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Name)?.Value ?? user?.FindFirst("name")?.Value;
        }

        /// <summary>
        /// JWT Token içerisindeki Email veya email claim'ini okur.
        /// </summary>
        public static string? GetUserEmail(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Email)?.Value ?? user?.FindFirst("email")?.Value;
        }
    }
}
