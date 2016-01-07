using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFChannelManager.Tests.FakeObjects
{
   // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDummyService" in both code and config file together.
   [ServiceContract]
   public interface IDummyService
   {
      [OperationContract]
      string DoWork();
      
      [OperationContract]
      string ReturnNull();

      [OperationContract]
      int ReturnInt();

      [OperationContract]
      IList<DummyDto> ReturnDtos();

   }
}
