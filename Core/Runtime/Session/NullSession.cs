using Core.Configuration.Startup;
//using Core.MultiTenancy;
using Core.Runtime.Remoting;

namespace Core.Runtime.Session
{
    /// <summary>
    /// Implements null object pattern for <see cref="ISession"/>.
    /// </summary>
    public class NullSession : SessionBase
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullSession Instance { get; } = new NullSession();

        /// <inheritdoc/>
        public override long? UserId => null;

        /// <inheritdoc/>
        public override int? TenantId => null;

       // public override MultiTenancySides MultiTenancySide => MultiTenancySides.Tenant;

        public override long? ImpersonatorUserId => null;

        public override int? ImpersonatorTenantId => null;

        private NullSession() 
            : base(
                  //new MultiTenancyConfig(), 
                  new DataContextAmbientScopeProvider<SessionOverride>(new AsyncLocalAmbientDataContext())
            )
        {

        }
    }
}