﻿//using Core.Application.Features;
using Core.Collections.Extensions;
//using Core.Localization;
//using Core.MultiTenancy;

namespace Core.Authorization
{
    internal abstract class PermissionDefinitionContextBase : IPermissionDefinitionContext
    {
        protected readonly PermissionDictionary Permissions;

        protected PermissionDefinitionContextBase()
        {
            Permissions = new PermissionDictionary();
        }

        public Permission CreatePermission(
            //string name,
            //ILocalizableString displayName = null,
            //ILocalizableString description = null,
            //MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            //IFeatureDependency featureDependency = null
            )
        {
            //if (Permissions.ContainsKey(name))
            //{
            //    throw new CoreException("There is already a permission with name: " + name);
            //}

            //var permission = new Permission(name, displayName, description, multiTenancySides, featureDependency);
            //Permissions[permission.Name] = permission;
            var permission = new Permission();
            return permission;
        }

        public Permission GetPermissionOrNull(string name)
        {
            return Permissions.GetOrDefault(name);
        }

        public void RemovePermission(string name)
        {
            Permissions.Remove(name);
        }
    }
}