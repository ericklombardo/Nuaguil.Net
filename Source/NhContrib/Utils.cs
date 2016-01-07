using System;
using System.Web;
using System.IO;
using System.Reflection;
using Nuaguil.Utils.DesignByContract;
using Nuaguil.NhContrib.Configuration;

namespace Nuaguil.NhContrib
{
    public static class Utils
    {

        /// <summary>
        /// Obtener la ruta del archivo de configuracion para
        /// la base de datos sapc
        /// </summary>
        /// <returns></returns>
        public static string GetDbCfg(string sessionFactoryName)
        {
            Check.Require(!string.IsNullOrEmpty(sessionFactoryName),
                "El nombre del factory no puede estar vacio");

            string path;
            
            SessionFactoryElement element = NHibernateSettings.
                    GetNHibernateSettings().SessionFactories[sessionFactoryName];

            Check.Ensure(element != null, "El factory '" + sessionFactoryName + "' no esta definido  en la colección");

            string cfgPath = element.FactoryConfigPath;

            //Si es una aplicacion web
            if (HttpContext.Current != null)
            {
                path = HttpContext.Current.Server.MapPath(cfgPath);

            }
            else
            {
                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + cfgPath;
            }

            return path;
        }

        public static string GetDefaultDbCfg()
        {
            string defaultFactory = NHibernateSettings.GetNHibernateSettings().DefaultFactory;
            Check.Ensure(!String.IsNullOrEmpty(defaultFactory), "Debe especificar el factory por omisión");

            return GetDbCfg(defaultFactory);        
        }

    }

}
