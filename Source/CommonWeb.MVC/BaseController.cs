using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Common.Logging;
using Common.Web.MVC.Ext;
using Common.Web.MVC.RestRpc;
using Nuaguil.Utils.DesignByContract;

namespace Common.Web.MVC
{
    public class BaseController : RestHandler, IReadOnlySessionState, IRequiresSessionState
    {
        // Fields
        private Dictionary<string, object> _viewData;
        protected static readonly ILog Logger = LogManager.GetLogger("MVC");

        // Methods
        protected void EndResponse(string message)
        {
            base.Context.Response.Write(message + "<br/>");
            base.Context.Response.StatusCode = 500;
            base.Context.Response.End();
        }

        protected void HandlerException(string message)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(message);
            }
            this.EndResponse(message);
        }

        protected void HandlerException(string message, Exception exc)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(message, exc);
            }
            this.EndResponse(message);
        }

        // Properties
        protected NameValueCollection Form
        {
            get
            {
                return base.Context.Request.Form;
            }
        }

        protected NameValueCollection QueryString
        {
            get
            {
                return base.Context.Request.QueryString;
            }
        }

        public ExtViewResponse RenderView(string mainJsClass, params string[] jsPath)
        {
            Check.Require(!String.IsNullOrEmpty(mainJsClass), "Debe establecer el nombre de la clase principal de la vista");

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < jsPath.Length; i++)
            {
                builder.Append(System.IO.File.ReadAllText(Context.Server.MapPath(jsPath[i])));
                builder.AppendLine();
            }

            Dictionary<string, object>  config = new Dictionary<string, object> { { "xtype", mainJsClass }, { "viewData", ViewData } };

            return new ExtViewResponse { Config = config, Scripts = builder.ToString() };
        }


        protected Dictionary<string, object> ViewData
        {
            get
            {
                if (this._viewData == null)
                {
                    this._viewData = new Dictionary<string, object>();
                }
                return this._viewData;
            }
        }
    }
}

