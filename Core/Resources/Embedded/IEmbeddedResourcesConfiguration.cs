using System.Collections.Generic;

namespace Core.Resources.Embedded
{
    public interface IEmbeddedResourcesConfiguration
    {
        List<EmbeddedResourceSet> Sources { get; }
    }
}