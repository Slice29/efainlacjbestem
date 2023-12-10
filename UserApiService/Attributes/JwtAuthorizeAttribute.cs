using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
namespace UserApiService.Attributes
{
public class JwtAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private string _issuer;
    private string _audience;

    public JwtAuthorizeAttribute()
    {
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

        // Set issuer and audience from configuration
        _issuer = "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0";
        _audience = "f898818f-3412-4426-87c5-535f62c403b4";

        var request = context.HttpContext.Request;
        var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
          var token = authHeader[7..].Trim(); // Skip the first 7 characters ("Bearer ")
            Console.WriteLine("uite aici " + token);
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    var discoveryDocument = new ConfigurationManager<OpenIdConnectConfiguration>(
                        $"{_issuer}/.well-known/openid-configuration",
                        new OpenIdConnectConfigurationRetriever()
                    );
                    var openIdConfig = discoveryDocument.GetConfigurationAsync().GetAwaiter().GetResult();
                    return openIdConfig.SigningKeys;
                }
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (principal != null)
                {
                    context.HttpContext.User = principal;
                    return;
                }
            }
            catch (SecurityTokenValidationException)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
                return;
            }
        }

        context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
    }

    private static string ExtractGuidFromResourceId(string resourceId)
    {
        if (string.IsNullOrEmpty(resourceId))
        {
            throw new ArgumentException("Resource ID is null or empty", nameof(resourceId));
        }

        var parts = resourceId.Split('/');
        if (parts.Length < 3)
        {
            throw new ArgumentException("Invalid Resource ID format", nameof(resourceId));
        }

        return parts.Last();
    }
}

}
