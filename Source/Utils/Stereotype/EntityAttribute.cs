using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuaguil.Utils.Stereotype
{

   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
   [Serializable]
   public class EntityAttribute : Attribute
   {
      public String Name { get; set; }
   }
}
