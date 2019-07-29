
using Core.Dependency;
using Microsoft.AspNetCore.Identity;

namespace Domain.Authorization.Users
{
    public class NullUserTokenProviderAccessor : IUserTokenProviderAccessor, ISingletonDependency
    {
        public IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>() where TUser : User<TUser>
        {
            return null;
        }
    }
}