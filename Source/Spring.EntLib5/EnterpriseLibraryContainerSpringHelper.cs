using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.ServiceLocation;
using Spring.Context;

namespace Spring.EntLib5
{
   public static class EnterpriseLibraryContainerSpringHelper
   {

      public static IServiceLocator CreateContainer(IConfigurationSource configurationSource,IApplicationContext applicationContext)
      {
         IContainerConfigurator configurator = new SpringContainerConfigurator(applicationContext);
         EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
         return new SpringServiceLocatorAdapter(applicationContext);
      }

      public static IServiceLocator CreateContainer(IApplicationContext applicationContext)
      {
         return CreateContainer(ConfigurationSourceFactory.Create(), applicationContext);
      }

   }
}
