﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
//using Core.Application.Features;
using Core.Collections.Extensions;
using Core.Configuration.Startup;
using Core.Dependency;
//using Core.MultiTenancy;
using Core.Runtime.Session;

namespace Core.Authorization
{
    /// <summary>
    /// Permission manager.
    /// </summary>
    internal class PermissionManager : PermissionDefinitionContextBase, IPermissionManager, ISingletonDependency
    {
        public ISession Session { get; set; }

        private readonly IIocManager _iocManager;
        private readonly IAuthorizationConfiguration _authorizationConfiguration;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PermissionManager(
            IIocManager iocManager,
            IAuthorizationConfiguration authorizationConfiguration)
        {
            _iocManager = iocManager;
            _authorizationConfiguration = authorizationConfiguration;

            Session = NullSession.Instance;
        }

        public void Initialize()
        {
            foreach (var providerType in _authorizationConfiguration.Providers)
            {
                using (var provider = _iocManager.ResolveAsDisposable<AuthorizationProvider>(providerType))
                {
                    provider.Object.SetPermissions(this);
                }
            }

            Permissions.AddAllPermissions();
        }

        public Permission GetPermission(string name)
        {
            var permission = Permissions.GetOrDefault(name);
            if (permission == null)
            {
                throw new CoreException("There is no permission with name: " + name);
            }

            return permission;
        }

        //public IReadOnlyList<Permission> GetAllPermissions(bool tenancyFilter = true)
        //{
        //    using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
        //    {
        //        var featureDependencyContextObject = featureDependencyContext.Object;
        //        return Permissions.Values
        //            .WhereIf(tenancyFilter, p => p.MultiTenancySides.HasFlag(Session.MultiTenancySide))
        //            .Where(p =>
        //                p.FeatureDependency == null ||
        //                Session.MultiTenancySide == MultiTenancySides.Host ||
        //                p.FeatureDependency.IsSatisfied(featureDependencyContextObject)
        //            ).ToImmutableList();
        //    }
        //}

        //public IReadOnlyList<Permission> GetAllPermissions(MultiTenancySides multiTenancySides)
        //{
        //    using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
        //    {
        //        var featureDependencyContextObject = featureDependencyContext.Object;
        //        return Permissions.Values
        //            .Where(p => p.MultiTenancySides.HasFlag(multiTenancySides))
        //            .Where(p =>
        //                p.FeatureDependency == null ||
        //                Session.MultiTenancySide == MultiTenancySides.Host ||
        //                (p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
        //                 multiTenancySides.HasFlag(MultiTenancySides.Host)) ||
        //                p.FeatureDependency.IsSatisfied(featureDependencyContextObject)
        //            ).ToImmutableList();
        //    }
        //}
    }
}