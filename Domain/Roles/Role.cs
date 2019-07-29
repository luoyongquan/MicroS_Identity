using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Roles
{
    public class Role: IdentityRole<int>
    {
        public virtual User DeleterUser { get; set; }

        public virtual User CreatorUser { get; set; }

        public virtual User LastModifierUser { get; set; }

        protected Role()
        {
        }

        /// <summary>
        /// Unique name of this role.
        /// </summary>
   
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
     
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

        ///// <summary>
        ///// Roles of this user.
        ///// </summary>
    
        //public virtual ICollection<UserRole> Roles { get; set; }


        protected Role(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return $"[Role {Id}, Name={Name}]";
        }


        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }

        /// <summary>
        /// Last modification date of this entity.
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Last modifier user of this entity.
        /// </summary>
        public virtual long? LastModifierUserId { get; set; }
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator of this entity.
        /// </summary>
        public virtual long? CreatorUserId { get; set; }
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public virtual int Id { get; set; }
        public Guid UniqueIdentity { get; set; } = Guid.NewGuid();
    }
}
