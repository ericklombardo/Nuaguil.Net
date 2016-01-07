using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using Spring.Data.NHibernate;

namespace Nuaguil.SpringExt.Data.NHibernate
{
    public class CustomLocalSessionFactoryObject :  LocalSessionFactoryObject
    {
        private HbmMapping[] _hbmMappings;

        public HbmMapping[] HbmMappings
        {
            set { _hbmMappings = value; }
        }

        protected override void PostProcessMappings(Configuration config)
        {
            if (_hbmMappings != null)
            {
                foreach (HbmMapping hbmMapping in _hbmMappings)
                {
                    config.AddMapping(hbmMapping);
                }
            }
        }
         
    }
}