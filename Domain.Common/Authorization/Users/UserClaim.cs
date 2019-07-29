using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Core.Domain.Entities;
using Core.Domain.Entities.Auditing;

namespace Domain.Authorization.Users
{
    [Table("_UserClaims")]
    public class UserClaim : CreationAuditedEntity<long>
    {
        /// <summary>
        /// Maximum length of the <see cref="ClaimType"/> property.
        /// </summary>
        public const int MaxClaimTypeLength = 256;

        public virtual long UserId { get; set; }

        [StringLength(MaxClaimTypeLength)]
        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }

        public UserClaim()
        {

        }

        public UserClaim(UserBase user, Claim claim)
        {
            UserId = user.Id;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
