using System;

namespace WCFChannelManager.Tests.FakeObjects
{
   public class DummyProxyChannel : ProxyChannel<IDummyService>
   {
      public DummyProxyChannel(IChannelManager<IDummyService> channelManager) 
         : base(channelManager)
      {
      }

      public new object Execute(string methodName, object[] parameters, Type[] parameterTypes)
      {
         return base.Execute(methodName, parameters, parameterTypes);
      }
       
   }
}