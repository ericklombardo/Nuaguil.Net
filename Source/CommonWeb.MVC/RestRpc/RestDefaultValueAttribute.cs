namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple=false)]
    public sealed class RestDefaultValueAttribute : Attribute
    {
        private object _value;

        public RestDefaultValueAttribute(bool value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(byte value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(char value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(double value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(short value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(int value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(long value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(object value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(float value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(string value)
        {
            this._value = value;
        }

        public RestDefaultValueAttribute(Type type, string value)
        {
            try
            {
                this._value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
            }
            catch
            {
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DefaultValueAttribute attribute = obj as DefaultValueAttribute;
            if (attribute == null)
            {
                return false;
            }
            if (this.Value != null)
            {
                return this.Value.Equals(attribute.Value);
            }
            return (attribute.Value == null);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Value
        {
            get
            {
                return this._value;
            }
        }
    }
}
