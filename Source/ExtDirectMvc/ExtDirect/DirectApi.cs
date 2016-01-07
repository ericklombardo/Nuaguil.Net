using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExtDirect.Attributes;
using ExtDirect.Model;
using ExtDirect.Strategies;
using System.Linq;

namespace ExtDirect
{
   public class DirectApi
   {

      public Type BaseClassType { get; set; }
      public StrategyType StrategyType { get; set; }
      public Assembly AssemblyControllers { get; set; }
      public string AppPath { get; set; }

      protected const string ProviderType = "remoting";

      public IEnumerable<DirectProvider> GetProviders()
      {
         
         IList<DirectProvider> providers = new List<DirectProvider>();
         
         IDirectTypesStrategy directTypesStrategy = CreateStrategy();

         foreach (Type controller in directTypesStrategy.GetTypes(AssemblyControllers))
         {
            IEnumerable<MethodInfo> methods = controller.GetMethods(BindingFlags.Public)
               .Where(x => !x.GetCustomAttributes(false).OfType<DirectIgnoreAttribute>().Any());

            foreach (MethodInfo methodInfo in methods)
            {
               
            }
         }

         return providers;
      }

      private string GetUrl(MethodInfo action)
      {
         string actionName = GetMemberName(action);
         string controllerName = GetMemberName(action.DeclaringType);

         return String.Format("{0}/{1}/{2}", AppPath, controllerName, actionName);
      }

      private String GetMemberName (MemberInfo memberInfo)
      {
         DirectApiAttribute directApiAttribute =
            memberInfo.GetCustomAttributes(false).OfType<DirectApiAttribute>().FirstOrDefault();

         return directApiAttribute == null ? memberInfo.Name : directApiAttribute.Name;
      }
      
      private  IDirectTypesStrategy CreateStrategy()
      {
         switch (StrategyType)
         {
            case StrategyType.BaseClass:
               return new BaseClassStrategy(){BaseClassType=BaseClassType};
            case StrategyType.Attribute:
               return new BaseAttributeStrategy();
            default:
               throw new ArgumentOutOfRangeException();
         }
      }  

   }
}