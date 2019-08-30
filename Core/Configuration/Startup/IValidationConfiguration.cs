using System;
using System.Collections.Generic;
using Core.Collections;
using Core.Runtime.Validation.Interception;

namespace Core.Configuration.Startup
{
    public interface IValidationConfiguration
    {
        List<Type> IgnoredTypes { get; }

        /// <summary>
        /// A list of method parameter validators.
        /// </summary>
        ITypeList<IMethodParameterValidator> Validators { get; }
    }
}