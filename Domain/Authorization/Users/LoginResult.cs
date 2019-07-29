using System.Security.Claims;


namespace Domain.Authorization.Users
{
    public class LoginResult<TUser>
      where TUser : UserBase
    {
        public LoginResultType Result { get; private set; }

        public TUser User { get; private set; }

        public ClaimsIdentity Identity { get; private set; }

        public LoginResult(LoginResultType result,  TUser user = null)
        {
            Result = result;
           
            User = user;
        }

        public LoginResult(TUser user, ClaimsIdentity identity)
            : this(LoginResultType.Success)
        {
            User = user;
            Identity = identity;
        }
    }
}