using System.Threading.Tasks;
using Core.Dependency;

namespace Core.Configuration
{
    public class VisibleSettingClientVisibilityProvider : ISettingClientVisibilityProvider
    {
        public async Task<bool> CheckVisible(IScopedIocResolver scope)
        {
            return await Task.FromResult(true);
        }
    }
}