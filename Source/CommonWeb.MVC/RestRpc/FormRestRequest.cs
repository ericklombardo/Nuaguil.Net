namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.Web;

    internal sealed class FormRestRequest : Common.Web.MVC.RestRpc.RestRequest
    {
        public FormRestRequest(HttpRequest httpRequest, Common.Web.MVC.RestRpc.RestOperation operation) : base(httpRequest, operation)
        {
        }

        protected override void ParseParameters()
        {
            base.ParseParameters();
            HttpRequest httpRequest = base.HttpRequest;
            foreach (string str in httpRequest.Form)
            {
                base.AddParameter(str, httpRequest.Form[str]);
            }
        }
    }
}
