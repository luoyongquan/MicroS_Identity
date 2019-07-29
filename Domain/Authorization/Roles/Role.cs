using Domain.Authorization.Users;
using Core.Domain.Entities.Auditing;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Authorization.Roles
{
    /// <summary>
    /// Represents a role in an application. A role is used to group permissions.
    /// </summary>
    /// <remarks> 
    /// Application should use permissions to check if user is granted to perform an operation.
    /// Checking 'if a user has a role' is not possible until the role is static (<see cref="RoleBase.IsStatic"/>).
    /// Static roles can be used in the code and can not be deleted by users.
    /// Non-static (dynamic) roles can be added/removed by users and we can not know their name while coding.
    /// A user can have multiple roles. Thus, user will have all permissions of all assigned roles.
    /// </remarks>
    public abstract class Role<TUser> : RoleBase, IFullAudited<TUser>
        where TUser : User<TUser>
    {
        public virtual TUser DeleterUser { get; set; }

        public virtual TUser CreatorUser { get; set; }

        public virtual TUser LastModifierUser { get; set; }

        protected Role()
        {
        }

        /// <summary>
        /// Creates a new <see cref="Role{TUser}"/> object.
        /// </summary>
        /// <param name="tenantId">TenantId or null (if this is not a tenant-level role)</param>
        /// <param name="displayName">Display name of the role</param>
        protected Role(string displayName)
            : base(displayName)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Role{TUser}"/> object.
        /// </summary>
        /// <param name="tenantId">TenantId or null (if this is not a tenant-level role)</param>
        /// <param name="name">Unique role name</param>
        /// <param name="displayName">Display name of the role</param>
        protected Role(string name, string displayName)
            : base(name, displayName)
        {
        }

      
    }
}