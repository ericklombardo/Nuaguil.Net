using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Web.MVC.Ext
{
    public class JsView
    {
        public string JsClass { get; set; }
        public bool IsUx { get; set; }
        public List<JsStore> Stores { get; set; }
        public string ViewPath { get; set; }

        public const string JsExtension = ".js";
        public const string JsUIExtension = ".ui.js";

        public JsView()
        {
            Stores = new List<JsStore>();
        }

        public string GetJsPath()
        {
            string[] arrStr = JsClass.Split('.');

            string area = arrStr.Length > 2 ? String.Format("/Areas/{0}", arrStr[1]) : String.Empty;
            int startIndex = arrStr.Length > 2 ? 2 : 1;
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = startIndex; i < arrStr.Length - 1; i++)
            {
                stringBuilder.AppendFormat("/{0}", arrStr[i]);
            }
            ViewPath = String.Format("~{0}/Views{1}", area, stringBuilder.ToString());
            return String.Format("{0}/{1}", ViewPath, JsClass);
        }

        public JsView AddStore(string jsClass)
        {
            Stores.Add(new JsStore{JsClass = jsClass,IsRelativeToView = true});
            return this;
        }


    }
}
