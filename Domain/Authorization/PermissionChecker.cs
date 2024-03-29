﻿//using System.Threading.Tasks;
//using Authorization.Roles;
//using Authorization.Users;

//using Runtime.Session;
//using Castle.Core.Logging;

//namespace Authorization
//{
//    /// <summary>
//    /// Application should inherit this class to implement <see cref="IPermissionChecker"/>.
//    /// </summary>
//    /// <typeparam name="TRole"></typeparam>
//    /// <typeparam name="TUser"></typeparam>
//    public abstract class PermissionChecker<TRole, TUser> : IPermissionChecker, ITransientDependency, IIocManagerAccessor
//        where TRole : Role<TUser>, new()
//        where TUser : User<TUser>
//    {
//        private readonly UserManager<TRole, TUser> _userManager;

//        public IIocManager IocManager { get; set; }

//        public ILogger Logger { get; set; }

//        public ISession Session { get; set; }

//        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        protected PermissionChecker(UserManager<TRole, TUser> userManager)
//        {
//            _userManager = userManager;

//            Logger = NullLogger.Instance;
//            Session = NullSession.Instance;
//        }

//        public virtual async Task<bool> IsGrantedAsync(string permissionName)
//        {
//            return Session.UserId.HasValue && await _userManager.IsGrantedAsync(Session.UserId.Value, permissionName);
//        }

//        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
//        {
//            return await _userManager.IsGrantedAsync(userId, permissionName);
//        }

//        [UnitOfWork]
//        public virtual async Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
//        {
//            if (CurrentUnitOfWorkProvider == null || CurrentUnitOfWorkProvider.Current == null)
//            {
//                return await IsGrantedAsync(user.UserId, permissionName);
//            }

//            using (CurrentUnitOfWorkProvider.Current.SetTenantId(user.TenantId))
//            {
//                return await _userManager.IsGrantedAsync(user.UserId, permissionName);
//            }
//        }
//    }
//}
