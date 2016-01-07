using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Common.Web.MVC.StringTemplateViewEngine;
using Newtonsoft.Json;

namespace Common.Web.MVC.Ext
{
    /// <summary>
    /// Clase estática para crear Vistas ExtJs creadas con  el ExtDesigner y sin este;
    /// y enviar el contenido de los Js al browser
    /// </summary>
    public static class ViewHelper
    {
        
        /// <summary>
        /// Método para crear una colleción de vistas
        /// </summary>
        /// <param name="xtype">Nombre del xtype de la vista principal de la colección</param>
        /// <returns>Colección de vistas</returns>
        public static JsViewCollection Create(string xtype)
        {
            return new JsViewCollection(xtype);
        }
        /// <summary>
        /// Método para crear una vista
        /// </summary>
        /// <param name="jsClass">Nombre de la clase de la vista</param>
        /// <returns></returns>
        public static JsView CreateView(string jsClass)
        {
            return new JsView {JsClass = jsClass};
        }

        /// <summary>
        /// Método para crear una vista que no tiene asociado un archivo .ui
        /// Se utiliza para crear vistas que no fueron creadas con ExtDesigner
        /// </summary>
        /// <param name="jsClass">Nombre de la clase de la vista</param>
        /// <returns></returns>
        public static JsView CreateUxView(string jsClass)
        {
            return new JsView { JsClass = jsClass,IsUx = true };
        }
        
        /// <summary>
        /// Método para crear un resultado de la colección de vista, para
        /// poder enviar el contenido de los archivos Js asociados a las vista al navegador
        /// </summary>
        /// <param name="jsViewCollection">Colección de vistas</param>
        /// <param name="controller">Controller encargado de enviar el resultado al navegador</param>
        /// <returns></returns>
        public static JavaScriptResult View(this JsViewCollection jsViewCollection,ExtBaseController controller)
        {
            return controller.ExtView(jsViewCollection);
        }

    }
}
