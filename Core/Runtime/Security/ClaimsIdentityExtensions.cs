﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Core.Extensions;
using JetBrains.Annotations;

namespace Core.Runtime.Security
{
    public static class ClaimsIdentityExtensions
    {
        public static UserIdentifier GetUserIdentifierOrNull(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var userId = identity.GetUserId();
            if (userId == null)
            {
                return null;
            }

            return new UserIdentifier(identity.GetTenantId(), userId.Value);
        }

        public static long? GetUserId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == CoreClaimTypes.UserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt64(userIdOrNull.Value);
        }

        public static int? GetTenantId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == CoreClaimTypes.TenantId);
            if (tenantIdOrNull == null || tenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt32(tenantIdOrNull.Value);
        }

        public static long? GetImpersonatorUserId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == CoreClaimTypes.ImpersonatorUserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt64(userIdOrNull.Value);
        }

        public static int? GetImpersonatorTenantId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == CoreClaimTypes.ImpersonatorTenantId);
            if (tenantIdOrNull == null || tenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt32(tenantIdOrNull.Value);
        }
    }
}