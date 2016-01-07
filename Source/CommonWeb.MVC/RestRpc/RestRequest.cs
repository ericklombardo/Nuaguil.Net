using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Web;

namespace Common.Web.MVC.RestRpc
{
    public class RestRequest
    {
        // Fields
        private string _contentType;
        private bool _detectUnmodifiedResponse;
        private string _eTag;
        private HttpRequest _httpRequest;
        private DateTime _ifModifiedSince;
        private RestOperation _operation;
        private Dictionary<string, object> _parsedParameters;
        private object[] _processedParameters;
        private string _scriptCallback;

        // Methods
        internal RestRequest(HttpRequest httpRequest, RestOperation operation)
        {
            this._httpRequest = httpRequest;
            this._operation = operation;
            this._parsedParameters = new Dictionary<string, object>();
            this._contentType = operation.DefaultResponseContentType;
            this._scriptCallback = _httpRequest.QueryString["callback"];
        }

        protected void AddParameter(string name, object value)
        {
            this._parsedParameters[name] = value;
        }

        private bool ClientSupportsCompression()
        {
            string str = this._httpRequest.Headers["Accept-encoding"];
            if (!string.IsNullOrEmpty(str))
            {
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    string strA = str2.Trim();
                    if (string.Compare(strA, "gzip", StringComparison.Ordinal) == 0)
                    {
                        return true;
                    }
                    if (strA.StartsWith("gzip", StringComparison.Ordinal) && ((strA[4] == ';') || (strA[4] == ' ')))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static RestRequest CreateRequest(RestEndPoint endPoint, HttpContext context)
        {
            HttpRequest httpRequest = context.Request;
            string pathInfo = httpRequest.PathInfo;
            if (string.IsNullOrEmpty(pathInfo))
            {
                return null;
            }
            string[] strArray = pathInfo.Split(new char[] { '/' });
            string str2 = strArray[strArray.Length - 1];
            RestOperation operation = endPoint[str2];
            if (operation == null)
            {
                return null;
            }
            if (!IsMatchingVerb(httpRequest.HttpMethod, operation.Verb))
            {
                return null;
            }
            RestRequest request2 = null;
            string contentType = httpRequest.ContentType;
            string[] strArray2 = contentType.Split(new char[] { ';' });
            if (!string.IsNullOrEmpty(contentType))
            {
                if (string.Compare(strArray2[0], "application/x-www-form-urlencoded", StringComparison.Ordinal) == 0)
                {
                    request2 = new FormRestRequest(httpRequest, operation);
                }
                else if ((string.Compare(strArray2[0], "text/json", StringComparison.Ordinal) == 0) || (string.Compare(contentType, "application/json", StringComparison.Ordinal) == 0))
                {
                    request2 = new JsonRestRequest(httpRequest, operation);
                }
            }
            if (request2 == null)
            {
                request2 = new RestRequest(httpRequest, operation);
            }
            request2.ParseHeaders();
            return request2;
        }

        internal RestResponse CreateResponse()
        {
            RestResponse response = null;
            if (string.Compare(this._contentType, "text/javascript", StringComparison.Ordinal) == 0)
            {
                response = new ScriptRestResponse(this, this._scriptCallback);
            }
            else if (string.Compare(this._contentType, "text/xml", StringComparison.Ordinal) == 0)
            {
                response = new XmlRestResponse(this);
            }
            else if (string.Compare(this._contentType, "text/json", StringComparison.Ordinal) == 0)
            {
                response = new JsonRestResponse(this);
            }
            else if ((string.Compare(this._contentType, "text/text", StringComparison.Ordinal) == 0) || (string.Compare(this._contentType, "text/html", StringComparison.Ordinal) == 0))
            {
                response = new TextRestResponse(this);
            }
            else if (string.Compare(this._contentType, "application/octet-stream", StringComparison.Ordinal) == 0)
            {
                response = new StreamRestResponse(this);
            }
            if ((response != null) && (this._operation.Verb == RestVerb.Get))
            {
                response.SetCacheLocation(this._operation.CacheLocation);
                if ((this._operation.CacheLocation == RestCacheLocation.None) || (this._operation.CacheDuration == 0))
                {
                    return response;
                }
                string[] array = null;
                IList<string> cacheVaryParameters = this._operation.CacheVaryParameters;
                if (cacheVaryParameters != null)
                {
                    array = new string[cacheVaryParameters.Count];
                    cacheVaryParameters.CopyTo(array, 0);
                }
                response.SetCacheBehavior(new TimeSpan(0, 0, this._operation.CacheDuration), array);
            }
            return response;
        }

        public object[] GetParameters()
        {
            if (this._processedParameters == null)
            {
                return new object[0];
            }
            return (object[])this._processedParameters.Clone();
        }

        internal object[] GetParametersInternal()
        {
            return this._processedParameters;
        }

        private static bool IsMatchingVerb(string requestVerb, RestVerb expectedVerb)
        {
            string strB = null;
            switch (expectedVerb)
            {
                case RestVerb.Get:
                    strB = "GET";
                    break;

                case RestVerb.Post:
                    strB = "POST";
                    break;

                case RestVerb.Put:
                    strB = "PUT";
                    break;

                case RestVerb.Delete:
                    strB = "DELETE";
                    break;
            }
            return (string.Compare(requestVerb, strB, StringComparison.OrdinalIgnoreCase) == 0);
        }

        protected virtual void ParseHeaders()
        {
            if (this._operation.Verb == RestVerb.Get)
            {
                string str = this._httpRequest.Headers["If-Modified-Since"];
                if (!(string.IsNullOrEmpty(str) || !DateTime.TryParse(str, out this._ifModifiedSince)))
                {
                    this._detectUnmodifiedResponse = true;
                }
                this._eTag = this._httpRequest.Headers["If-None-Match"];
                if (!string.IsNullOrEmpty(this._eTag))
                {
                    this._detectUnmodifiedResponse = true;
                }
            }
        }

        protected virtual void ParseParameters()
        {
            IDictionary<string, object> defaultParameterValues = this._operation.DefaultParameterValues;
            if (defaultParameterValues != null)
            {
                foreach (KeyValuePair<string, object> pair in defaultParameterValues)
                {
                    this.AddParameter(pair.Key, pair.Value);
                }
            }
            foreach (string str in this._httpRequest.QueryString.Keys)
            {
                this.AddParameter(str, this._httpRequest.QueryString[str]);
            }
        }

        private void ProcessParameters()
        {
            if (this._parsedParameters.ContainsKey("output"))
            {
                if (!this._operation.EnableContentTypeSelection)
                {
                    throw new RestException(500, "The service does not allow choosing an output type.");
                }
                string strA = (string)this._parsedParameters["output"];
                this._parsedParameters.Remove("output");
                if (string.Compare(strA, "json", StringComparison.Ordinal) != 0)
                {
                    if (string.Compare(strA, "xml", StringComparison.Ordinal) != 0)
                    {
                        if (string.Compare(strA, "text", StringComparison.Ordinal) != 0)
                        {
                            if (string.Compare(strA, "html", StringComparison.Ordinal) != 0)
                            {
                                throw new RestException(500, "Invalid service output requested.");
                            }
                            this._contentType = "text/html";
                        }
                        else
                        {
                            this._contentType = "text/text";
                        }
                    }
                    else
                    {
                        this._contentType = "text/xml";
                    }
                }
                else
                {
                    this._contentType = "text/json";
                    if ((this._operation.Verb == RestVerb.Get) && this._parsedParameters.ContainsKey("callback"))
                    {
                        this._scriptCallback = (string)this._parsedParameters["callback"];
                        this._contentType = "text/javascript";
                        this._parsedParameters.Remove("callback");
                    }
                }
            }
            IDictionary<string, ParameterInfo> parameters = this._operation.Parameters;
            int num = (parameters == null) ? 0 : parameters.Count;
            if (num != this._parsedParameters.Count)
            {
                throw new RestException(500, "Invalid number of parameters passed to service.");
            }
            this._processedParameters = new object[num];
            if (num != 0)
            {
                int index = 0;
                foreach (KeyValuePair<string, ParameterInfo> pair in parameters)
                {
                    string key = pair.Key;
                    if (!this._parsedParameters.ContainsKey(key))
                    {
                        throw new RestException(500, "Invalid parameters passed into service.");
                    }
                    object inputValue = this._parsedParameters[key];
                    try
                    {
                        inputValue = this.ProcessParameterValue(inputValue, pair.Value);
                    }
                    catch (Exception exception)
                    {
                        throw new RestException(500, "Invalid value passed into service for the '" + key + "' parameter.", exception);
                    }
                    this._processedParameters[index] = inputValue;
                    index++;
                }
            }
            if (num != this._processedParameters.Length)
            {
                throw new RestException(500, "Invalid number of parameters passed to service.");
            }
        }

        private object ProcessParameterValue(object inputValue, ParameterInfo parameterInfo)
        {
            if (inputValue == null)
            {
                return null;
            }
            Type c = inputValue.GetType();
            Type parameterType = parameterInfo.ParameterType;
            if (parameterType.IsAssignableFrom(c))
            {
                return inputValue;
            }
            if (inputValue == typeof(string))
            {
                TypeConverter converter = TypeDescriptor.GetConverter(parameterType);
                if ((converter != null) && converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFromInvariantString((string)inputValue);
                }
            }
            return Convert.ChangeType(inputValue, parameterType, CultureInfo.InvariantCulture);
        }

        // Properties
        internal string ContentType
        {
            get
            {
                return this._contentType;
            }
        }

        public bool DetectUnmodifiedResponse
        {
            get
            {
                return this._detectUnmodifiedResponse;
            }
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

        protected HttpRequest HttpRequest
        {
            get
            {
                return this._httpRequest;
            }
        }

        public DateTime IfModifiedSince
        {
            get
            {
                return this._ifModifiedSince;
            }
        }

        internal RestOperation Operation
        {
            get
            {
                return this._operation;
            }
        }

        public string OperationName
        {
            get
            {
                return this._operation.Name;
            }
        }

        public bool SupportsCompression
        {
            get
            {
                return (this._operation.EnableCompression && this.ClientSupportsCompression());
            }
        }
    }
}


