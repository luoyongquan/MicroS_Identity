using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Authorization
{
    public interface IAuthorizationHelper
    {
        Task AuthorizeAsync(IEnumerable<IAuthorizeAttribute> authorizeAttributes);

        Task AuthorizeAsync(MethodInfo methodInfo, Type type);
    }
}