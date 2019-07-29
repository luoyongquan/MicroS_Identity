using Domain.Authorization.Module;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Authorization.Roles
{
    /// <summary>
    /// Used to perform Module database operations for a role.
    /// </summary>
    public interface IRoleModuleFunctionStore<in TRole, in TModule>
        where TRole : RoleBase
        where TModule : ModuleFunctionBase
    {
        /// <summary>
        /// Adds a Module grant setting to a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="ModuleGrant">Module grant setting info</param>
        Task AddModuleFunctionAsync(TRole role, ModuleFunctionBase ModuleGrant);

        /// <summary>
        /// Removes a Module grant setting from a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="ModuleGrant">Module grant setting info</param>
        Task RemoveModuleFunctionAsync(TRole role, ModuleFunctionBase ModuleGrant);

        /// <summary>
        /// Gets Module grant setting informations for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of Module setting informations</returns>
        Task<IList<ModuleFunctionBase>> GetModulesAsync(TRole role);

        /// <summary>
        /// Gets Module grant setting informations for a role.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>List of Module setting informations</returns>
        Task<IList<ModuleFunctionBase>> GetModulesAsync(int roleId);

        /// <summary>
        /// Checks whether a role has a Module grant setting info.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="ModuleGrant">Module grant setting info</param>
        /// <returns></returns>
        Task<bool> HasModuleFunctionAsync(int roleId, ModuleFunctionBase ModuleGrant);

        /// <summary>
        /// Deleted all Module settings for a role.
        /// </summary>
        /// <param name="role">Role</param>
        Task RemoveAllModuleSettingsAsync(TRole role);
    }
}