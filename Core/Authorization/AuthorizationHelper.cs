using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
//using Core.Application.Features;
using Core.Configuration.Startup;
using Core.Dependency;
//using Core.Localization;
using Core.Reflection;
using Core.Runtime.Session;

namespace Core.Authorization
{
    public class AuthorizationHelper : IAuthorizationHelper, ITransientDependency
    {
        public ISession Session { get; set; }
        public IPermissionChecker PermissionChecker { get; set; }
        //public ILocalizationManager LocalizationManager { get; set; }

        //private readonly IFeatureChecker _featureChecker;
        private readonly IAuthorizationConfiguration _authConfiguration;

        public AuthorizationHelper(
            //IFeatureChecker featureChecker, 
            IAuthorizationConfiguration authConfiguration
            )
        {
            //_featureChecker = featureChecker;
            _authConfiguration = authConfiguration;
            Session = NullSession.Instance;
            PermissionChecker = NullPermissionChecker.Instance;
            //LocalizationManager = NullLocalizationManager.Instance;
        }

        public virtual async Task AuthorizeAsync(IEnumerable<IAuthorizeAttribute> authorizeAttributes)
        {
            if (!_authConfiguration.IsEnabled)
            {
                return;
            }

            if (!Session.UserId.HasValue)
            {
                //throw new AuthorizationException(
                //    LocalizationManager.GetString(Consts.LocalizationSourceName, "CurrentUserDidNotLoginToTheApplication")
                //    );
            }

            foreach (var authorizeAttribute in authorizeAttributes)
            {
                await PermissionChecker.AuthorizeAsync(authorizeAttribute.RequireAllPermissions, authorizeAttribute.Permissions);
            }
        }

        public virtual async Task AuthorizeAsync(MethodInfo methodInfo, Type type)
        {
            //await CheckFeatures(methodInfo, type);
            await CheckPermissions(methodInfo, type);
        }

        protected virtual async Task CheckFeatures(MethodInfo methodInfo, Type type)
        {
            //var featureAttributes = ReflectionHelper.GetAttributesOfMemberAndType<RequiresFeatureAttribute>(methodInfo, type);

            //if (featureAttributes.Count <= 0)
            //{
            //    return;
            //}

            //foreach (var featureAttribute in featureAttributes)
            //{
            //    await _featureChecker.CheckEnabledAsync(featureAttribute.RequiresAll, featureAttribute.Features);
            //}
        }

        protected virtual async Task CheckPermissions(MethodInfo methodInfo, Type type)
        {
            if (!_authConfiguration.IsEnabled)
            {
                return;
            }

            if (AllowAnonymous(methodInfo, type))
            {
                return;
            }

            if (ReflectionHelper.IsPropertyGetterSetterMethod(methodInfo, type))
            {
                return;
            }

            if (!methodInfo.IsPublic && !methodInfo.GetCustomAttributes().OfType<IAuthorizeAttribute>().Any())
            {
                return;
            }

            var authorizeAttributes =
                ReflectionHelper
                    .GetAttributesOfMemberAndType(methodInfo, type)
                    .OfType<IAuthorizeAttribute>()
                    .ToArray();

            if (!authorizeAttributes.Any())
            {
                return;
            }

            await AuthorizeAsync(authorizeAttributes);
        }

        private static bool AllowAnonymous(MemberInfo memberInfo, Type type)
        {
            return ReflectionHelper
                .GetAttributesOfMemberAndType(memberInfo, type)
                .OfType<IAllowAnonymousAttribute>()
                .Any();
        }
    }
}