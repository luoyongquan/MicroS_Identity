using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Core.Threading;

namespace Core.Authorization
{
    public static class AuthorizationHelperExtensions
    {
        public static async Task AuthorizeAsync(this IAuthorizationHelper authorizationHelper, IAuthorizeAttribute authorizeAttribute)
        {
            await authorizationHelper.AuthorizeAsync(new[] { authorizeAttribute });
        }

        public static void Authorize(this IAuthorizationHelper authorizationHelper, IEnumerable<IAuthorizeAttribute> authorizeAttributes)
        {
            AsyncHelper.RunSync(() => authorizationHelper.AuthorizeAsync(authorizeAttributes));
        }

        public static void Authorize(this IAuthorizationHelper authorizationHelper, IAuthorizeAttribute authorizeAttribute)
        {
            authorizationHelper.Authorize(new[] { authorizeAttribute });
        }

        public static void Authorize(this IAuthorizationHelper authorizationHelper, MethodInfo methodInfo, Type type)
        {
            AsyncHelper.RunSync(() => authorizationHelper.AuthorizeAsync(methodInfo, type));
        }
    }
}