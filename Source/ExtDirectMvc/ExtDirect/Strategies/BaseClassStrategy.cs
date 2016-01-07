
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using ExtDirect.Attributes;


namespace ExtDirect.Strategies
{
   public class BaseClassStrategy : IDirectTypesStrategy
   {

      public Type BaseClassType { get; set; }
      
      public IEnumerable<Type> GetTypes(Assembly assemblyControllers)
      {

         /*
          *         
         string name="";
         var baseType = requestContext.HttpContext.ApplicationInstance.GetType().BaseType;
         if (baseType != null)
         {
            name = baseType.Assembly.FullName;         
         }
         requestContext.HttpContext.Response.ContentType = "text/javascript";
         requestContext.HttpContext.Response.Write(String.Format("var a = 'Esta es una prueba';\n var asm = '{0}'", name));

          * 
          * 
          * 
          * **/

         return assemblyControllers.GetTypes().Where(t => !t.GetCustomAttributes(false).OfType<DirectIgnoreAttribute>().Any() && t.IsAssignableFrom(BaseClassType));
      }

   }
}