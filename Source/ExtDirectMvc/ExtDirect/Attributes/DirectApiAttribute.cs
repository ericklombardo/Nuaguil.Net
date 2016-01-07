using System;

namespace ExtDirect.Attributes
{
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
   public class DirectApiAttribute : Attribute
   {
      public String Name { get; set; }
   }
}