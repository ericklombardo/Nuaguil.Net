using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using Common.Web.MVC.RestRpc;

namespace Common.Web.MVC
{
    public class RestRouteHandler : IRouteHandler
    {
        // Methods
        protected virtual IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return null;//new RestHandler(requestContext);
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return null; this.GetHttpHandler(requestContext);
        }
    }

}
