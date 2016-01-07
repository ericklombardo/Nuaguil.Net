using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Spring.Context;

namespace SpringServiceLocator
{
   public class SpringServiceLocatorAdapter : ServiceLocatorImplBase
   {

      private readonly IApplicationContext _applicationContext;

      public SpringServiceLocatorAdapter(IApplicationContext applicationContext)
      {
         _applicationContext = applicationContext;
      }

      protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
      {
         return _applicationContext.GetObjectsOfType(serviceType).Values.Cast<object>();
      }

      protected override object DoGetInstance(Type serviceType, string key)
      {
         if (key == null)
         {
            return DoGetAllInstances(serviceType).FirstOrDefault();
         }

         return _applicationContext.GetObject(key, serviceType);
      }

   }
}
