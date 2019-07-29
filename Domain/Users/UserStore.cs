using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Roles;
using Microsoft.AspNetCore.Identity;

namespace Domain.Users
{
    /// <summary>
    /// 针对不同的Area，可以定义多个UserStore。
    /// TODO：可以支持多种形式的用户读取吗？比如AD，接口验证等？待研究
    /// </summary>
    public class UserStore<TRole, TUser> :
        IUserStore<TUser>,
        IUserPasswordStore<TUser>
        where TRole : Role
        where TUser : User
    {
        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }


        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            cancellationToken.ThrowIfCancellationRequested();

            // 根据 ID 找用户
            // 这里就可以进行数据库的访问了
            // 本文就简单处理了，直接返回了一个用户对象
            //return await Task.Run(() =>
            //{
            //    return new User
            //    {
            //        Id = 122333,
            //        UserName = "admin",   // 一定要给这个属性赋值，否则会报 value cannot be null
            //        PasswordHash = "111222",
            //    };
            //});
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}