using Castle.Core;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.SubSystems.Conversion;

namespace Core.Dependency
{
    public class CorePropertiesDependenciesModelInspector : PropertiesDependenciesModelInspector
    {
        public CorePropertiesDependenciesModelInspector(IConversionManager converter) 
            : base(converter)
        {
        }

        protected override void InspectProperties(ComponentModel model)
        {
            if (model.Implementation.FullName != null && 
                model.Implementation.FullName.StartsWith("Microsoft"))
            {
                return;
            }

            base.InspectProperties(model);
        }
    }
}
