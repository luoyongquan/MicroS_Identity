using Core.Domain.Entities.Auditing;
using Domain.Organizations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public abstract class User<TUser> : UserBase, IFullAudited<TUser>, IMayHaveOrganizationUnit
        where TUser : User<TUser>
    {
        /// <summary>
        /// �û�ͷ��
        /// </summary>
        [Display(Name = "�û�ͷ��")]
        [MaxLength(500)]
        public string Avatar { get; set; }

        public virtual long? OrganizationUnitId { get; set; }
        public virtual TUser DeleterUser { get; set; }

        public virtual TUser CreatorUser { get; set; }

        public virtual TUser LastModifierUser { get; set; }
    }
}