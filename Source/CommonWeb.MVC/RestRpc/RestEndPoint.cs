using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;

namespace Common.Web.MVC.RestRpc
{
    internal sealed class RestEndPoint
    {
        // Fields
        private string _description;
        private static readonly Dictionary<Type, RestEndPoint> _endPointMap = new Dictionary<Type, RestEndPoint>();
        private Type _endPointType;
        private bool _isAsync;
        private RestEndPointAttribute _metaInfo;
        private string _name;
        private List<RestOperation> _operationList;
        private Dictionary<string, RestOperation> _operations;
        private Dictionary<Type, RestType> _types;

        // Methods
        private RestEndPoint(Type endPointType, bool isAsync)
        {
            this._endPointType = endPointType;
            this._operations = new Dictionary<string, RestOperation>();
            this._operationList = new List<RestOperation>();
            this._types = new Dictionary<Type, RestType>();
            this._isAsync = isAsync;
            object[] customAttributes = endPointType.GetCustomAttributes(typeof(RestEndPointAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length != 0))
            {
                this._metaInfo = (RestEndPointAttribute)customAttributes[0];
                this._name = this._metaInfo.Name;
            }
            if (string.IsNullOrEmpty(this._name))
            {
                this._name = endPointType.Name;
            }
            customAttributes = endPointType.GetCustomAttributes(typeof(RestDescriptionAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length != 0))
            {
                this._description = ((RestDescriptionAttribute)customAttributes[0]).Description;
            }
            if (this._description == null)
            {
                this._description = string.Empty;
            }
        }

        private void AddOperation(RestOperation operation)
        {
            if (this._operations.ContainsKey(operation.Name))
            {
                throw new InvalidOperationException("An overload of the method '" + operation.Method.Name + "' was already defined. Overloaded methods are not supported as RPC operations.");
            }
            try
            {
                this.AddType(operation.ReturnType);
                foreach (ParameterInfo info in operation.ParameterList)
                {
                    this.AddType(info.ParameterType);
                }
            }
            catch (Exception exception)
            {
                throw new RestException(500, "The method '" + operation.Method.Name + "' uses an unsupported type in its signature.", exception);
            }
            this._operations[operation.Name] = operation;
            this._operationList.Add(operation);
        }

        internal void AddType(Type type)
        {
            if (!this._types.ContainsKey(type))
            {
                this._types[type] = null;
                try
                {
                    RestType.ValidateType(type, this);
                }
                catch
                {
                    this._types.Remove(type);
                    throw;
                }
            }
        }

        private static RestOperation CreateAsyncOperation(Type endPointType, MethodInfo beginMethodInfo, RestMethodAttribute attribute)
        {
            MethodInfo endMethodInfo = null;
            if (beginMethodInfo.Name.StartsWith("Begin"))
            {
                string name = "End" + beginMethodInfo.Name.Substring(5);
                BindingFlags bindingAttr = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance;
                endMethodInfo = endPointType.GetMethod(name, bindingAttr);
            }
            if (endMethodInfo == null)
            {
                throw new RestException("Invalid RPC method '" + beginMethodInfo.Name + "'. RPC methods in an AsyncRestHandler must be implemented as a pair of Begin/End methods.");
            }
            Type returnType = endMethodInfo.ReturnType;
            ParameterInfo[] parameters = beginMethodInfo.GetParameters();
            bool flag = false;
            if (((beginMethodInfo.ReturnType == typeof(IAsyncResult)) && (parameters != null)) && (parameters.Length >= 2))
            {
                int length = parameters.Length;
                if ((parameters[length - 1].ParameterType == typeof(object)) && (parameters[length - 2].ParameterType == typeof(AsyncCallback)))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                throw new RestException("Invalid RPC method '" + beginMethodInfo.Name + "'. Begin RPC methods in an AsyncHandler must accept an AsyncCallback and an Object as the last two parameters, and return an IAsyncResult.");
            }
            bool flag2 = false;
            if (returnType != typeof(void))
            {
                ParameterInfo[] infoArray2 = endMethodInfo.GetParameters();
                if (((infoArray2 != null) && (infoArray2.Length == 1)) && (infoArray2[0].ParameterType == typeof(IAsyncResult)))
                {
                    flag2 = true;
                }
            }
            if (!flag2)
            {
                throw new RestException("Invalid RPC method '" + endMethodInfo.Name + "'. End RPC methods in an AsyncHandler must accept an IAsyncResult and return a value.");
            }
            List<ParameterInfo> list = null;
            int num2 = parameters.Length - 2;
            if (num2 != 0)
            {
                list = new List<ParameterInfo>();
                foreach (ParameterInfo info2 in parameters)
                {
                    if (info2.IsOut)
                    {
                        throw new RestException("Invalid RPC method '" + beginMethodInfo.Name + "'. RPC parameters cannot be marked as out or ref.");
                    }
                    list.Add(info2);
                    if (list.Count == num2)
                    {
                        break;
                    }
                }
            }
            return new RestOperation(beginMethodInfo, endMethodInfo, attribute, returnType, list);
        }

        private static RestOperation CreateOperation(Type endPointType, MethodInfo methodInfo, RestMethodAttribute attribute)
        {
            Type returnType = methodInfo.ReturnType;
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (returnType == typeof(void))
            {
                throw new RestException("Invalid RPC method '" + methodInfo.Name + "'. RPC methods must return a value.");
            }
            return new RestOperation(methodInfo, attribute, returnType, parameters);
        }

        public void GenerateMetadataResponse(HttpResponse response)
        {
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "text/xml";
            XmlTextWriter writer = new XmlTextWriter(response.OutputStream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteComment("RSDL Generation is not yet fully implemented. Once it is, it will be used to drive generation of C# proxies that can then be used by Script# code.");
            writer.WriteStartElement("rsdl", "http://projects.nikhilk.net/schemas/scriptSharp/rsdl");
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
        }

        public static RestEndPoint GetEndPoint(RestHandler endPointHandler)
        {
            Type key = endPointHandler.GetType();
            RestEndPoint point = null;
            if (!_endPointMap.TryGetValue(key, out point))
            {
                bool isAsync = endPointHandler is IHttpAsyncHandler;
                point = new RestEndPoint(key, isAsync);
                BindingFlags bindingAttr = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance;
                MethodInfo[] methods = key.GetMethods(bindingAttr);
                if ((methods != null) && (methods.Length != 0))
                {
                    foreach (MethodInfo info in methods)
                    {
                        object[] customAttributes = info.GetCustomAttributes(typeof(RestMethodAttribute), true);
                        if ((customAttributes != null) && (customAttributes.Length == 1))
                        {
                            RestMethodAttribute attribute = (RestMethodAttribute)customAttributes[0];
                            RestOperation operation = null;
                            if (!isAsync)
                            {
                                operation = CreateOperation(key, info, attribute);
                            }
                            else
                            {
                                operation = CreateAsyncOperation(key, info, attribute);
                            }
                            point.AddOperation(operation);
                        }
                    }
                }
                _endPointMap[key] = point;
            }
            return point;
        }

        // Properties
        public bool IsAsync
        {
            get
            {
                return this._isAsync;
            }
        }

        public RestOperation this[string name]
        {
            get
            {
                RestOperation operation = null;
                this._operations.TryGetValue(name, out operation);
                return operation;
            }
        }

        public ICollection<RestOperation> Operations
        {
            get
            {
                return this._operations.Values;
            }
        }
    }
}


