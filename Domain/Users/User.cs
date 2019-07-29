using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    public class User : IdentityUser
    {
        /// <summary>
        /// 用户头像
        /// </summary>

        public string Avatar { get; set; }

        public virtual long? OrganizationUnitId { get; set; }
        public virtual User DeleterUser { get; set; }

        public virtual User CreatorUser { get; set; }

        public virtual User LastModifierUser { get; set; }

        /// <summary>
        /// Authorization source name.
        /// It's set to external authentication source name if created by an external source.
        /// Default: null.
        /// </summary>
        public virtual string AuthenticationSource { get; set; }

        /// <summary>
        /// User name.
        /// User name must be unique for it's tenant.
        /// </summary>
        public virtual string UserName { get; set; }



        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public virtual string Name { get; set; }


        /// <summary>
        /// Password of the user.
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Confirmation code for email.
        /// </summary>
        public virtual string EmailConfirmationCode { get; set; }

        /// <summary>
        /// Reset code for password.
        /// It's not valid if it's null.
        /// It's for one usage and must be set to null after reset.
        /// </summary>
        public virtual string PasswordResetCode { get; set; }

        /// <summary>
        /// Lockout end date.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the access failed count.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the lockout enabled.
        /// </summary>
        public virtual bool IsLockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Is the <see cref="PhoneNumber"/> confirmed.
        /// </summary>
        public virtual bool IsPhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the security stamp.
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// Is two factor auth enabled.
        /// </summary>
        public virtual bool IsTwoFactorEnabled { get; set; }

        ///// <summary>
        ///// Login definitions for this user.
        ///// </summary>
        //public virtual ICollection<UserLogin> Logins { get; set; }

        ///// <summary>
        ///// Roles of this user.
        ///// </summary>
        //public virtual ICollection<UserRole> Roles { get; set; }

        ///// <summary>
        ///// Claims of this user.
        ///// </summary>
        //public virtual ICollection<UserClaim> Claims { get; set; }



        /// <summary>
        /// Is the <see cref="EjiorFrameworkUserBase.EmailAddress"/> confirmed.
        /// </summary>
        public virtual bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Is this user active?
        /// If as user is not active, he/she can not use the application.
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// The last time this user entered to the system.
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }

        public User()
        {
            IsActive = true;
            IsLockoutEnabled = true;
            SecurityStamp = Guid.NewGuid().ToString("N");
            CreationTime = DateTime.Now;
        }

        public virtual void SetNewPasswordResetCode()
        {
            PasswordResetCode = Guid.NewGuid().ToString("N");
        }

        public virtual void SetNewEmailConfirmationCode()
        {
            EmailConfirmationCode = Guid.NewGuid().ToString("N");
        }


        public override string ToString()
        {
            return $"[User {Id}] {UserName}";
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
