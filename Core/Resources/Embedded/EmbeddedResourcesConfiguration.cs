﻿using System.Collections.Generic;

namespace Core.Resources.Embedded
{
    public class EmbeddedResourcesConfiguration : IEmbeddedResourcesConfiguration
    {
        public List<EmbeddedResourceSet> Sources { get; }

        public EmbeddedResourcesConfiguration()
        {
            Sources = new List<EmbeddedResourceSet>();
        }
    }
}