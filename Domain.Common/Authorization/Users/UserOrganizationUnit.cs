using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities;
using Core.Domain.Entities.Auditing;
using Domain.Organizations;

namespace Domain.Authorization.Users
{
    /// <summary>
    /// Represents membership of a User to an OU.
    /// </summary>
    [Table("_UserOrganizationUnits")]
    public class UserOrganizationUnit : CreationAuditedEntity<long>, ISoftDelete
    {

        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Id of the <see cref="OrganizationUnit"/>.
        /// </summary>
        public virtual long OrganizationUnitId { get; set; }

        /// <summary>
        /// Specifies if the organization is soft deleted or not.
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOrganizationUnit"/> class.
        /// </summary>
        public UserOrganizationUnit()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOrganizationUnit"/> class.
        /// </summary>
        /// <param name="userId">Id of the User.</param>
        /// <param name="organizationUnitId">Id of the <see cref="OrganizationUnit"/>.</param>
        public UserOrganizationUnit(long userId, long organizationUnitId)
        {

            UserId = userId;
            OrganizationUnitId = organizationUnitId;
        }
    }
}
