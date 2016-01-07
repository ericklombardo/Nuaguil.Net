using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtDirect.Attributes;

namespace ExtDirect.Strategies
{
   public class BaseAttributeStrategy : IDirectTypesStrategy
   {
      public IEnumerable<Type> GetTypes(Assembly assemblyControllers)
      {

         var query = from type in assemblyControllers.GetTypes()
                     let attrs = type.GetCustomAttributes(false)
                     where !attrs.OfType<DirectIgnoreAttribute>().Any() && 
                           attrs.OfType<DirectApiAttribute>().Any()
                     select type;
         
         return query;
      }
   }
}