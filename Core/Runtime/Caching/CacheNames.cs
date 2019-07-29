namespace Core.Runtime.Caching
{
    /// <summary>
    /// Names of standard caches used in Core.
    /// </summary>
    public static class CacheNames
    {
        /// <summary>
        /// Application settings cache: ApplicationSettingsCache.
        /// </summary>
        public const string ApplicationSettings = "ApplicationSettingsCache";

        /// <summary>
        /// Tenant settings cache: TenantSettingsCache.
        /// </summary>
        public const string TenantSettings = "TenantSettingsCache";

        /// <summary>
        /// User settings cache: UserSettingsCache.
        /// </summary>
        public const string UserSettings = "UserSettingsCache";

        /// <summary>
        /// Localization scripts cache: LocalizationScripts.
        /// </summary>
        public const string LocalizationScripts = "LocalizationScripts";
    }
}