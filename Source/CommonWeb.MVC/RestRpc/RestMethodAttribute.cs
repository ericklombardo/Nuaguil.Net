namespace Common.Web.MVC.RestRpc
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public sealed class RestMethodAttribute : Attribute
    {
        private int _cacheDuration;
        private Common.Web.MVC.RestRpc.RestCacheLocation _cacheLocation;
        private Common.Web.MVC.RestRpc.RestMethodConvention _convention;
        private bool _enableCompression;
        private bool _enableContentTypeSelection;
        private string _responseContentType;
        private Common.Web.MVC.RestRpc.RestVerb _verb;

        public RestMethodAttribute() : this(Common.Web.MVC.RestRpc.RestVerb.Post)
        {
        }

        public RestMethodAttribute(Common.Web.MVC.RestRpc.RestVerb verb)
        {
            if ((verb < Common.Web.MVC.RestRpc.RestVerb.Get) || (verb > Common.Web.MVC.RestRpc.RestVerb.Delete))
            {
                throw new ArgumentOutOfRangeException("verb");
            }
            this._verb = verb;
            this._responseContentType = "text/json";
            this._enableContentTypeSelection = true;
        }

        public int CacheDuration
        {
            get
            {
                return this._cacheDuration;
            }
            set
            {
                this._cacheDuration = value;
            }
        }

        public Common.Web.MVC.RestRpc.RestCacheLocation CacheLocation
        {
            get
            {
                return this._cacheLocation;
            }
            set
            {
                this._cacheLocation = value;
            }
        }

        public Common.Web.MVC.RestRpc.RestMethodConvention Convention
        {
            get
            {
                return this._convention;
            }
            set
            {
                this._convention = value;
            }
        }

        public bool EnableCompression
        {
            get
            {
                return this._enableCompression;
            }
            set
            {
                this._enableCompression = value;
            }
        }

        public bool EnableContentTypeSelection
        {
            get
            {
                return this._enableContentTypeSelection;
            }
            set
            {
                this._enableContentTypeSelection = value;
            }
        }

        public string ResponseContentType
        {
            get
            {
                return this._responseContentType;
            }
            set
            {
                this._responseContentType = value;
            }
        }

        public Common.Web.MVC.RestRpc.RestVerb Verb
        {
            get
            {
                return this._verb;
            }
        }
    }
}
