using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Authorization.Users;
using Core.Domain.Entities;
using Core.Domain.Entities.Auditing;
using Domain.Authorization.Users;

namespace Domain.Authorization.Roles
{
    /// <summary>
    /// Base class for role.
    /// </summary>
    [Table("_Roles")]
    public abstract class RoleBase : FullAuditedEntity<long>
    {
        /// <summary>
        /// Maximum length of the <see cref="DisplayName"/> property.
        /// </summary>
        public const int MaxDisplayNameLength = 64;

        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// Unique name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Is this a static role?
        /// Static roles can not be deleted, can not change their name.
        /// They can be used programmatically.
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// Is this role will be assigned to new users as default?
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// Roles of this user.
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual ICollection<UserRole> Roles { get; set; }
      
        protected RoleBase()
        {
            Name = Guid.NewGuid().ToString("N");
        }

        protected RoleBase(string displayName)
            : this()
        {

            DisplayName = displayName;
        }

        protected RoleBase(string name, string displayName)
            : this(displayName)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"[Role {Id}, Name={Name}]";
        }
    }
}