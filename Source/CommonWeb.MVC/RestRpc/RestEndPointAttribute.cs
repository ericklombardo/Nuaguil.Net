namespace Common.Web.MVC.RestRpc
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class RestEndPointAttribute : Attribute
    {
        private string _infoformationUrl;
        private string _name;
        private string _provider;
        private string _version;

        public RestEndPointAttribute(string name)
        {
            this._name = name;
            this._version = "1.0";
        }

        public string InformationUrl
        {
            get
            {
                if (this._infoformationUrl == null)
                {
                    return string.Empty;
                }
                return this._infoformationUrl;
            }
            set
            {
                this._infoformationUrl = value;
            }
        }

        public string Name
        {
            get
            {
                if (this._name == null)
                {
                    return string.Empty;
                }
                return this._name;
            }
        }

        public string Provider
        {
            get
            {
                if (this._provider == null)
                {
                    return string.Empty;
                }
                return this._provider;
            }
            set
            {
                this._provider = value;
            }
        }

        public string Version
        {
            get
            {
                if (this._version == null)
                {
                    return string.Empty;
                }
                return this._version;
            }
            set
            {
                this._version = this.Version;
            }
        }
    }
}
