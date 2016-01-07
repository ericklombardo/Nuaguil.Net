using System;

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace SpectralSpring.CompositeSupport
{
    public class RegionAdapterMapper
    {

        public RegionAdapterMapper(RegionAdapterMappings mappings)
        {
            Mappings = mappings;
        }

        public RegionAdapterMapper(): this(null)
        {
            Mappings = ServiceLocator.Current.GetInstance<RegionAdapterMappings>();
        }

        public RegionAdapterMappings Mappings { get; private set; }

        public RegionAdapterMapper Map<TControl, TAdapter>() where TAdapter : class, IRegionAdapter
        {
            if (null != Mappings)
            {
                Mappings.RegisterMapping(typeof(TControl), ServiceLocator.Current.GetInstance<TAdapter>());
            }
            return this;
        }

    }
}
