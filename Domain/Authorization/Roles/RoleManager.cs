using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Dependency;
using Core.Runtime.Session;
using Domain;
using Domain.Authorization.Users;
using Domain.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Domain.Authorization.Roles
{
    /// <summary>
    /// Extends <see cref="RoleManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// Applications should derive this class with appropriate generic arguments.
    /// </summary>
    public abstract class CoreRoleManager<TRole, TUser>
        : RoleManager<TRole>,
        ITransientDependency
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>
    {

        public ISession Session { get; set; }



        //private IRolePermissionStore<TRole> RolePermissionStore
        //{
        //    get
        //    {
        //        if (!(Store is IRolePermissionStore<TRole>))
        //        {
        //            throw new Exception("Store is not IRolePermissionStore");
        //        }

        //        return Store as IRolePermissionStore<TRole>;
        //    }
        //}

        protected RoleStore<TRole, TUser> Store { get; private set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        protected CoreRoleManager(RoleStore<TRole, TUser> store) : base(store)
        {

            Store = store;
            Session = NullSession.Instance;

        }



        /// <summary>
        /// Creates a role.
        /// </summary>
        /// <param name="role">Role</param>
        public override async Task<IdentityResult> CreateAsync(TRole role)
        {
            var result = await CheckDuplicateRoleNameAsync(role.Id, role.Name, role.DisplayName);
            if (!result.Succeeded)
            {
                return result;
            }
            return await base.CreateAsync(role);
        }

        public override async Task<IdentityResult> UpdateAsync(TRole role)
        {
            var result = await CheckDuplicateRoleNameAsync(role.Id, role.Name, role.DisplayName);
            if (!result.Succeeded)
            {
                return result;
            }

            return await base.UpdateAsync(role);
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="role">Role</param>
        public async override Task<IdentityResult> DeleteAsync(TRole role)
        {
            if (role.IsStatic)
            {
                return IdentityResult.Failed(string.Format("不能删除系统预定义角色: {0}", role.Name));
            }

            return await base.DeleteAsync(role);
        }

        /// <summary>
        /// Gets a role by given id.
        /// Throws exception if no role with given id.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>Role</returns>
        /// <exception cref="Exception">Throws exception if no role with given id</exception>
        public virtual async Task<TRole> GetRoleByIdAsync(long roleId)
        {
            var role = await FindByIdAsync(roleId);
            if (role == null)
            {
                throw new FrameworkException("There is no role with id: " + roleId);
            }

            return role;
        }

        /// <summary>
        /// Gets a role by given name.
        /// Throws exception if no role with given roleName.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Role</returns>
        /// <exception cref="Exception">Throws exception if no role with given roleName</exception>
        public virtual async Task<TRole> GetRoleByNameAsync(string roleName)
        {
            var role = await FindByNameAsync(roleName);
            if (role == null)
            {
                throw new FrameworkException("There is no role with name: " + roleName);
            }

            return role;
        }

    
        public virtual async Task<IdentityResult> CheckDuplicateRoleNameAsync(long? expectedRoleId, string name, string displayName)
        {
            var role = await FindByNameAsync(name);
            if (role != null && role.Id != expectedRoleId)
            {
                return IdentityResult.Failed(string.Format("角色名{0}已被占用.", name));
            }

            role = await FindByDisplayNameAsync(displayName);
            if (role != null && role.Id != expectedRoleId)
            {
                return IdentityResult.Failed(string.Format("角色显示名称 {0}已被占用.", displayName));
            }

            return IdentityResult.Success;
        }

        private Task<TRole> FindByDisplayNameAsync(string displayName)
        {
            return Store.FindByDisplayNameAsync(displayName);
        }

      

    }
}