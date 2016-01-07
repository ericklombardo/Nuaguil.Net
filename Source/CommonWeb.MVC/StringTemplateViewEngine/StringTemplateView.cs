using System;
using System.Web.Mvc;
using Antlr4.StringTemplate;
using System.Linq;
namespace Common.Web.MVC.StringTemplateViewEngine
{
    /// <summary>
    /// A view that renders a StringTemplate
    /// </summary>
    public class StringTemplateView : IView
    {
        #region Properties
        /// <summary>
        /// The template associated with this view
        /// </summary>
        private Template _template { get; set; }
        #endregion

        #region .ctors
        public StringTemplateView(string templateString)
        {
            //null check
            if (string.IsNullOrEmpty(templateString)) throw new ArgumentNullException("templateString");

            //set template
            _template = new Template(StringTemplateViewEngine.Group, templateString);
        }
        public StringTemplateView(Template template)
        {
            //null check
            if (template == null) throw new ArgumentNullException("template");

            //set template
            _template = template;
        }
        #endregion

        #region IView Members
        /// <summary>
        /// Renders the string template
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="writer"></param>
        public void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            //persist the controller's viewdata in the template's attribute store
           _template.Add("viewData", viewContext.Controller.ViewData.ToDictionary(x => x.Key, x => x.Value));

           _template.Add("model",viewContext.ViewData.Model);

            //render the template to the text writer
           var noIndentWriter = new NoIndentWriter(writer);
            _template.Write(noIndentWriter);
        }

        #endregion
    }
}
