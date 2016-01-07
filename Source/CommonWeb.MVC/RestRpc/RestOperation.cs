using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Web.MVC.RestRpc
{
    internal sealed class RestOperation
    {
        // Fields
        private RestMethodAttribute _attribute;
        private MethodInfo _beginMethodInfo;
        private List<string> _cacheVaryParams;
        private Dictionary<string, object> _defaultParameterValues;
        private string _description;
        private MethodInfo _endMethodInfo;
        private MethodInfo _methodInfo;
        private string _name;
        private IList<ParameterInfo> _parameterList;
        private Dictionary<string, ParameterInfo> _parameters;
        private Type _returnType;

        // Methods
        public RestOperation(MethodInfo methodInfo, RestMethodAttribute attribute, Type returnType, IList<ParameterInfo> parameters)
            : this(methodInfo.Name, attribute, returnType, parameters)
        {
            this._methodInfo = methodInfo;
        }

        private RestOperation(string name, RestMethodAttribute attribute, Type returnType, IList<ParameterInfo> parameters)
        {
            this._name = name;
            this._attribute = attribute;
            this._returnType = returnType;
            this._parameterList = parameters;
            if (parameters != null)
            {
                this._parameters = new Dictionary<string, ParameterInfo>();
                foreach (ParameterInfo info in parameters)
                {
                    this._parameters[info.Name] = info;
                }
            }
        }

        public RestOperation(MethodInfo beginMethodInfo, MethodInfo endMethodInfo, RestMethodAttribute attribute, Type returnType, IList<ParameterInfo> parameters)
            : this(beginMethodInfo.Name.Substring(5), attribute, returnType, parameters)
        {
            this._beginMethodInfo = beginMethodInfo;
            this._endMethodInfo = endMethodInfo;
        }

        public IAsyncResult BeginInvoke(RestHandler endPoint, object[] parameters, AsyncCallback callback, object context)
        {
            object[] destinationArray = null;
            int length = 0;
            if (parameters != null)
            {
                length = parameters.Length;
            }
            if (length != 0)
            {
                destinationArray = new object[length + 2];
                Array.Copy(parameters, destinationArray, length);
            }
            else
            {
                destinationArray = new object[2];
            }
            destinationArray[length] = callback;
            destinationArray[length + 1] = context;
            return (IAsyncResult)this._beginMethodInfo.Invoke(endPoint, destinationArray);
        }

        public object EndInvoke(RestHandler endPoint, IAsyncResult result)
        {
            return this._endMethodInfo.Invoke(endPoint, new object[] { result });
        }

        public object Invoke(RestHandler endPoint, object[] parameters)
        {
            return this._methodInfo.Invoke(endPoint, parameters);
        }

        // Properties
        public int CacheDuration
        {
            get
            {
                return this._attribute.CacheDuration;
            }
        }

        public RestCacheLocation CacheLocation
        {
            get
            {
                return this._attribute.CacheLocation;
            }
        }

        public IList<string> CacheVaryParameters
        {
            get
            {
                if (((this._cacheVaryParams == null) && (this._parameters != null)) && (this._parameters.Count != 0))
                {
                    foreach (ParameterInfo info in this._parameters.Values)
                    {
                        object[] customAttributes = info.GetCustomAttributes(typeof(RestVaryCacheAttribute), false);
                        if ((customAttributes != null) && (customAttributes.Length != 0))
                        {
                            if (this._cacheVaryParams == null)
                            {
                                this._cacheVaryParams = new List<string>();
                            }
                            this._cacheVaryParams.Add(info.Name);
                        }
                    }
                }
                return this._cacheVaryParams;
            }
        }

        public IDictionary<string, object> DefaultParameterValues
        {
            get
            {
                if (((this._defaultParameterValues == null) && (this._parameters != null)) && (this._parameters.Count != 0))
                {
                    foreach (ParameterInfo info in this._parameters.Values)
                    {
                        object[] customAttributes = info.GetCustomAttributes(typeof(RestDefaultValueAttribute), false);
                        if ((customAttributes != null) && (customAttributes.Length != 0))
                        {
                            if (this._defaultParameterValues == null)
                            {
                                this._defaultParameterValues = new Dictionary<string, object>();
                            }
                            this._defaultParameterValues[info.Name] = ((RestDefaultValueAttribute)customAttributes[0]).Value;
                        }
                    }
                }
                return this._defaultParameterValues;
            }
        }

        public string DefaultResponseContentType
        {
            get
            {
                return this._attribute.ResponseContentType;
            }
        }

        public string Description
        {
            get
            {
                if (this._description == null)
                {
                    object[] customAttributes = this.Method.GetCustomAttributes(typeof(RestDescriptionAttribute), true);
                    if ((customAttributes != null) && (customAttributes.Length != 0))
                    {
                        this._description = ((RestDescriptionAttribute)customAttributes[0]).Description;
                    }
                    if (this._description == null)
                    {
                        this._description = string.Empty;
                    }
                }
                return this._description;
            }
        }

        public bool EnableCompression
        {
            get
            {
                return this._attribute.EnableCompression;
            }
        }

        public bool EnableContentTypeSelection
        {
            get
            {
                return this._attribute.EnableContentTypeSelection;
            }
        }

        public MethodInfo Method
        {
            get
            {
                if (this._methodInfo != null)
                {
                    return this._methodInfo;
                }
                return this._beginMethodInfo;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public IList<ParameterInfo> ParameterList
        {
            get
            {
                return this._parameterList;
            }
        }

        public IDictionary<string, ParameterInfo> Parameters
        {
            get
            {
                return this._parameters;
            }
        }

        public Type ReturnType
        {
            get
            {
                return this._returnType;
            }
        }

        public RestVerb Verb
        {
            get
            {
                return this._attribute.Verb;
            }
        }
    }
}

