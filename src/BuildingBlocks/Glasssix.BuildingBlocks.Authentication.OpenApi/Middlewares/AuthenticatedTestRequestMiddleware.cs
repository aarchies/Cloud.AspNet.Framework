using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Middlewares
{
    public class AuthenticatedTestRequestMiddleware
    {
        public static readonly string TestAuthorizationHeader = "FakeAuthorization";
        private readonly RequestDelegate _next;

        public AuthenticatedTestRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains(TestAuthorizationHeader))
            {
                var token = context.Request.Headers[TestAuthorizationHeader].Single();
                var jwt = new JwtSecurityToken(token);
                var claimsIdentity = new ClaimsIdentity(jwt.Claims, JwtBearerDefaults.AuthenticationScheme, JwtClaimTypes.Name, JwtClaimTypes.Role);
                context.User = new ClaimsPrincipal(claimsIdentity);
            }

            await _next(context);
        }
    }
}