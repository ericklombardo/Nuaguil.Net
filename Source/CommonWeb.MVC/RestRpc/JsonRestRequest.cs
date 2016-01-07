namespace Common.Web.MVC.RestRpc
{
    using Common.Web.MVC.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Web;

    internal class JsonRestRequest : Common.Web.MVC.RestRpc.RestRequest
    {
        public JsonRestRequest(HttpRequest httpRequest, Common.Web.MVC.RestRpc.RestOperation operation) : base(httpRequest, operation)
        {
        }

        private object CoerceDictionary(IDictionary deserializedTable, Type dictionaryType)
        {
            Type expectedType = dictionaryType.GetGenericArguments()[1];
            IDictionary dictionary = (IDictionary) Activator.CreateInstance(dictionaryType);
            foreach (DictionaryEntry entry in deserializedTable)
            {
                string key = entry.Key as string;
                if (string.IsNullOrEmpty(key))
                {
                    throw new FormatException("Invalid JSON data in request body.");
                }
                object obj2 = this.CoerceValue(entry.Value, expectedType);
                dictionary[key] = obj2;
            }
            return dictionary;
        }

        private object CoerceList(IList deserializedList, Type listType)
        {
            Type elementType;
            IList list;
            bool flag = false;
            if (listType.IsGenericType)
            {
                elementType = listType.GetGenericArguments()[0];
                list = (IList) Activator.CreateInstance(listType);
            }
            else
            {
                elementType = listType.GetElementType();
                list = new ArrayList(deserializedList.Count);
                flag = true;
            }
            foreach (object obj2 in deserializedList)
            {
                list.Add(this.CoerceValue(obj2, elementType));
            }
            if (flag)
            {
                return ((ArrayList) list).ToArray(elementType);
            }
            return list;
        }

        private object CoerceObject(IDictionary data, Type objectType)
        {
            BindingFlags bindingAttr = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance;
            object obj2 = Activator.CreateInstance(objectType);
            foreach (DictionaryEntry entry in data)
            {
                string key = entry.Key as string;
                if (string.IsNullOrEmpty(key))
                {
                    throw new Common.Web.MVC.RestRpc.RestException("Invalid input data.");
                }
                Type expectedType = null;
                PropertyInfo property = null;
                FieldInfo field = null;
                property = objectType.GetProperty(key, bindingAttr);
                if (property != null)
                {
                    expectedType = property.PropertyType;
                }
                else
                {
                    field = objectType.GetField(key, bindingAttr);
                    if (field != null)
                    {
                        expectedType = field.FieldType;
                    }
                }
                if (expectedType == null)
                {
                    throw new FormatException("Invalid JSON data in request body.");
                }
                object obj3 = this.CoerceValue(entry.Value, expectedType);
                if (property != null)
                {
                    property.SetValue(obj2, obj3, null);
                }
                else
                {
                    field.SetValue(obj2, obj3);
                }
            }
            return obj2;
        }

        private object CoerceValue(object deserializedValue, Type expectedType)
        {
            if (deserializedValue == null)
            {
                if (expectedType.IsPrimitive)
                {
                    throw new FormatException("Invalid JSON data in request body.");
                }
                return null;
            }
            Type c = deserializedValue.GetType();
            if (expectedType.IsAssignableFrom(c))
            {
                return deserializedValue;
            }
            if (c == typeof(string))
            {
                TypeConverter converter = TypeDescriptor.GetConverter(expectedType);
                if ((converter != null) && converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFromInvariantString((string) deserializedValue);
                }
            }
            if (expectedType == typeof(string))
            {
                return deserializedValue.ToString();
            }
            if (expectedType.IsEnum)
            {
                expectedType = Enum.GetUnderlyingType(expectedType);
            }
            if (expectedType.IsPrimitive)
            {
                return Convert.ChangeType(deserializedValue, expectedType, CultureInfo.InvariantCulture);
            }
            if (typeof(IDictionary).IsAssignableFrom(expectedType))
            {
                IDictionary deserializedTable = deserializedValue as IDictionary;
                if (deserializedTable == null)
                {
                    throw new FormatException("Invalid JSON data in request body.");
                }
                return this.CoerceDictionary(deserializedTable, expectedType);
            }
            if (typeof(IList).IsAssignableFrom(expectedType))
            {
                IList deserializedList = deserializedValue as IList;
                if (deserializedList == null)
                {
                    throw new FormatException("Invalid JSON data in request body.");
                }
                return this.CoerceList(deserializedList, expectedType);
            }
            if (!(deserializedValue is IDictionary))
            {
                throw new FormatException("Invalid JSON data in request body.");
            }
            return this.CoerceObject((IDictionary) deserializedValue, expectedType);
        }

        private void DeserializeParameters()
        {
            Hashtable hashtable = null;
            Common.Web.MVC.Serialization.JsonReader reader = null;
            Exception innerException = null;
            try
            {
                StreamReader reader2 = new StreamReader(base.HttpRequest.InputStream);
                reader = new Common.Web.MVC.Serialization.JsonReader(reader2);
                hashtable = reader.ReadValue() as Hashtable;
            }
            catch (Exception exception2)
            {
                innerException = exception2;
            }
            if (hashtable == null)
            {
                throw new Common.Web.MVC.RestRpc.RestException(500, "Invalid JSON in request body. The parameters must be packaged as name/value pairs in a single JSON object.", innerException);
            }
            if ((hashtable != null) && (hashtable.Count != 0))
            {
                IDictionary<string, ParameterInfo> parameters = base.Operation.Parameters;
                foreach (DictionaryEntry entry in hashtable)
                {
                    string key = (string) entry.Key;
                    object deserializedValue = entry.Value;
                    ParameterInfo info = null;
                    if (parameters.TryGetValue(key, out info))
                    {
                        if ((deserializedValue == null) && info.ParameterType.IsValueType)
                        {
                            continue;
                        }
                        if (deserializedValue != null)
                        {
                            deserializedValue = this.CoerceValue(deserializedValue, info.ParameterType);
                        }
                    }
                    base.AddParameter(key, deserializedValue);
                }
            }
        }

        protected override void ParseParameters()
        {
            base.ParseParameters();
            if ((base.Operation.Verb != Common.Web.MVC.RestRpc.RestVerb.Get) && (base.HttpRequest.ContentLength != 0))
            {
                this.DeserializeParameters();
            }
        }
    }
}
