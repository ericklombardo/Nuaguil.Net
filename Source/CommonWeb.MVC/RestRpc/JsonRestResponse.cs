using System.Globalization;
using Newtonsoft.Json;

namespace Common.Web.MVC.RestRpc
{
    using Common.Web.MVC.Serialization;
    using System;
    using System.IO;
    using System.Text;

    internal class JsonRestResponse : Common.Web.MVC.RestRpc.TextRestResponse
    {
        public JsonRestResponse(Common.Web.MVC.RestRpc.RestRequest request) : base(request)
        {
        }

        protected override void GenerateResponse(TextWriter writer, object result)
        {            
            Formatting formatting = Formatting.None; 
            JsonSerializerSettings settings = null;

            JsonSerializer serializer = JsonSerializer.Create(settings);
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
            StringWriter textWriter = new StringWriter(CultureInfo.InvariantCulture);
            using (JsonTextWriter writer2 = new JsonTextWriter(textWriter))
            {
                writer2.Formatting = formatting;
                serializer.Serialize(writer2, result);
            }

            writer.Write(textWriter.ToString());
        }
    }
}
