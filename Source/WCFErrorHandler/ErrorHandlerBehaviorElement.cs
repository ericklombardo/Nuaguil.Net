using System;
using System.ServiceModel.Configuration;

namespace Nuaguil.WCFErrorHandler
{
   public class ErrorHandlerBehaviorElement : BehaviorExtensionElement
   {
      public override Type BehaviorType
      {
         get { return typeof(ErrorServiceBehavior); }
      }

      protected override object CreateBehavior()
      {
         return new ErrorServiceBehavior();
      }
   }
}
