//using Core.Dependency;

using System;

namespace Core.ObjectMapping
{
    public sealed class NullObjectMapper : IObjectMapper//, ISingletonDependency
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullObjectMapper Instance { get; } = new NullObjectMapper();

        public TDestination Map<TDestination>(object source)
        {
            throw new Exception("Core.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new Exception("Core.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }
    }
}
