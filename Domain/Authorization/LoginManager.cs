using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Authorization.Roles;
using Domain.Authorization.Users;
using Core.Auditing;
using Core.Dependency;
using Core.Extensions;
using Core.Domain.Repositories;
//using Domain.Common.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Domain.Authorization
{
    public abstract class LogInManager<TRole, TUser> : ITransientDependency
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>
    {
        protected UserManager<TRole, TUser> UserManager { get; }
        private readonly IRepositoryContext _repositoryContext;
        protected IRepository<UserLoginAttempt, long> UserLoginAttemptRepository { get; }
        protected RoleManager<TRole, TUser> RoleManager { get; }
        public IClientInfoProvider ClientInfoProvider { get; set; }
        protected IUserManagementConfig UserManagementConfig { get; }
        protected LogInManager(
            UserManager<TRole, TUser> userManager, IUserManagementConfig userManagementConfig,

            IRepository<UserLoginAttempt, long> userLoginAttemptRepository, IRepositoryContext repositoryContext,

            RoleManager<TRole, TUser> roleManager)
        {

            UserLoginAttemptRepository = userLoginAttemptRepository;
            _repositoryContext = repositoryContext;
            RoleManager = roleManager;
            UserManager = userManager;
            UserManagementConfig = userManagementConfig;
            ClientInfoProvider = NullClientInfoProvider.Instance;
        }


        public virtual async Task<LoginResult<TUser>> LoginAsync(UserLoginInfo login)
        {
            var result = await LoginAsyncInternal(login);
            await SaveLoginAttempt(result, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }

        protected virtual async Task<LoginResult<TUser>> LoginAsyncInternal(UserLoginInfo login)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }
            var user = await UserManager.Store.FindAsync(login);
            if (user == null)
            {
                return new LoginResult<TUser>(LoginResultType.UnknownExternalLogin);
            }

            return await CreateLoginResultAsync(user);
        }

        public virtual async Task<LoginResult<TUser>> LoginAsync(string userNameOrEmailAddress, string plainPassword, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, shouldLockout);
            await SaveLoginAttempt(result, userNameOrEmailAddress);
            return result;
        }

        protected virtual async Task<LoginResult<TUser>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            var loggedInFromExternalSource = await TryLoginFromExternalAuthenticationSources(userNameOrEmailAddress, plainPassword);

            var user = await UserManager.Store.FindByNameOrEmailAsync(userNameOrEmailAddress);
            if (user == null)
            {
                return new LoginResult<TUser>(LoginResultType.InvalidUserNameOrEmailAddress);
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return new LoginResult<TUser>(LoginResultType.LockedOut, user);
            }
            if (!loggedInFromExternalSource)
            {
                var verificationResult = UserManager.PasswordHasher.VerifyHashedPassword(user.Password, plainPassword);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return await GetFailedPasswordValidationAsLoginResultAsync(user, shouldLockout);
                }
                if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return await GetSuccessRehashNeededAsLoginResultAsync(user);
                }
                await UserManager.ResetAccessFailedCountAsync(user.Id);
            }
            return await CreateLoginResultAsync(user);
        }

        protected virtual async Task<LoginResult<TUser>> GetFailedPasswordValidationAsLoginResultAsync(TUser user, bool shouldLockout = false)
        {
            if (shouldLockout)
            {
                if (await TryLockOutAsync(user.Id))
                {
                    return new LoginResult<TUser>(LoginResultType.LockedOut, user);
                }
            }

            return new LoginResult<TUser>(LoginResultType.InvalidPassword, user);
        }

        protected virtual async Task<LoginResult<TUser>> GetSuccessRehashNeededAsLoginResultAsync(TUser user, bool shouldLockout = false)
        {
            return await GetFailedPasswordValidationAsLoginResultAsync(user, shouldLockout);
        }

        protected virtual async Task<LoginResult<TUser>> CreateLoginResultAsync(TUser user)
        {
            if (!user.IsActive)
            {
                return new LoginResult<TUser>(LoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync() && !user.IsEmailConfirmed)
            {
                return new LoginResult<TUser>(LoginResultType.UserEmailIsNotConfirmed);
            }

            user.LastLoginTime = DateTime.Now;

            await UserManager.Store.UpdateAsync(user);

            // await UnitOfWorkManager.Current.SaveChangesAsync();
            _repositoryContext.Commit();
            return new LoginResult<TUser>(
                user,
                await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)
            );
        }

        protected virtual async Task SaveLoginAttempt(LoginResult<TUser> loginResult, string userNameOrEmailAddress)
        {
            var loginAttempt = new UserLoginAttempt
            {
                UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                UserNameOrEmailAddress = userNameOrEmailAddress,

                Result = loginResult.Result,

                BrowserInfo = ClientInfoProvider.BrowserInfo,
                ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                ClientName = ClientInfoProvider.ComputerName,
            };

            await UserLoginAttemptRepository.InsertAsync(loginAttempt);
            _repositoryContext.Commit();
            //await UnitOfWorkManager.Current.SaveChangesAsync();

            //await uow.CompleteAsync();
        }

        protected virtual async Task<bool> TryLockOutAsync(long userId)
        {
            (await UserManager.AccessFailedAsync(userId)).CheckErrors();

            var isLockOut = await UserManager.IsLockedOutAsync(userId);
            _repositoryContext.Commit();

            return isLockOut;
        }

        protected virtual async Task<bool> TryLoginFromExternalAuthenticationSources(string userNameOrEmailAddress, string plainPassword)
        {
            if (!UserManagementConfig.ExternalAuthenticationSources.Any())
            {
                return false;
            }

            foreach (var sourceType in UserManagementConfig.ExternalAuthenticationSources)
            {
                var source = IocManager.Instance.Resolve<IExternalAuthenticationSource<TUser>>(sourceType);
                if (await source.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword))
                {
                    var user = await UserManager.Store.FindByNameOrEmailAsync(userNameOrEmailAddress);
                    if (user == null)
                    {
                        user = await source.CreateUserAsync(userNameOrEmailAddress);
                        user.AuthenticationSource = source.Name;
                        user.Password = UserManager.PasswordHasher.HashPassword(Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used

                        if (user.Roles == null)
                        {
                            user.Roles = new List<UserRole>();
                            foreach (var defaultRole in RoleManager.Roles.Where(r => r.IsDefault).ToList())
                            {
                                user.Roles.Add(new UserRole(user.Id, defaultRole.Id));
                            }
                        }

                        await UserManager.Store.CreateAsync(user);
                    }
                    else
                    {
                        await source.UpdateUserAsync(user);

                        user.AuthenticationSource = source.Name;

                        await UserManager.Store.UpdateAsync(user);
                    }

                    _repositoryContext.Commit();

                    return true;
                }
            }


            return false;
        }


        protected virtual async Task<bool> IsEmailConfirmationRequiredForLoginAsync()
        {
            return false;
        }
    }
}
