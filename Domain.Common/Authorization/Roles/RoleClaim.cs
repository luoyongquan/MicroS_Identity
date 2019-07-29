using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Core.Domain.Entities;
using Core.Domain.Entities.Auditing;

namespace Domain.Authorization.Roles
{
    [Table("_RoleClaims")]
    public class RoleClaim : CreationAuditedEntity<long>
    {
        /// <summary>
        /// Maximum length of the <see cref="ClaimType"/> property.
        /// </summary>
        public const int MaxClaimTypeLength = 256;

        public virtual long RoleId { get; set; }

        [StringLength(MaxClaimTypeLength)]
        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }

        public RoleClaim()
        {
            
        }

        public RoleClaim(RoleBase role, Claim claim)
        {
         
            RoleId = role.Id;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
