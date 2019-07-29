using Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Auditing;
using Core.Resources.Embedded;

namespace Core.Configuration.Startup
{
    public class StartupConfiguration : IStartupConfiguration
    {
        /// <summary>
        /// Reference to the IocManager.
        /// </summary>
        public IocManager IocManager { get; } = IocManager.Instance;
        public IAuditingConfiguration Auditing { get; private set; }
        public IEmbeddedResourcesConfiguration EmbeddedResources { get; private set; }
        public void Initialize()
        {
            Auditing = IocManager.Resolve<IAuditingConfiguration>();
            EmbeddedResources = IocManager.Resolve<IEmbeddedResourcesConfiguration>();
        }
    }
}
