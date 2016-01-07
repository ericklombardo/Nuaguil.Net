using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;

using SpectralSpring.ModuleSupport;

namespace SpectralSpring.CompositeSupport
{
    public class SpringModuleCatalog : ModuleCatalog
    {
        /// <summary>
        /// Read all of the objects that are configured in the spring context with Module type and
        /// add them to the module catalog.
        /// </summary>
        public void RefreshModules()
        {
            IEnumerable<Module> modules = ServiceLocator.Current.GetAllInstances<Module>();
            foreach (Module module in modules)
            {
                this.AddModule(module.GetType());
            }
        }
    }
}
