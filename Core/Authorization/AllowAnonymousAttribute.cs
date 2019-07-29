using System;

namespace Core.Authorization
{
    /// <summary>
    /// Used to allow a method to be accessed by any user.
    /// Suppress <see cref="AuthorizeAttribute"/> defined in the class containing that method.
    /// </summary>
    public class AllowAnonymousAttribute : Attribute, IAllowAnonymousAttribute
    {

    }
}