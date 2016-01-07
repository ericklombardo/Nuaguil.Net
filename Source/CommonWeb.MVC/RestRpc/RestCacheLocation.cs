namespace Common.Web.MVC.RestRpc
{
    using System;

    public enum RestCacheLocation
    {
        None,
        Server,
        Downstream,
        Client,
        ServerAndClient
    }
}
