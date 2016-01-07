using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;
using log4net;

namespace Nuaguil.STUtils
{
   public class Log4NetTemplateErrorListener : ITemplateErrorListener
   {

      protected static readonly ILog Logger = LogManager.GetLogger(typeof(Log4NetTemplateErrorListener));
      
      public void CompiletimeError(TemplateMessage msg)
      {
         if (Logger.IsErrorEnabled)
            Logger.ErrorFormat("CompiletimeError: " + msg, msg.Cause);
      }

      public void RuntimeError(TemplateMessage msg)
      {
         if (Logger.IsErrorEnabled)
            Logger.ErrorFormat("RuntimeError: " + msg, msg.Cause);
      }

      public void IOError(TemplateMessage msg)
      {
         if (Logger.IsErrorEnabled)
            Logger.ErrorFormat("IOError: " + msg, msg.Cause);
      }

      public void InternalError(TemplateMessage msg)
      {
         if (Logger.IsErrorEnabled)
            Logger.ErrorFormat("InternalError: " + msg, msg.Cause);
      }
   }
}
