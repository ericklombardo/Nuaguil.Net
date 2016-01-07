namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class RestType
    {
        private Common.Web.MVC.RestRpc.RestDataType _dataType;
        private Type _managedType;
        private static readonly Common.Web.MVC.RestRpc.RestType BooleanType = new Common.Web.MVC.RestRpc.RestType(Common.Web.MVC.RestRpc.RestDataType.Boolean);
        private static readonly Common.Web.MVC.RestRpc.RestType DateTimeType = new Common.Web.MVC.RestRpc.RestType(Common.Web.MVC.RestRpc.RestDataType.DateTime);
        private static readonly Common.Web.MVC.RestRpc.RestType DoubleType = new Common.Web.MVC.RestRpc.RestType(Common.Web.MVC.RestRpc.RestDataType.Double);
        private static readonly Common.Web.MVC.RestRpc.RestType IntegerType = new Common.Web.MVC.RestRpc.RestType(Common.Web.MVC.RestRpc.RestDataType.Integer);
        private static readonly Common.Web.MVC.RestRpc.RestType ObjectType = new Common.Web.MVC.RestRpc.RestType(Common.Web.MVC.RestRpc.RestDataType.Object);
        private static readonly Common.Web.MVC.RestRpc.RestType StringType = new Common.Web.MVC.RestRpc.RestType(Common.Web.MVC.RestRpc.RestDataType.String);

        private RestType(Common.Web.MVC.RestRpc.RestDataType dataType) : this(dataType, null)
        {
        }

        private RestType(Common.Web.MVC.RestRpc.RestDataType dataType, Type managedType)
        {
            this._dataType = dataType;
            this._managedType = managedType;
        }

        public static void ValidateType(Type managedType, Common.Web.MVC.RestRpc.RestEndPoint endPoint)
        {
            return;
            
            if (managedType == typeof(void))
            {
                throw new Common.Web.MVC.RestRpc.RestException("Void is not a supported type.");
            }
            if (managedType.IsEnum)
            {
                managedType = Enum.GetUnderlyingType(managedType);
            }
            if (managedType.IsArray)
            {
                managedType = managedType.GetElementType();
            }
            if (((managedType != typeof(object)) && (managedType != typeof(string))) && ((managedType != typeof(DateTime)) && !managedType.IsPrimitive))
            {
                string str = "'" + managedType.FullName + "'";
                if (managedType.IsAbstract)
                {
                    throw new Common.Web.MVC.RestRpc.RestException(str + " is not a supported type because it is abstract.");
                }
                if (managedType.IsInterface)
                {
                    throw new Common.Web.MVC.RestRpc.RestException(str + " is not a supported type because it is an interface.");
                }
                if (managedType.IsNotPublic)
                {
                    throw new Common.Web.MVC.RestRpc.RestException(str + " is not a supported type because it is not a public type.");
                }
                if (managedType.IsPointer)
                {
                    throw new Common.Web.MVC.RestRpc.RestException(str + " is not a supported type because it is a pointer type.");
                }
                if (managedType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null) == null)
                {
                    throw new Common.Web.MVC.RestRpc.RestException(str + " is not a supported type because it does not have a public parameterless constructor.");
                }
                if (typeof(IDictionary).IsAssignableFrom(managedType))
                {
                    if (managedType.IsGenericType)
                    {
                        Type[] genericArguments = managedType.GetGenericArguments();
                        if (genericArguments[0] != typeof(string))
                        {
                            throw new Common.Web.MVC.RestRpc.RestException(str + " is not a supported type because the dictionary key is not a string type.");
                        }
                        try
                        {
                            endPoint.AddType(genericArguments[1]);
                        }
                        catch
                        {
                            throw new Common.Web.MVC.RestRpc.RestException(str + " is not a supported type because the dictionary value type is not supported.");
                        }
                    }
                }
                /*
                else if (typeof(IList).IsAssignableFrom(managedType))
                {
                    if (managedType.IsGenericType)
                    {
                        try
                        {
                            Type[] typeArray2 = managedType.GetGenericArguments();
                            endPoint.AddType(typeArray2[0]);
                        }
                        catch
                        {
                            throw new Common.Web.MVC.RestRpc.RestException(str + " is not a supported type because the list item type is not supported.");
                        }
                    }
                }
                 */ 
                else
                {
                    BindingFlags bindingAttr = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance;
                    foreach (FieldInfo info2 in managedType.GetFields(bindingAttr))
                    {
                        endPoint.AddType(info2.FieldType);
                    }
                    foreach (PropertyInfo info3 in managedType.GetProperties(bindingAttr))
                    {
                        if ((info3.GetGetMethod() != null) && (info3.GetSetMethod() != null))
                        {
                            endPoint.AddType(info3.PropertyType);
                        }
                    }
                }
            }
        }

        public Common.Web.MVC.RestRpc.RestDataType DataType
        {
            get
            {
                return this._dataType;
            }
        }

        public bool IsCompositeType
        {
            get
            {
                if ((this._dataType != Common.Web.MVC.RestRpc.RestDataType.List) && (this._dataType != Common.Web.MVC.RestRpc.RestDataType.Map))
                {
                    return (this._dataType == Common.Web.MVC.RestRpc.RestDataType.Record);
                }
                return true;
            }
        }

        public Type ManagedType
        {
            get
            {
                return this._managedType;
            }
        }
    }
}
