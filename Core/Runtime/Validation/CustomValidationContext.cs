using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Dependency;

namespace Core.Runtime.Validation
{
    public class CustomValidationContext
    {
        /// <summary>
        /// List of validation results (errors). Add validation errors to this list.
        /// </summary>
        public List<ValidationResult> Results { get; }

        /// <summary>
        /// Can be used to resolve dependencies on validation.
        /// </summary>
        public IIocResolver IocResolver { get; }

        public CustomValidationContext(List<ValidationResult> results, IIocResolver iocResolver)
        {
            Results = results;
            IocResolver = iocResolver;
        }
    }
}