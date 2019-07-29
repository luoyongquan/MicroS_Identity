using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Configuration.Startup;

namespace Core.Runtime.Caching.Configuration
{
    internal class CachingConfiguration : ICachingConfiguration
    {
        public IStartupConfiguration Configuration { get; private set; }

        public IReadOnlyList<ICacheConfigurator> Configurators
        {
            get { return _configurators.ToImmutableList(); }
        }
        private readonly List<ICacheConfigurator> _configurators;

        public CachingConfiguration(IStartupConfiguration Configuration)
        {
            Configuration = Configuration;

            _configurators = new List<ICacheConfigurator>();
        }

        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }

        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }
    }
}