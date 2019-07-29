using Domain.Authorization.Module;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Authorization.Roles
{
    /// <summary>
    /// Used to perform Module database operations for a role.
    /// </summary>
    public interface IRoleModuleStore<in TRole>
        where TRole : RoleBase
    {
        /// <summary>
        /// Adds a Module grant setting to a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="ModuleGrant">Module grant setting info</param>
        Task AddModuleAsync(TRole role, ModuleBase ModuleGrant);

        /// <summary>
        /// Removes a Module grant setting from a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="ModuleGrant">Module grant setting info</param>
        Task RemoveModuleAsync(TRole role, ModuleBase ModuleGrant);

        /// <summary>
        /// Gets Module grant setting informations for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of Module setting informations</returns>
        Task<IList<ModuleBase>> GetModulesAsync(TRole role);

        /// <summary>
        /// Gets Module grant setting informations for a role.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>List of Module setting informations</returns>
        Task<IList<ModuleBase>> GetModulesAsync(int roleId);

        /// <summary>
        /// Checks whether a role has a Module grant setting info.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="ModuleGrant">Module grant setting info</param>
        /// <returns></returns>
        Task<bool> HasModuleAsync(int roleId, ModuleBase ModuleGrant);

        /// <summary>
        /// Deleted all Module settings for a role.
        /// </summary>
        /// <param name="role">Role</param>
        Task RemoveAllModuleSettingsAsync(TRole role);
    }
}