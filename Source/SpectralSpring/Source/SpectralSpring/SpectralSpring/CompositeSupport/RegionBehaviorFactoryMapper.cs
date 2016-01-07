using System;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Regions;

namespace SpectralSpring.CompositeSupport
{
    public class RegionBehaviorFactoryMapper
    {

        public RegionBehaviorFactoryMapper()
        {
            Factory = ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>();
        }

        public RegionBehaviorFactoryMapper(IRegionBehaviorFactory factory)
        {
            Factory = factory;
        }

        public IRegionBehaviorFactory Factory { get; private set; }

        public RegionBehaviorFactoryMapper Map<T>(string behaviorKey)
          where T : IRegionBehavior
        {
            if (null != Factory)
            {
                Factory.AddIfMissing(behaviorKey, typeof(T));
            }
            return this;
        }
    }
}
