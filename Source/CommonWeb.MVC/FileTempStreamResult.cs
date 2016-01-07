using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Common.Web.MVC
{
   public class FileTempStreamResult : FileStreamResult
   {

      public string TmpFileName { get; set; }
      
      public FileTempStreamResult(Stream fileStream, string contentType) : base(fileStream, contentType)
      {

      }

      protected override void WriteFile(HttpResponseBase response)
      {
         try
         {
            base.WriteFile(response);
         }
         finally
         {
            File.Delete(TmpFileName);
         }
      }
   }
}