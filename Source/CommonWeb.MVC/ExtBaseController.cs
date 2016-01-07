using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Common.Logging;
using Common.Web.MVC.Ext;
using Common.Web.MVC.StringTemplateViewEngine;
using Newtonsoft.Json;
using Nuaguil.Utils.DesignByContract;
using Nuaguil.Utils.Model.Dto;


namespace Common.Web.MVC
{
    public abstract class ExtBaseController : Controller
    {

      protected static readonly ILog Logger = LogManager.GetLogger("MVC");


      #region ExtView Render

      public JavaScriptResult ExtView(string mainXType,params string[] jsViews)
      {
         JsViewCollection jsViewCollection = ViewHelper.Create(mainXType);

         foreach (string jsView in jsViews)
         {
               jsViewCollection.Add(jsView);
         }

         return ExtView(jsViewCollection);
      }

      public JavaScriptResult ExtView(string mainXType, string jsViews)
      {
         StringBuilder builder = new StringBuilder(jsViews);
         ViewData["xtype"] = mainXType;

         AddJsonP(builder);

         //Response.ContentType = "text/javascript";
         //Response.ContentEncoding = new UnicodeEncoding(false, false, false);
         return JavaScript(builder.ToString()); //View(new  StringTemplateView(builder.ToString()));
      }

      public JavaScriptResult ExtView(JsViewCollection jsViewCollection)
      {
         Check.Require(!String.IsNullOrEmpty(jsViewCollection.MainViewXType), "Debe establecer el nombre de la clase principal de la vista");
         Check.Require(jsViewCollection.Count > 0, "Debe espeficar al menos una vista");
         //string area = RouteData.DataTokens.ContainsKey("area")?String.Format("Areas/{0}/",RouteData.DataTokens["area"]):string.Empty;
         
         StringBuilder builder = new StringBuilder();
         foreach (JsView t in jsViewCollection)
         {
               string path = t.GetJsPath();
               string jsFile = Server.MapPath(path + JsView.JsExtension);
               string jsUiFile = Server.MapPath(path + JsView.JsUIExtension);
               if (!t.IsUx)
               {
                  Check.Ensure(System.IO.File.Exists(jsUiFile),String.Format("La vista {0} no existe",jsUiFile));
                  builder.Append(System.IO.File.ReadAllText(jsUiFile));
                  builder.AppendLine();
               }
               Check.Ensure(System.IO.File.Exists(jsFile), String.Format("La vista {0} no existe", jsFile));
               builder.Append(System.IO.File.ReadAllText(jsFile));
               builder.AppendLine();
               foreach (JsStore store in t.Stores)
               {
                  string storeFile = Server.MapPath(String.Format("{0}/{1}{2}", t.ViewPath, store.JsClass, JsView.JsExtension));
                  Check.Ensure(System.IO.File.Exists(storeFile), String.Format("El archivo store {0} no existe", storeFile));
                  builder.Append(System.IO.File.ReadAllText(storeFile));
               }

         }

         ViewData["xtype"] = jsViewCollection.MainViewXType;
            
         AddJsonP(builder);

         //Response.ContentType = "text/javascript";
         //Response.ContentEncoding = new UnicodeEncoding(false, false, false);
         return JavaScript(builder.ToString());//View(new StringTemplateView(builder.ToString()));
      }

      #endregion

      protected String FormatModelErrors()
      {
         StringBuilder stringBuilder = new StringBuilder("<ul>");
         foreach (KeyValuePair<string, ModelState> keyValuePair in ModelState)
         {
            foreach (ModelError modelError in keyValuePair.Value.Errors)
            {
               stringBuilder.AppendFormat("<li>{0}</li>", modelError.ErrorMessage);
            }
         }
         stringBuilder.Append("</ul>");
         return stringBuilder.ToString();
      }


      protected FileResult File(Stream fileStream, string tmpFileName, string contentType, string fileDownloadName)
      {
         FileTempStreamResult result = new FileTempStreamResult(fileStream, contentType);
         result.TmpFileName = tmpFileName;
         result.FileDownloadName = fileDownloadName;
         return result;
      }
       
      protected FileStreamResult ContentDispositionFile(Stream fileStream, string contentType, string fileDownloadName = null, Boolean inLine = false)
      {
         ContentDispositionFileStreamResult fileStreamResult = new ContentDispositionFileStreamResult(fileStream, contentType);
         fileStreamResult.FileDownloadName = fileDownloadName;
         fileStreamResult.Inline = inLine;
         return fileStreamResult;
      }


      #region ExtMethods 

      protected JsonResult ExtAjaxResponse(bool success, string invalidMessage, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
      {
         return Json(new { success, invalidMessage },behavior);
      }

      protected JsonResult ExtAjaxResponse(bool success, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
      {
         return ExtAjaxResponse(success, String.Empty,behavior);
      }

      protected JsonResult ExtJsonStore<T>(IList<T> data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
      {
         return Json(data == null ? null : new { data, total = data.Count },behavior);
      }

      protected JsonResult ExtJsonStore<T>(PagedResultDto<T> dto, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
      {
         return Json(dto == null ? null : new { data = dto.Rows, total = dto.Total },behavior);
      }

      protected JsonResult ExtFormLoad(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
      {
         return Json(data == null ? null : new {data, success = true},behavior);
      }

      protected JsonResult ExtFormSubmit(object data,bool success = true ,JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
      {
         return Json(data == null ? null : new { data, success, errors = default(object) },behavior);
      }

      protected JsonResult ExtFormDelete(bool success, string errorMessage, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
      {
         return Json(new {success, errorMessage},behavior);
      }

      protected JsonResult ExtFormDelete(bool success, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
      {
         return ExtFormDelete(success, String.Empty,behavior);
      }

      #endregion

      #region Overrides Json Methods (Use Json.Net)

      protected new JsonResult Json(object data)
      {
         return Json(data, null, null, JsonRequestBehavior.DenyGet);
      }

      protected new JsonResult Json(object data, string contentType)
      {
         return Json(data, contentType, null, JsonRequestBehavior.DenyGet);
      }

      protected new JsonResult Json(object data, string contentType, Encoding contentEncoding)
      {
         return Json(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
      }

      protected new  JsonResult Json(object data, JsonRequestBehavior behavior)
      {
         return Json(data, null, null, behavior);
      }

      protected new  JsonResult Json(object data, string contentType, JsonRequestBehavior behavior)
      {
         return Json(data, contentType, null, behavior);
      }

      protected new  JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
      {
         return new JsonNetResult()
         {
            Data = data,
            ContentType = contentType,
            ContentEncoding = contentEncoding,
            JsonRequestBehavior = behavior
         };
      }

      #endregion

      #region Helper Methods

       private void AddJsonP(StringBuilder builder)
       {
         ViewData["success"] = true;
         string json = JsonConvert.SerializeObject(ViewData.ToDictionary(x => x.Key, x => x.Value));
         builder.AppendLine();
         builder.AppendFormat("{0}({1})", Request.QueryString["callback"], json);
       }

      private bool IsRelativeToDefaultPath(string file)
      {
         return !Regex.IsMatch(file, @"^(~|\.\./|/|https?://)");
      }

       #endregion

    }
}
