namespace Common.Web.MVC.RestRpc
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple=false)]
    public sealed class RestResourceIdentifierAttribute : Attribute
    {
    }
}
