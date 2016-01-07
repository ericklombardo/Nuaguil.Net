using System;
using System.Web.Mvc;
using Common.Logging;
using Nuaguil.Utils.DesignByContract;

namespace Common.Web.MVC.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomHandleErrorAttribute : FilterAttribute, IExceptionFilter
    {

        private static readonly ILog Logger = LogManager.GetLogger("MVC");

        private string _customErrorMessage;

        public string CustomErrorMessage
        {
            set { _customErrorMessage = value; }
            get { return String.IsNullOrEmpty(_customErrorMessage) ? "Error inesperado" : _customErrorMessage; }
        }

        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {
            Check.Require(filterContext != null, "filterContext es requerido");

            if (filterContext.ExceptionHandled)
              return;


            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            Exception innerException = filterContext.Exception;

            if (Logger.IsErrorEnabled)
                Logger.Error(String.Format("Error inesperado: Controller: {0} - Acción : {1}", controllerName, actionName), innerException);


            ContentResult result = new ContentResult();
            result.Content = CustomErrorMessage;
            filterContext.Result = result;
            
            
            filterContext.ExceptionHandled = true;

            if (filterContext.HttpContext.Request.QueryString["requestType"] == "renderView")
            {
               string json = "{success : false, error : 'Error al mostrar la vista'}";
               string msg = String.Format("{0}({1})", filterContext.HttpContext.Request.QueryString["callback"], json);
               filterContext.HttpContext.Response.Write(msg);
               filterContext.HttpContext.Response.StatusCode = 200;
            }
            else
            {
               filterContext.HttpContext.Response.Clear();
               filterContext.HttpContext.Response.StatusCode = 500;
               filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }

        #endregion
    }
}