using Common.Web.MVC.Ext;

namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.IO;

    internal sealed class ScriptRestResponse : Common.Web.MVC.RestRpc.JsonRestResponse
    {
        private string _scriptCallback;

        public ScriptRestResponse(Common.Web.MVC.RestRpc.RestRequest request, string scriptCallback) : base(request)
        {
            this._scriptCallback = scriptCallback;
        }

        protected override void GenerateResponse(TextWriter writer, object result)
        {
            ExtViewResponse viewResponse = result as ExtViewResponse;
            object config = result;

            if (viewResponse != null)
            {
                config = viewResponse.Config;
                writer.WriteLine(viewResponse.Scripts);
            }

            writer.Write(this._scriptCallback);
            writer.Write("(");
            base.GenerateResponse(writer, config);
            writer.Write(")");
        }
    }
}
