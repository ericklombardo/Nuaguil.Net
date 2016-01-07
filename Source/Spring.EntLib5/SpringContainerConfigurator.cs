using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Spring.Context;
using Spring.Objects;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace Spring.EntLib5
{
   public class SpringContainerConfigurator : IContainerConfigurator
   {

      private readonly IApplicationContext _applicationContext;
      private readonly IObjectDefinitionFactory _factory;

      public SpringContainerConfigurator(IApplicationContext applicationContext)
      {
         _applicationContext = applicationContext;
         _factory = new DefaultObjectDefinitionFactory();
      }

      #region IContainerConfigurator Members

      public void RegisterAll(Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource configurationSource, ITypeRegistrationsProvider rootProvider)
      {
         foreach (var registration in rootProvider.GetRegistrations(configurationSource))
         {
            register(registration);
         }

      }

      #endregion

      private void register(TypeRegistration registrationEntry)
      {
         
         AbstractObjectDefinition objDef = _factory.CreateObjectDefinition(registrationEntry.ImplementationType.AssemblyQualifiedName, null, AppDomain.CurrentDomain);
         objDef.PropertyValues = getMutablePropertyValues(registrationEntry);
         objDef.ConstructorArgumentValues = getConstructorArgumentValues(registrationEntry);
         
         IObjectFactory objectFactory = ((IConfigurableApplicationContext)_applicationContext).ObjectFactory;
         ((IObjectDefinitionRegistry)objectFactory).RegisterObjectDefinition(String.Concat(registrationEntry.Name,".",registrationEntry.ServiceType.Name), objDef);
      }

      private ConstructorArgumentValues getConstructorArgumentValues(TypeRegistration registrationEntry)
      {
         ConstructorArgumentValues constructorArgs = new ConstructorArgumentValues();
         int index = 0;
         foreach (ParameterValue constructorParameter in registrationEntry.ConstructorParameters)
         {
            constructorArgs.AddIndexedArgumentValue(index++,getInjectionParameterValue(constructorParameter));
         }
         
         return constructorArgs;
      }

      private MutablePropertyValues getMutablePropertyValues(TypeRegistration registrationEntry)
      {
         MutablePropertyValues properties = new MutablePropertyValues();
         foreach (InjectedProperty property in registrationEntry.InjectedProperties)
         {
            properties.Add(new PropertyValue(property.PropertyName,getInjectionParameterValue(property.PropertyValue)));
         }

         return properties;
      }

      private object getInjectionParameterValue(ParameterValue dependencyParameter)
      {
         var visitor = new SpringParameterValueVisitor();
         visitor.Visit(dependencyParameter);
         return visitor.InjectionParameter;
      }


   }
}
