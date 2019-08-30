using System.Threading.Tasks;
using Core.Dependency;

namespace Core.Configuration
{
    public class HiddenSettingClientVisibilityProvider : ISettingClientVisibilityProvider
    {
        public async Task<bool> CheckVisible(IScopedIocResolver scope)
        {
            return await Task.FromResult(false);
        }
    }
}