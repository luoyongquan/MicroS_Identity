using Core.Dependency;
using Core.Runtime.Caching.Configuration;
using Castle.Core.Logging;

namespace Core.Runtime.Caching.Memory
{
    /// <summary>
    /// Implements <see cref="ICacheManager"/> to work with MemoryCache.
    /// </summary>
    public class MemoryCacheManager : CacheManagerBase
    {
        public ILogger Logger { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MemoryCacheManager(IIocManager iocManager, ICachingConfiguration configuration)
            : base(iocManager, configuration)
        {
            Logger = NullLogger.Instance;
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return new MemoryCache(name)
            {
                Logger = Logger
            };
        }

        protected override void DisposeCaches()
        {
            foreach (var cache in Caches.Values)
            {
                cache.Dispose();
            }
        }
    }
}
