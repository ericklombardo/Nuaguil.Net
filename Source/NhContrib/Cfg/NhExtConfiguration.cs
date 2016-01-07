using System;
using System.Collections.Generic;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Util;
using System.Linq;

namespace Nuaguil.NhContrib.Cfg
{
    public class NhExtConfiguration : NHibernate.Cfg.Configuration
    {

        public IList<IAuxiliaryDatabaseObject> GetAuxiliaryDatabaseObjects()
        {
            return auxiliaryDatabaseObjects;
        }

        public string[] GenerateSchemaCreationScriptAuxiliaryDatabaseObjects()
        {
            return GenerateSchemaCreationScriptAuxiliaryDatabaseObjects(x => true);
        }

        public string[] GenerateSchemaDropScriptAuxiliaryDatabaseObjects()
        {
            return GenerateSchemaDropScriptAuxiliaryDatabaseObjects(x => true);
        }

        public string[] GenerateSchemaCreationScriptAuxiliaryDatabaseObjects(Func<IAuxiliaryDatabaseObject,bool> predicate)
        {
            Dialect dialect = Dialect.GetDialect(Properties);
            IMapping mapping = BuildMapping();
            string defaultCatalog = PropertiesHelper.GetString("default_catalog", Properties, null);
            string defaultSchema = PropertiesHelper.GetString("default_schema", Properties, null);
            List<string> list = new List<string>();
            foreach (IAuxiliaryDatabaseObject obj2 in auxiliaryDatabaseObjects.Where(predicate))
            {
                if (obj2.AppliesToDialect(dialect))
                {
                    list.Add(obj2.SqlCreateString(dialect, mapping, defaultCatalog, defaultSchema));
                }
            }

            return list.ToArray();
        }

        public string[] GenerateSchemaDropScriptAuxiliaryDatabaseObjects(Func<IAuxiliaryDatabaseObject, bool> predicate)
        {
            Dialect dialect = Dialect.GetDialect(Properties);
            string defaultCatalog = PropertiesHelper.GetString("default_catalog", Properties, null);
            string defaultSchema = PropertiesHelper.GetString("default_schema", Properties, null);
            List<string> list = new List<string>();
            foreach (IAuxiliaryDatabaseObject obj2 in auxiliaryDatabaseObjects.Where(predicate))
            {
                if (obj2.AppliesToDialect(dialect))
                {
                    list.Add(obj2.SqlDropString(dialect,defaultCatalog, defaultSchema));
                }
            }

            return list.ToArray();
        }


    }
}
