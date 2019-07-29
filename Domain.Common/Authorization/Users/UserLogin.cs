using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities;

namespace Domain.Authorization.Users
{
    /// <summary>
    /// Used to store a User Login for external Login services.
    /// </summary>
    [Table("_UserLogins")]
    public class UserLogin : Entity<long>
    {
        /// <summary>
        /// Maximum length of <see cref="LoginProvider"/> property.
        /// </summary>
        public const int MaxLoginProviderLength = 128;

        /// <summary>
        /// Maximum length of <see cref="ProviderKey"/> property.
        /// </summary>
        public const int MaxProviderKeyLength = 256;

        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Login Provider.
        /// </summary>
        [Required]
        [MaxLength(MaxLoginProviderLength)]
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Key in the <see cref="LoginProvider"/>.
        /// </summary>
        [Required]
        [MaxLength(MaxProviderKeyLength)]
        public virtual string ProviderKey { get; set; }

        public UserLogin()
        {
            
        }

        public UserLogin(long userId, string loginProvider, string providerKey)
        {
         
            UserId = userId;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }
    }
}
