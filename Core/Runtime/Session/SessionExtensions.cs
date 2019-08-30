namespace Core.Runtime.Session
{
    /// <summary>
    /// Extension methods for <see cref="ISession"/>.
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Gets current User's Id.
        /// Throws <see cref="Exception"/> if <see cref="ISession.UserId"/> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current User's Id.</returns>
        public static long GetUserId(this ISession session)
        {
            if (!session.UserId.HasValue)
            {
                throw new CoreException("Session.UserId is null! Probably, user is not logged in.");
            }

            return session.UserId.Value;
        }

        /// <summary>
        /// Gets current Tenant's Id.
        /// Throws <see cref="Exception"/> if <see cref="ISession.TenantId"/> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current Tenant's Id.</returns>
        /// <exception cref="Exception"></exception>
        public static int GetTenantId(this ISession session)
        {
            if (!session.TenantId.HasValue)
            {
                throw new CoreException("Session.TenantId is null! Possible problems: No user logged in or current logged in user in a host user (TenantId is always null for host users).");
            }

            return session.TenantId.Value;
        }

        /// <summary>
        /// Creates <see cref="UserIdentifier"/> from given session.
        /// Returns null if <see cref="ISession.UserId"/> is null.
        /// </summary>
        /// <param name="session">The session.</param>
        public static UserIdentifier ToUserIdentifier(this ISession session)
        {
            return session.UserId == null
                ? null
                : new UserIdentifier(session.TenantId, session.GetUserId());
        }
    }
}