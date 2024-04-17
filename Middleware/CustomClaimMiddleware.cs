using System.Security.Claims;

namespace TodoListService.Middleware;

public class CustomClaimMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        // Retrieve the current user's identity

        if (context.User.Identity is ClaimsIdentity userIdentity)
        {
            // Find the extension_loyaltyId claim
            var roleClaims = userIdentity.FindFirst("roles");

            if (roleClaims != null)
            {
                var roleValue = roleClaims.Value;
                foreach (var value in roleValue.Split(','))
                {
                    // Create a new claim with the same value as roleClaims but with ClaimTypes.Role type
                    var roleClaim = new Claim(ClaimTypes.Role, value);
                    // Add the role claim to the user's identity
                    userIdentity.AddClaim(roleClaim);
                }
                userIdentity.RemoveClaim(roleClaims);
            }
        }

        // Call the next middleware in the pipeline
        await next(context);
    }
}