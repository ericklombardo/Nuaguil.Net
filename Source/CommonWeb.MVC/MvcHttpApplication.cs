using System;
using System.Web;
using Common.Logging;
using Spring.Web.Mvc;

namespace Common.Web.MVC
{
   public class MvcHttpApplication : SpringMvcApplication
   {
      protected static readonly ILog Logger = LogManager.GetLogger("MVC");

      #region HttpApplication Methods

      protected virtual void Application_Error(object sender, EventArgs e)
      {

         Exception exc = Server.GetLastError();
         string message = "Error inesperado";

         if (Logger.IsErrorEnabled)
            Logger.Error(message, exc);

         Server.ClearError();

         if (HttpContext.Current.Request.Params["requestType"] == "renderView")
         {
            string json = "{success : false, error : 'La vista no existe'}";
            string msg = String.Format("{0}({1})", HttpContext.Current.Request.QueryString["callback"], json);
            HttpContext.Current.Response.Write(msg);
            HttpContext.Current.Response.StatusCode = 200;
         }
         else
         {
            HttpContext.Current.Response.Write(message);
            HttpContext.Current.Response.StatusCode = 500;
         }

         HttpContext.Current.Response.End();

      }

      #endregion

   }
}
