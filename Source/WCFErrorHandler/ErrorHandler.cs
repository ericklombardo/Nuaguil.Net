using System;
using System.ServiceModel.Dispatcher;
using Common.Logging;

namespace Nuaguil.WCFErrorHandler
{
   public class ErrorHandler : IErrorHandler 
   {

      protected static readonly ILog Logger = LogManager.GetLogger(typeof(ErrorHandler));

      #region IErrorHandler Members

      public bool HandleError(Exception ex)
      {
         if (Logger.IsErrorEnabled)
            Logger.Error("Ocurrio un error en el servicio", ex);

         return false;
      }

      public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
      {
         
      }

      #endregion
   }
}
