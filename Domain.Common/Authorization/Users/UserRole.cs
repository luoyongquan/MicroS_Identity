using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities;
using Core.Domain.Entities.Auditing;

namespace Domain.Authorization.Users
{
    /// <summary>
    /// Represents role record of a user. 
    /// </summary>
    [Table("_UserRoles")]
    public class UserRole: CreationAuditedEntity<long>
    {
      
        /// <summary>
        /// User id.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Role id.
        /// </summary>
        public virtual long RoleId { get; set; }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        public UserRole()
        {

        }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        public UserRole(long userId, long roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}