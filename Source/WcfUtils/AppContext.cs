using System;
using System.Threading;
using System.Web;

namespace Nuaguil.Wcf.Utils
{
    public class AppContext
    {

        private AppContext()
        {
        }
        
        /// <summary>
        /// Obtener el nombre del usuario actual
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            string cuenta = string.Empty;

            if (HttpContext.Current == null)
            {
                cuenta = Thread.CurrentPrincipal.Identity.Name;
                if (String.IsNullOrEmpty(cuenta))
                {
                    cuenta = "dbo";
                }
            }
            else
                cuenta = HttpContext.Current.User.Identity.Name;

            int loc = cuenta.IndexOf("\\");
            if (loc > 0)
            {
                cuenta = cuenta.Substring(loc + 1);
            }

            return cuenta;
        }
    
    }
}
