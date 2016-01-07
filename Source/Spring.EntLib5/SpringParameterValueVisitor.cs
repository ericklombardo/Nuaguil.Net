using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace Spring.EntLib5
{
   public class SpringParameterValueVisitor : ParameterValueVisitor
   {
      public object InjectionParameter { get; private set; }

      protected override void VisitConstantParameterValue(ConstantParameterValue parameterValue)
      {
         InjectionParameter = parameterValue.Value;
      }

      protected override void VisitResolvedParameterValue(ContainerResolvedParameter parameterValue)
      {
         string name = parameterValue.Name ?? String.Format("{0}.{1}", parameterValue.Type.Name, "__default__");          
         InjectionParameter = new RuntimeObjectReference(String.Concat(name,".", parameterValue.Type.Name));
      }

      protected override void VisitEnumerableParameterValue(ContainerResolvedEnumerableParameter parameterValue)
      {
         string typeName = parameterValue.ElementType.Name;
         ManagedList listVal = new ManagedList();
         listVal.ElementTypeName = parameterValue.ElementType.AssemblyQualifiedName;
         foreach (string name in parameterValue.Names)
         {
            listVal.Add(new RuntimeObjectReference(String.Concat(name,".",typeName)));
         }
         InjectionParameter = listVal;
         
      }


   }
}
