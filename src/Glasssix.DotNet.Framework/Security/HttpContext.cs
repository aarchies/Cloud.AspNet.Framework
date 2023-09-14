using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace Glasssix.DotNet.Framework.Security
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _accessor;
        private static IServiceProvider _provider;
        public static Microsoft.AspNetCore.Http.HttpContext Instance => _accessor?.HttpContext;

        public static HttpRequest Request => Instance?.Request;

        public static IGlasssixBuilder AddHttpContext(this IGlasssixBuilder builder)
        {
            _provider = builder.Services.BuildServiceProvider();
            _accessor = _provider.GetRequiredService<IHttpContextAccessor>();
            return builder;
        }

        #region User(当前用户安全主体)

        /// <summary>
        /// 当前用户安全主体
        /// </summary>
        public static ClaimsPrincipal User
        {
            get
            {
                if (Instance == null)
                    return UnauthenticatedPrincipal.Instance;
                if (Instance.User is { } principal)
                    return principal;
                return UnauthenticatedPrincipal.Instance;
            }
        }

        #endregion User(当前用户安全主体)

        #region Identity(当前用户身份)

        /// <summary>
        /// 当前用户身份
        /// </summary>
        public static ClaimsIdentity Identity
        {
            get
            {
                if (User.Identity is ClaimsIdentity identity)
                    return identity;
                return UnauthenticatedIdentity.Instance;
            }
        }

        #endregion Identity(当前用户身份)
    }
}