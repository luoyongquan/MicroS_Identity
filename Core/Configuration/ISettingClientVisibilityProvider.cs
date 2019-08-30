using System.Threading.Tasks;
using Core.Dependency;

namespace Core.Configuration
{
    public interface ISettingClientVisibilityProvider
    {
        Task<bool> CheckVisible(IScopedIocResolver scope);
    }
}