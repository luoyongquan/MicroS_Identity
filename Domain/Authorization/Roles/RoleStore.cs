using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Users;
using Core.Dependency;
using Core.Domain.Repositories;
using Domain.Authorization.Users;
using Microsoft.AspNetCore.Identity;

namespace Domain.Authorization.Roles
{
    /// <summary>
    /// Implements 'Role Store' of ASP.NET Identity Framework.
    /// </summary>
    public abstract class RoleStore<TRole, TUser> :
        IQueryableRoleStore<TRole>,
        ITransientDependency

        where TRole : Role<TUser>
        where TUser : User<TUser>
    {
        private readonly IRepository<TRole, long> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected RoleStore(
            IRepository<TRole,long> roleRepository,
            IRepository<UserRole, long> userRoleRepository
           )
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            
        }

        public virtual IQueryable<TRole> Roles
        {
            get { return _roleRepository.GetAll(); }
        }

        public virtual async Task CreateAsync(TRole role)
        {
            await _roleRepository.InsertAsync(role);
        }

        public virtual async Task UpdateAsync(TRole role)
        {
            await _roleRepository.UpdateAsync(role);
        }

        public virtual async Task DeleteAsync(TRole role)
        {
            await _userRoleRepository.DeleteAsync(ur => ur.RoleId == role.Id);
            await _roleRepository.DeleteAsync(role);
        }

        public virtual async Task<TRole> FindByIdAsync(long roleId)
        {
            return await _roleRepository.FirstOrDefaultAsync(roleId);
        }

        public virtual async Task<TRole> FindByNameAsync(string roleName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.Name == roleName
                );
        }

        public virtual async Task<TRole> FindByDisplayNameAsync(string displayName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.DisplayName == displayName
                );
        }
        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
