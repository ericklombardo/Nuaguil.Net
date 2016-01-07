using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Web.MVC.StringTemplateViewEngine
{
    [Serializable]
    public class StringTemplateExtModel
    {
        // Properties
        public string JsFiles { get; set; }
        public string MainClassJs { get; set; }
        public string JsonViewData { get; set; }
    }
}
