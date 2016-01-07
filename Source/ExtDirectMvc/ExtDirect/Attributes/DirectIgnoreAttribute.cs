using System;

namespace ExtDirect.Attributes
{
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,AllowMultiple=false)]
   public class DirectIgnoreAttribute : Attribute
   {
       
   }
}