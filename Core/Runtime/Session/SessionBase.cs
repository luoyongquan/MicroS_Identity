using System;
using Core.Configuration.Startup;
//using Core.MultiTenancy;

namespace Core.Runtime.Session
{
    public abstract class SessionBase : ISession
    {
        public const string SessionOverrideContextKey = "Core.Runtime.Session.Override";

        //public IMultiTenancyConfig MultiTenancy { get; }

        public abstract long? UserId { get; }

        public abstract int? TenantId { get; }

        public abstract long? ImpersonatorUserId { get; }

        public abstract int? ImpersonatorTenantId { get; }

        //public virtual MultiTenancySides MultiTenancySide
        //{
        //    get
        //    {
        //        return MultiTenancy.IsEnabled && !TenantId.HasValue
        //            ? MultiTenancySides.Host
        //            : MultiTenancySides.Tenant;
        //    }
        //}

        protected SessionOverride OverridedValue => SessionOverrideScopeProvider.GetValue(SessionOverrideContextKey);
        protected IAmbientScopeProvider<SessionOverride> SessionOverrideScopeProvider { get; }

        protected SessionBase(
            //IMultiTenancyConfig multiTenancy, 
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
        {
            //MultiTenancy = multiTenancy;
            SessionOverrideScopeProvider = sessionOverrideScopeProvider;
        }

        public IDisposable Use(int? tenantId, long? userId)
        {
            return SessionOverrideScopeProvider.BeginScope(SessionOverrideContextKey, new SessionOverride(tenantId, userId));
        }
    }
}