﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Reflection
{
    public class AssemblyFinder : IAssemblyFinder
    {
       // private readonly IModuleManager _moduleManager;

        //public AssemblyFinder(IModuleManager moduleManager)
        //{
        //    _moduleManager = moduleManager;
        //}
        public AssemblyFinder()
        {
        }
        public List<Assembly> GetAllAssemblies()
        {
            var assemblies = new List<Assembly>();

            //foreach (var module in _moduleManager.Modules)
            //{
            //    assemblies.Add(module.Assembly);
            //    assemblies.AddRange(module.Instance.GetAdditionalAssemblies());
            //}

            return assemblies.Distinct().ToList();
        }
    }
}