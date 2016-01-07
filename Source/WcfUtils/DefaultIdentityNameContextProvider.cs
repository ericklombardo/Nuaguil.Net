using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Nuaguil.Wcf.Utils
{
   public class DefaultIdentityNameContextProvider : IIdentityNameContextProvider
   {

      public MessageHeader<String> GetData()
      {
         return new MessageHeader<string>(AppContext.GetUserName());
      }

   }
}
