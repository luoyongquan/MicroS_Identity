﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Authorization.Roles;
using Core;
using Core.Dependency;
using Core.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Domain.Authorization.Users
{
    /// <summary>
    /// Implements 'User Store' of ASP.NET Identity Framework.
    /// </summary>
    public abstract class UserStore<TRole, TUser> :
        IUserStore<TUser, long>,
        IUserPasswordStore<TUser, long>,
        IUserEmailStore<TUser, long>,
        IUserLoginStore<TUser, long>,
        IUserRoleStore<TUser, long>,
        IQueryableUserStore<TUser, long>,
        IUserLockoutStore<TUser, long>,
        IUserPhoneNumberStore<TUser, long>,
        IUserClaimStore<TUser, long>,
        IUserSecurityStampStore<TUser, long>,
        IUserTwoFactorStore<TUser, long>,

        ITransientDependency

        where TRole : Role<TUser>
        where TUser : User<TUser>
    {


        private readonly IRepository<TUser, long> _userRepository;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<UserClaim, long> _userClaimRepository;
        private readonly IRepository<TRole, long> _roleRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected UserStore(
            IRepository<TUser, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<TRole, long> roleRepository,

            IRepository<UserClaim, long> userClaimRepository)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userClaimRepository = userClaimRepository;

        }

        #region IQueryableUserStore

        public virtual IQueryable<TUser> Users => _userRepository.GetAll();

        #endregion

        #region IUserStore

        public virtual async Task CreateAsync(TUser user)
        {
            await _userRepository.InsertAsync(user);
        }

        public virtual async Task UpdateAsync(TUser user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public virtual async Task DeleteAsync(TUser user)
        {
            await _userRepository.DeleteAsync(user.Id);
        }

        public virtual async Task<TUser> FindByIdAsync(long userId)
        {
            return await _userRepository.FirstOrDefaultAsync(userId);
        }

        public virtual async Task<TUser> FindByNameAsync(string userName)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.UserName == userName
            );
        }

        public virtual async Task<TUser> FindByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.EmailAddress == email
            );
        }

        /// <summary>
        /// Tries to find a user with user name or email address in current tenant.
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        public virtual async Task<TUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => (user.UserName == userNameOrEmailAddress || user.EmailAddress == userNameOrEmailAddress)
                );
        }

        /// <summary>
        /// Tries to find a user with user name or email address in given tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>

        public virtual async Task<TUser> FindByNameOrEmailAsync(int? tenantId, string userNameOrEmailAddress)
        {
            return await FindByNameOrEmailAsync(userNameOrEmailAddress);
        }

        #endregion

        #region IUserPasswordStore

        public virtual Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.FromResult(user.Password);
        }

        public virtual Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        #endregion

        #region IUserEmailStore

        public virtual Task SetEmailAsync(TUser user, string email)
        {
            user.EmailAddress = email;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.EmailAddress);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.IsEmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.IsEmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserLoginStore

        public virtual async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            await _userLoginRepository.InsertAsync(
                new UserLogin
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });
        }

        public virtual async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            await _userLoginRepository.DeleteAsync(
                ul => ul.UserId == user.Id &&
                      ul.LoginProvider == login.LoginProvider &&
                      ul.ProviderKey == login.ProviderKey
            );
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return (await _userLoginRepository.GetAllAsync(ul => ul.UserId == user.Id))
                .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                .ToList();
        }

        public virtual async Task<TUser> FindAsync(UserLoginInfo login)
        {
            var userLogin = await _userLoginRepository.FirstOrDefaultAsync(
                ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey
            );

            if (userLogin == null)
            {
                return null;
            }

            return await _userRepository.FirstOrDefaultAsync(u => u.Id == userLogin.UserId);
        }

        public virtual Task<List<TUser>> FindAllAsync(UserLoginInfo login)
        {
            var query = from userLogin in _userLoginRepository.GetAll()
                        join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                        where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                        select user;

            return Task.FromResult(query.ToList());
        }

        public virtual Task<TUser> FindAsync(int? tenantId, UserLoginInfo login)
        {

            var query = from userLogin in _userLoginRepository.GetAll()
                        join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                        where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                        select user;

            return Task.FromResult(query.FirstOrDefault());

        }

        #endregion

        #region IUserRoleStore

        public virtual async Task AddToRoleAsync(TUser user, string roleName)
        {
            var role = await GetRoleByNameAsync(roleName);
            await _userRoleRepository.InsertAsync(new UserRole(user.Id, role.Id));
        }

        public virtual async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            var role = await GetRoleByNameAsync(roleName);
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            if (userRole == null)
            {
                return;
            }

            await _userRoleRepository.DeleteAsync(userRole);
        }


        public virtual async Task<IList<string>> GetRolesAsync(TUser user)
        {
            // var query = _userRoleRepository.Query();
            //_userRepository.Context.
            //IEntityFrameworkRepositoryContext efContext;
            //if (_userRoleRepository.Context is IEntityFrameworkRepositoryContext)
            //{
            //    efContext = _userRoleRepository.Context as IEntityFrameworkRepositoryContext;
            //}
            //var RepositoryContext = IocManager.Instance.Resolve<IRepositoryContext>();
            //var thisuserRoleRepository = new EntityFrameworkRepository<UserRole, long>(RepositoryContext);
            //var thisroleRepository = new EntityFrameworkRepository<TRole, long>(RepositoryContext);

            var query = from userRole in _userRoleRepository.GetAll()
                        join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                        where userRole.UserId == user.Id
                        select role.Name;

            //var roleIds = _userRoleRepository.Query().Where(u => u.UserId == user.Id).Select(u=>u.RoleId).ToList();
            //var query = _roleRepository.Query().Where(u => roleIds.Contains(u.Id)).Select(u => u.Name);

            return await Task.FromResult(query.ToList());
        }

        public virtual async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            var role = await GetRoleByNameAsync(roleName);
            return await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id) != null;
        }

        #endregion



        #region IUserLockoutStore

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return Task.FromResult(
                user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset()
            );
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? new DateTime?() : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(++user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.IsLockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.IsLockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserPhoneNumberStore

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.IsPhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.IsPhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            var userClaims = await _userClaimRepository.GetAll(uc => uc.UserId == user.Id);
            return userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)).ToList();
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            return _userClaimRepository.InsertAsync(new UserClaim(user, claim));
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            return _userClaimRepository.DeleteAsync(
                uc => uc.UserId == user.Id &&
                      uc.ClaimType == claim.Type &&
                      uc.ClaimValue == claim.Value
            );
        }

        #endregion

        #region IDisposable

        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }

        #endregion

        #region Helpers

        private async Task<TRole> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                throw new CoreException("Could not find a role with name: " + roleName);
            }

            return role;
        }

        #endregion

        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.IsTwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.IsTwoFactorEnabled);
        }

        public async Task<string> GetUserNameFromDatabaseAsync(long userId)
        {
            var user = await _userRepository.GetAsync(userId);
            return user.UserName;

        }
    }
}
