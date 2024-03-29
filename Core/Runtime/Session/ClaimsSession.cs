using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Core.Configuration.Startup;
using Core.Dependency;
//using Core.MultiTenancy;
using Core.Runtime.Security;

namespace Core.Runtime.Session
{
    /// <summary>
    /// Implements <see cref="ISession"/> to get session properties from current claims.
    /// </summary>
    public class ClaimsSession : SessionBase, ISingletonDependency
    {
        public override long? UserId
        {
            get
            {
                if (OverridedValue != null)
                {
                    return OverridedValue.UserId;
                }

                var userIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == CoreClaimTypes.UserId);
                if (string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    return null;
                }

                long userId;
                if (!long.TryParse(userIdClaim.Value, out userId))
                {
                    return null;
                }

                return userId;
            }
        }

        public override int? TenantId
        {
            get
            {
                //if (!MultiTenancy.IsEnabled)
                //{
                //    return MultiTenancyConsts.DefaultTenantId;
                //}

                if (OverridedValue != null)
                {
                    return OverridedValue.TenantId;
                }

                var tenantIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == CoreClaimTypes.TenantId);
                if (!string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return Convert.ToInt32(tenantIdClaim.Value);
                }

                if (UserId == null)
                {
                    //Resolve tenant id from request only if user has not logged in!
                    //return TenantResolver.ResolveTenantId();
                }
                
                return null;
            }
        }

        public override long? ImpersonatorUserId
        {
            get
            {
                var impersonatorUserIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == CoreClaimTypes.ImpersonatorUserId);
                if (string.IsNullOrEmpty(impersonatorUserIdClaim?.Value))
                {
                    return null;
                }

                return Convert.ToInt64(impersonatorUserIdClaim.Value);
            }
        }

        public override int? ImpersonatorTenantId
        {
            get
            {
                //if (!MultiTenancy.IsEnabled)
                //{
                //    return MultiTenancyConsts.DefaultTenantId;
                //}

                var impersonatorTenantIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == CoreClaimTypes.ImpersonatorTenantId);
                if (string.IsNullOrEmpty(impersonatorTenantIdClaim?.Value))
                {
                    return null;
                }

                return Convert.ToInt32(impersonatorTenantIdClaim.Value);
            }
        }

        protected IPrincipalAccessor PrincipalAccessor { get; }
        //protected ITenantResolver TenantResolver { get; }

        public ClaimsSession(
            IPrincipalAccessor principalAccessor,
            //IMultiTenancyConfig multiTenancy,
            //ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
            : base(
                  //multiTenancy, 
                  sessionOverrideScopeProvider)
        {
            //TenantResolver = tenantResolver;
            PrincipalAccessor = principalAccessor;
        }
    }
}