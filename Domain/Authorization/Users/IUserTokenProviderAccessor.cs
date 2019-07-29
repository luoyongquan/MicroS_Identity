using Microsoft.AspNetCore.Identity;

namespace Domain.Authorization.Users
{
    public interface IUserTokenProviderAccessor
    {
        IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>() 
            where TUser : User<TUser>;
    }
}