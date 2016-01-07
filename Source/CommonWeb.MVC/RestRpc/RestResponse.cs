namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Web;

    public abstract class RestResponse
    {
        private TimeSpan _cacheDuration;
        private Common.Web.MVC.RestRpc.RestCacheLocation _cacheLocation;
        private string[] _cacheVaryParams;
        private string _eTag;
        private bool _hasExpiryInformation;
        private bool _hasModificationInformation;
        private DateTime _modifiedDate;
        private  Common.Web.MVC.RestRpc.RestRequest _request;
        protected HttpContext _context;
        private bool _unmodified;

        protected RestResponse(Common.Web.MVC.RestRpc.RestRequest request)
        {
            this._request = request;
        }

        protected abstract void GenerateResponse(Stream output, object result);
        internal void SendResponse(HttpContext context, object result)
        {
            _context = context;
            HttpResponse response = context.Response;
            response.ContentType = this._request.ContentType;
            response.ContentEncoding = Encoding.UTF8;
            response.Charset = "UTF-8";
            if (this._unmodified)
            {
                response.StatusCode = 0x130;
                response.StatusDescription = "Not Modified";
                response.AddHeader("ETag", this._request.ETag);
            }
            else
            {
                Stream outputStream = response.OutputStream;
                bool supportsCompression = this._request.SupportsCompression;
                if (supportsCompression)
                {
                    response.AddHeader("Content-encoding", "gzip");
                    outputStream = new GZipStream(outputStream, CompressionMode.Compress);
                }
                HttpCachePolicy cache = response.Cache;
                if (this._cacheLocation == Common.Web.MVC.RestRpc.RestCacheLocation.None)
                {
                    cache.SetCacheability(HttpCacheability.NoCache);
                    cache.SetNoServerCaching();
                }
                else
                {
                    if (this._hasModificationInformation)
                    {
                        cache.SetETag(this._eTag);
                        cache.SetLastModified(this._modifiedDate);
                    }
                    else
                    {
                        cache.SetLastModified(context.Timestamp);
                    }
                    switch (this._cacheLocation)
                    {
                        case Common.Web.MVC.RestRpc.RestCacheLocation.Server:
                            cache.SetCacheability(HttpCacheability.Server);
                            break;

                        case Common.Web.MVC.RestRpc.RestCacheLocation.Downstream:
                            cache.SetCacheability(HttpCacheability.Public);
                            cache.SetNoServerCaching();
                            break;

                        case Common.Web.MVC.RestRpc.RestCacheLocation.Client:
                            cache.SetCacheability(HttpCacheability.Private);
                            cache.SetNoServerCaching();
                            break;

                        case Common.Web.MVC.RestRpc.RestCacheLocation.ServerAndClient:
                            cache.SetCacheability(HttpCacheability.ServerAndPrivate);
                            break;
                    }
                    if (this._hasExpiryInformation)
                    {
                        cache.SetExpires(context.Timestamp.Add(this._cacheDuration));
                        cache.SetMaxAge(this._cacheDuration);
                        if ((this._cacheLocation == Common.Web.MVC.RestRpc.RestCacheLocation.Server) || (this._cacheLocation == Common.Web.MVC.RestRpc.RestCacheLocation.ServerAndClient))
                        {
                            cache.SetOmitVaryStar(true);
                            if (this._cacheVaryParams != null)
                            {
                                foreach (string str in this._cacheVaryParams)
                                {
                                    cache.VaryByParams[str] = true;
                                }
                            }
                            else
                            {
                                cache.VaryByParams.IgnoreParams = true;
                            }
                            if (supportsCompression)
                            {
                                cache.VaryByHeaders["Accept-encoding"] = true;
                            }
                            response.AddFileDependency(context.Server.MapPath(context.Request.CurrentExecutionFilePath));
                            cache.SetValidUntilExpires(true);
                        }
                    }
                }
                this.GenerateResponse(outputStream, result);
                if (supportsCompression)
                {
                    outputStream.Flush();
                }
            }
        }

        public void SetCacheBehavior(TimeSpan duration, string[] varyByParameters)
        {
            if (this._cacheLocation == Common.Web.MVC.RestRpc.RestCacheLocation.None)
            {
                throw new InvalidOperationException("This response has been marked as non-cacheable.");
            }
            this._cacheDuration = duration;
            if (varyByParameters != null)
            {
                this._cacheVaryParams = (string[]) varyByParameters.Clone();
            }
            this._hasExpiryInformation = true;
        }

        public void SetCacheLocation(Common.Web.MVC.RestRpc.RestCacheLocation location)
        {
            if (this._request.Operation.Verb != Common.Web.MVC.RestRpc.RestVerb.Get)
            {
                throw new InvalidOperationException("Cacheability of responses can only be changed for GET requests.");
            }
            this._cacheLocation = location;
        }

        public void SetModificationHeaders(DateTime modifiedDate, string eTag)
        {
            if (this._cacheLocation == Common.Web.MVC.RestRpc.RestCacheLocation.None)
            {
                throw new InvalidOperationException("Non-cacheable responses cannot be associated with modification information.");
            }
            this._modifiedDate = modifiedDate;
            this._eTag = eTag;
            this._hasModificationInformation = true;
        }

        public void SetUnModifiedFromPreviousResponse()
        {
            if (this._request.Operation.Verb != Common.Web.MVC.RestRpc.RestVerb.Get)
            {
                throw new InvalidOperationException("Responses can only be marked as unmodified for GET requests.");
            }
            this._unmodified = true;
        }

        public string ETag
        {
            get
            {
                if (this._eTag == null)
                {
                    return string.Empty;
                }
                return this._eTag;
            }
        }

        public bool IsUnmodified
        {
            get
            {
                return this._unmodified;
            }
        }

        public DateTime ModifiedDate
        {
            get
            {
                return this._modifiedDate;
            }
        }

        protected Common.Web.MVC.RestRpc.RestRequest Request
        {
            get
            {
                return this._request;
            }
        }
    }
}
