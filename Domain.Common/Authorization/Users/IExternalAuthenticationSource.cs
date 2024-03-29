using System.Threading.Tasks;

namespace Domain.Authorization.Users
{
    /// <summary>
    /// Defines an external authorization source.
    /// </summary>
    /// <typeparam name="TTenant">Tenant type</typeparam>
    /// <typeparam name="TUser">User type</typeparam>
    public interface IExternalAuthenticationSource< TUser>
             where TUser : UserBase
    {
        /// <summary>
        /// Unique name of the authentication source.
        /// This source name is set to <see cref="UserBase.AuthenticationSource"/>
        /// if the user authenticated by this authentication source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Used to try authenticate a user by this source.
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <param name="plainPassword">Plain password of the user</param>
        /// <param name="tenant">Tenant of the user or null (if user is a host user)</param>
        /// <returns>True, indicates that this used has authenticated by this source</returns>
        Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword);

        /// <summary>
        /// This method is a user authenticated by this source which does not exists yet.
        /// So, source should create the User and fill properties.
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <param name="tenant">Tenant of the user or null (if user is a host user)</param>
        /// <returns>Newly created user</returns>
        Task<TUser> CreateUserAsync(string userNameOrEmailAddress);

        /// <summary>
        /// This method is called after an existing user is authenticated by this source.
        /// It can be used to update some properties of the user by the source.
        /// </summary>
        /// <param name="user">The user that can be updated</param>
        /// <param name="tenant">Tenant of the user or null (if user is a host user)</param>
        Task UpdateUserAsync(TUser user);
    }
}