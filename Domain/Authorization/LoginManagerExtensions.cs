using Domain.Authorization.Roles;
using Domain.Authorization.Users;
using Core.Threading;

namespace Domain.Authorization
{
    public static class LogInManagerExtensions
    {
        public static LoginResult<TUser> Login<TRole, TUser>(
            this LogInManager<TRole, TUser> logInManager,
            string userNameOrEmailAddress,
            string plainPassword,
            bool shouldLockout = true)
                          where TRole : Role<TUser>, new()
                where TUser : User<TUser>
        {
            return AsyncHelper.RunSync(
                () => logInManager.LoginAsync(
                    userNameOrEmailAddress,
                    plainPassword,
                    shouldLockout
                )
            );
        }
    }
}
