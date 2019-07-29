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
    public interface IStartupConfiguration
    {
        /// <summary>
        /// Reference to the IocManager.
        /// </summary>
        IocManager IocManager { get;  }
        IAuditingConfiguration Auditing { get;  }
        IEmbeddedResourcesConfiguration EmbeddedResources { get;  }
        void Initialize();
    }
}
