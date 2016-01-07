namespace Common.Web.MVC.RestRpc
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=false)]
    public sealed class RestDescriptionAttribute : Attribute
    {
        private string _description;

        public RestDescriptionAttribute(string description)
        {
            this._description = description;
        }

        public string Description
        {
            get
            {
                return this._description;
            }
        }
    }
}
