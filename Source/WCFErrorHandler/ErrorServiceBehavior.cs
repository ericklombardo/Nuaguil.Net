using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Nuaguil.WCFErrorHandler
{
   public class ErrorServiceBehavior : IServiceBehavior 
   {
      #region IServiceBehavior Members

      public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
      {
         
      }

      public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
      {
         ErrorHandler handler = new ErrorHandler();
         foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
         {
            dispatcher.ErrorHandlers.Add(handler);
         }
      }

      public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
      {
         
      }

      #endregion
   }
}
