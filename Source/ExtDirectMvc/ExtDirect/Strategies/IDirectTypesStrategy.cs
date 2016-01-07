using System;
using System.Reflection;
using System.Collections.Generic;

namespace ExtDirect.Strategies
{
   public interface IDirectTypesStrategy
   {
      /// <summary>
      /// Get the types  to register in the Ext.Direct api
      /// </summary>
      /// <param name="assemblyControllers">Assembly that has the controller types</param>
      /// <returns>Array of types to register</returns>
      IEnumerable<Type> GetTypes(Assembly assemblyControllers);
   }
}