namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.IO;

    public class TextRestResponse : Common.Web.MVC.RestRpc.RestResponse
    {
        public TextRestResponse(Common.Web.MVC.RestRpc.RestRequest request) : base(request)
        {
        }

        protected sealed override void GenerateResponse(Stream output, object result)
        {
            StreamWriter writer = new StreamWriter(output);
            this.GenerateResponse(writer, result);
            writer.Flush();
        }

        protected virtual void GenerateResponse(TextWriter output, object result)
        {
            if (result != null)
            {
                output.Write(result);
            }
        }
    }
}
