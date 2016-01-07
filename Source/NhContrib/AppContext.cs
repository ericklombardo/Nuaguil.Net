using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using NHibernate;

namespace Nuaguil.NhContrib
{
    public class AppContext
    {

        private AppContext()
        {
        }
        
        /// <summary>
        /// Método para obtener el nombre del usuario actual
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            string cuenta;

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

            int loc = cuenta.IndexOf("\\", StringComparison.Ordinal);
            if (loc > 0)
            {
                cuenta = cuenta.Substring(loc + 1);
            }

            return cuenta;
        }

        /// <summary>
        /// Método para obtener el principal del usuario actual
        /// </summary>
        /// <returns></returns>
        public IPrincipal  GetCurrentPrincipal()
        {
            return HttpContext.Current == null ? Thread.CurrentPrincipal : HttpContext.Current.User;
        }

        /// <summary>
        /// Método para establecer el identificador del usuario en el contexto de sesión para el
        /// repositorio de datos MSSQLServer 2005 o superior
        /// </summary>
        /// <param name="session">Objeto de sesión a la base de datos</param>
        public static void SetMssqlContext(ISession session)
        {
            string cuenta = GetUserName();
            string app = HttpContext.Current == null ? "DOTNET":"ASPNET";

            string str = string.Format("DECLARE @BinVar varbinary (128)" +
                                       " SET @BinVar = CAST(N'{0}&{1}|' AS varbinary(128) ) " +
                                       " SET CONTEXT_INFO @BinVar ", app, cuenta);


            session.CreateSQLQuery(str).ExecuteUpdate();
        }
    
    }
}
