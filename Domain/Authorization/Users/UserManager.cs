using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Authorization.Roles;
using Domain.Organizations;
using Core.Runtime.Security;
using Core.Runtime.Session;
using Domain;
using Microsoft.AspNetCore.Identity;
using Core.Dependency;
using Core;
using Core.Domain.Repositories;
using Domain.Authorization.Users;

namespace Domain.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="UserManager{TUser,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    public abstract class CoreUserManager<TRole, TUser>
        : UserManager<TUser>,
         ITransientDependency
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>
    {

        public ISession Session { get; set; }
        protected CoreRoleManager<TRole, TUser> RoleManager { get; }

        public UserStore<TRole, TUser> Store { get; }
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;

        protected CoreUserManager(
            UserStore<TRole, TUser> userStore,
            CoreRoleManager<TRole, TUser> roleManager,
             IUserTokenProviderAccessor userTokenProviderAccessor,
             IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository
         )
            : base(userStore)
        {
            Store = userStore;
            RoleManager = roleManager;
            Session = NullSession.Instance;
            UserLockoutEnabledByDefault = true;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;
            UserTokenProvider = userTokenProviderAccessor.GetUserTokenProviderOrNull<TUser>();

        }

        public override async Task<IdentityResult> CreateAsync(TUser user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }
            return await base.CreateAsync(user);
        }

        public virtual async Task<TUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await Store.FindByNameOrEmailAsync(userNameOrEmailAddress);
        }

        public virtual Task<List<TUser>> FindAllAsync(UserLoginInfo login)
        {
            return Store.FindAllAsync(login);
        }

        /// <summary>
        /// Gets a user by given id.
        /// Throws exception if no user found with given id.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        /// <exception cref="Exception">Throws exception if no user found with given id</exception>
        public virtual async Task<TUser> GetUserByIdAsync(long userId)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new FrameworkException("There is no user with id: " + userId);
            }

            return user;
        }

        public async override Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            //if (user.TenantId.HasValue)
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            //}

            return identity;
        }

        public async override Task<IdentityResult> UpdateAsync(TUser user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            //Admin user's username can not be changed!
            if (user.UserName != User<TUser>.AdminUserName)
            {
                if ((await GetOldUserNameAsync(user.Id)) == User<TUser>.AdminUserName)
                {
                    return IdentityResult.Failed(string.Format("不能重命名默认管理员的用户名 {0}", User<TUser>.AdminUserName));
                }
            }

            return await base.UpdateAsync(user);
        }

        public async override Task<IdentityResult> DeleteAsync(TUser user)
        {
            if (user.UserName == User<TUser>.AdminUserName)
            {
                return IdentityResult.Failed(string.Format("不能删除默认管理员{0}!", User<TUser>.AdminUserName));
            }

            return await base.DeleteAsync(user);
        }

        public virtual async Task<IdentityResult> ChangePasswordAsync(TUser user, string newPassword)
        {
            var result = await PasswordValidator.ValidateAsync(newPassword);
            if (!result.Succeeded)
            {
                return result;
            }

            await Store.SetPasswordHashAsync(user, PasswordHasher.HashPassword(newPassword));
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId, string userName, string emailAddress)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                return IdentityResult.Failed(string.Format("名字{0}已被占用.", userName));
            }

            //user = (await FindByEmailAsync(emailAddress));
            //if (user != null && user.Id != expectedUserId)
            //{
            //    return IdentityResult.Failed(string.Format("邮箱地址 '{0}' 已被占用.", emailAddress));
            //}

            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> SetRoles(TUser user, string[] roleNames)
        {
            //Remove from removed roles
            if (roleNames == null)
            {
                foreach (var userRole in user.Roles.ToList())
                {
                    var role = await RoleManager.FindByIdAsync(userRole.RoleId);
                    var result = await RemoveFromRoleAsync(user.Id, role.Name);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
                return IdentityResult.Success;
            }
            foreach (var userRole in user.Roles.ToList())
            {
                var role = await RoleManager.FindByIdAsync(userRole.RoleId);
                if (roleNames.All(roleName => role.Name != roleName))
                {
                    var result = await RemoveFromRoleAsync(user.Id, role.Name);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            //Add to added roles
            foreach (var roleName in roleNames)
            {
                var role = await RoleManager.GetRoleByNameAsync(roleName);
                if (user.Roles.All(ur => ur.RoleId != role.Id))
                {
                    var result = await AddToRoleAsync(user.Id, roleName);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            return IdentityResult.Success;
        }

        public virtual async Task<bool> IsInOrganizationUnitAsync(long userId, long ouId)
        {
            return await IsInOrganizationUnitAsync(
                await GetUserByIdAsync(userId),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }

        public virtual async Task<bool> IsInOrganizationUnitAsync(TUser user, OrganizationUnit ou)
        {
            return await _userOrganizationUnitRepository.CountAsync(uou =>
               uou.UserId == user.Id && uou.OrganizationUnitId == ou.Id
                ) > 0;
        }

        public virtual async Task AddToOrganizationUnitAsync(long userId, long ouId)
        {
            await AddToOrganizationUnitAsync(
                await GetUserByIdAsync(userId),
                await _organizationUnitRepository.FirstOrDefaultAsync(x => x.Id == ouId)
                );
        }

        public virtual async Task AddToOrganizationUnitAsync(TUser user, OrganizationUnit ou)
        {
            var currentOus = await GetOrganizationUnitsAsync(user);

            if (currentOus.Any(cou => cou.Id == ou.Id))
            {
                return;
            }
            await _userOrganizationUnitRepository.InsertAsync(new UserOrganizationUnit(user.Id, ou.Id));
        }

        public virtual async Task RemoveFromOrganizationUnitAsync(long userId, long ouId)
        {
            await RemoveFromOrganizationUnitAsync(
                await GetUserByIdAsync(userId),
                await _organizationUnitRepository.FirstOrDefaultAsync(x => x.Id == ouId)
                );
        }

        public virtual async Task RemoveFromOrganizationUnitAsync(TUser user, OrganizationUnit ou)
        {
            await _userOrganizationUnitRepository.DeleteAsync(uou => uou.UserId == user.Id && uou.OrganizationUnitId == ou.Id);
        }

        public virtual async Task SetOrganizationUnitsAsync(long userId, params long[] organizationUnitIds)
        {
            await SetOrganizationUnitsAsync(
                await GetUserByIdAsync(userId),
                organizationUnitIds
                );
        }

        public virtual async Task SetOrganizationUnitsAsync(TUser user, params long[] organizationUnitIds)
        {
            if (organizationUnitIds == null)
            {
                organizationUnitIds = new long[0];
            }

            var currentOus = await GetOrganizationUnitsAsync(user);

            //Remove from removed OUs
            foreach (var currentOu in currentOus)
            {
                if (!organizationUnitIds.Contains(currentOu.Id))
                {
                    await RemoveFromOrganizationUnitAsync(user, currentOu);
                }
            }

            //Add to added OUs
            foreach (var organizationUnitId in organizationUnitIds)
            {
                if (currentOus.All(ou => ou.Id != organizationUnitId))
                {
                    await AddToOrganizationUnitAsync(
                        user,
                        await _organizationUnitRepository.GetAsync(organizationUnitId)
                        );
                }
            }
        }

        public virtual Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(TUser user)
        {
            var query = from uou in _userOrganizationUnitRepository.Query()
                        join ou in _organizationUnitRepository.Query() on uou.OrganizationUnitId equals ou.Id
                        where uou.UserId == user.Id
                        select ou;

            return Task.FromResult(query.ToList());
        }


        public virtual Task<List<TUser>> GetUsersInOrganizationUnit(OrganizationUnit organizationUnit, bool includeChildren = false)
        {
            if (!includeChildren)
            {
                var query = from uou in _userOrganizationUnitRepository.Query()
                            join user in Store.Users on uou.UserId equals user.Id
                            where uou.OrganizationUnitId == organizationUnit.Id
                            select user;

                return Task.FromResult(query.ToList());
            }
            else
            {
                var query = from uou in _userOrganizationUnitRepository.Query()
                            join user in Store.Users on uou.UserId equals user.Id
                            join ou in _organizationUnitRepository.Query() on uou.OrganizationUnitId equals ou.Id
                            where ou.Code.StartsWith(organizationUnit.Code)
                            select user;

                return Task.FromResult(query.ToList());
            }
        }

        public virtual void RegisterTwoFactorProviders()
        {
            TwoFactorProviders.Clear();

            if (false)
            {
                RegisterTwoFactorProvider(
                  "邮箱",
                    new EmailTokenProvider<TUser, long>
                    {
                        Subject = "安全码",
                        BodyFormat = "您的安全码是: {0}"
                    }
                );
            }

            if (false)
            {
                RegisterTwoFactorProvider(
                   "短信",
                    new PhoneNumberTokenProvider<TUser, long>
                    {
                        MessageFormat = "您的短信验证码是: {0}"
                    }
                );
            }
        }

        public virtual void InitializeLockoutSettings(int? tenantId)
        {
            UserLockoutEnabledByDefault = false;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromSeconds(10);
            MaxFailedAccessAttemptsBeforeLockout = 5;
        }

        public override async Task<IList<string>> GetValidTwoFactorProvidersAsync(long userId)
        {
            var user = await GetUserByIdAsync(userId);

            RegisterTwoFactorProviders();

            return await base.GetValidTwoFactorProvidersAsync(userId);
        }

        public override async Task<IdentityResult> NotifyTwoFactorTokenAsync(long userId, string twoFactorProvider, string token)
        {
            var user = await GetUserByIdAsync(userId);

            RegisterTwoFactorProviders();

            return await base.NotifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
        }

        public override async Task<string> GenerateTwoFactorTokenAsync(long userId, string twoFactorProvider)
        {
            var user = await GetUserByIdAsync(userId);

            RegisterTwoFactorProviders();

            return await base.GenerateTwoFactorTokenAsync(userId, twoFactorProvider);
        }

        public override async Task<bool> VerifyTwoFactorTokenAsync(long userId, string twoFactorProvider, string token)
        {
            var user = await GetUserByIdAsync(userId);

            RegisterTwoFactorProviders();

            return await base.VerifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
        }

        protected virtual Task<string> GetOldUserNameAsync(long userId)
        {
            return Store.GetUserNameFromDatabaseAsync(userId);
        }







    }
}