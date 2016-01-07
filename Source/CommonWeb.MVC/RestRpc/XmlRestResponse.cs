namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.IO;

    internal sealed class XmlRestResponse : Common.Web.MVC.RestRpc.TextRestResponse
    {
        public XmlRestResponse(Common.Web.MVC.RestRpc.RestRequest request) : base(request)
        {
        }

        protected override void GenerateResponse(TextWriter output, object result)
        {
            throw new NotImplementedException();
        }
    }
}
