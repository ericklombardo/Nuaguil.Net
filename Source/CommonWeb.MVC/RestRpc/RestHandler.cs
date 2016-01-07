using System;
using System.Web;

namespace Common.Web.MVC.RestRpc
{
    public abstract class RestHandler : IHttpHandler
    {
        // Fields
        private HttpContext _context;
        private RestEndPoint _endPoint;
        private RestRequest _request;
        private RestResponse _response;

        // Methods
        protected RestHandler()
        {
            this._endPoint = RestEndPoint.GetEndPoint(this);
        }

        protected virtual void Dispose()
        {
            this._context = null;
        }

        internal bool Initialize(HttpContext context)
        {
            this._context = context;
            if (this.TryProcessMetadataRequest())
            {
                return false;
            }
            this.OnInitializing(EventArgs.Empty);
            this._request = RestRequest.CreateRequest(this._endPoint, this._context);
            if (this._request == null)
            {
                throw new RestException(0x194, "El end point no se encuentra.");
            }
            this._response = this._request.CreateResponse();
            if (this._response == null)
            {
                throw new RestException(500, "El end point no provee un tipo de salida valido.");
            }
            this.OnInitialized(EventArgs.Empty);
            if (this._response.IsUnmodified)
            {
                this._response.SendResponse(context, null);
                return false;
            }
            return true;
        }

        private object InvokeMethod()
        {
            this.OnMethodInvoking(EventArgs.Empty);
            object obj2 = this._request.Operation.Invoke(this, this._request.GetParametersInternal());
            this.OnMethodInvoked(EventArgs.Empty);
            return obj2;
        }

        protected virtual void OnInitialized(EventArgs e)
        {
        }

        protected virtual void OnInitializing(EventArgs e)
        {
        }

        protected virtual void OnMethodInvoked(EventArgs e)
        {
        }

        protected virtual void OnMethodInvoking(EventArgs e)
        {
        }

        protected virtual void OnResponseSending(EventArgs e)
        {
        }

        protected virtual void OnResponseSent(EventArgs e)
        {
        }

        internal void SendResponse(object result)
        {
            this.OnResponseSending(EventArgs.Empty);
            this._response.SendResponse(this._context, result);
            this.OnResponseSent(EventArgs.Empty);
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            try
            {
                if (this.Initialize(context))
                {
                    object result = this.InvokeMethod();
                    this.SendResponse(result);
                }
            }
            catch (HttpException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new HttpException(500, "Error al procesar la petici\x00f3n", exception);
            }
            finally
            {
                this.Dispose();
            }
        }

        internal bool TryProcessMetadataRequest()
        {
            HttpRequest request = this._context.Request;
            HttpResponse response = this._context.Response;
            if (!request.HttpMethod.Equals("GET", StringComparison.Ordinal))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(request.PathInfo))
            {
                return false;
            }
            this._endPoint.GenerateMetadataResponse(response);
            return true;
        }

        // Properties
        protected HttpContext Context
        {
            get
            {
                if (this._context == null)
                {
                    throw new ObjectDisposedException("RestRpcHandler");
                }
                return this._context;
            }
        }

        protected RestRequest Request
        {
            get
            {
                return this._request;
            }
        }

        protected RestResponse Response
        {
            get
            {
                return this._response;
            }
        }

        bool IHttpHandler.IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

