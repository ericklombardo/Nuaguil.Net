using System;
using System.ServiceModel;

namespace Nuaguil.Wcf.Utils
{
   public interface IIdentityNameContextProvider
    {
      MessageHeader<String> GetData();
    }
}
