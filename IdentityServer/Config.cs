using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public sealed class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
             {
                 new ApiResource("Server1", "Service1 API"),
                 new ApiResource("Server2", "Service2 API")
             };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
             {
                 new Client
                 {
                     ClientId = "ClientServer1",
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                     ClientSecrets =
                     {
                         new Secret("ClientServer1".Sha256())
                     },
                     AllowedScopes = new List<string> {"Server1"},
                     AccessTokenLifetime = 60 * 60 * 1
                 },
                 new Client
                 {
                     ClientId = "ClientServer2",
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                     ClientSecrets =
                     {
                         new Secret("ClientServer2".Sha256())
                     },
                     AllowedScopes = new List<string> {"Server2"},
                     AccessTokenLifetime = 60 * 60 * 1
                 }
             };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
             {
                 new TestUser
                 {
                     Username = "test",
                     Password = "123456",
                     SubjectId = "1"
                 }
             };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>();
        }
    }
}