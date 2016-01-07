using System;
using System.IO;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Common.Web.MVC
{
   public class ContentDispositionFileStreamResult : FileStreamResult
   {

      public Boolean Inline { get; set; }
      
      public ContentDispositionFileStreamResult(Stream fileStream, string contentType) 
         : base(fileStream, contentType)
      {
      }

      public override void ExecuteResult(ControllerContext context)
      {
         if (context == null)
            throw new ArgumentNullException("context");
         HttpResponseBase response = context.HttpContext.Response;
         response.ContentType = ContentType;
         if (!string.IsNullOrEmpty(FileDownloadName))
         {
            string headerValue = GetHeaderValue(FileDownloadName);
            context.HttpContext.Response.AddHeader("Content-Disposition", headerValue);
         }
         WriteFile(response);
      }

      protected string GetHeaderValue(string fileName)
      {
         try
         {
            ContentDisposition contentDisposition = new ContentDisposition
                                                       {
                                                          FileName = fileName,
                                                          Inline = Inline
                                                       };
            return contentDisposition.ToString();
         }
         catch (FormatException ex)
         {
            return "FormatException";
         }
      }

   }
}