using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory.Support;
using Spring.Objects.Factory.Config;

using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;

namespace SpectralSpring.CompositeSupport
{
    /// <summary>
    /// Spring bootstrapper for Prism.
    /// Prepares and configures Composite dependencies with the Spring Container.
    /// Powered by coffee and "Nelly Furtado ft Timbaland - Promiscuous"
    /// </summary>
    public abstract class SpringBootstrapper : Bootstrapper
    {
        private IApplicationContext _springContainer;
        private readonly ILoggerFacade _logger;

        private bool _useDefaultConfiguration = true;

       protected SpringBootstrapper()
        {
            _logger = new TraceLogger();
        }

        public new void Run()
        {
            Run(true);
        }

        /// Run the bootstrapper process.
        public override void Run(bool runWithDefaultConfiguration)
        {
            _useDefaultConfiguration = runWithDefaultConfiguration;

            // Create the Spring container and register it with Prism
            PrepareContainer();
            
            // Configures region adaptors and behaviors
            ConfigureRegionAdapters();

            // Registers root activation exceptions
            RegisterFrameworkExceptionTypes();

            // Prepares the shell, registers RegionManager and updates the regions
            ConfigureShell();

            InitializeModules();
        }

      

        /// <summary>
        /// Creates the Spring container
        /// May be overriden to add object required before bootstrapper container configuration
        /// </summary>
        protected virtual void PrepareContainer()
        {
            CreateContainer();
            AddMandatoryObjects();
            if (_useDefaultConfiguration)
            {
                AddDefaultObjects();
            }
        }

        /// <summary>
        /// Objects required first, needed by all default objects.
        /// </summary>
        private void AddMandatoryObjects()
        {

            RegisterSingletonInstance("ILoggerFacade", _logger);

            // Identify and enumerate the available modules
            ConfigureModuleCatalog();

            IModuleCatalog catalog = GetModuleCatalog();
            if (catalog != null)
            {
                // Register the module catalog
                RegisterSingletonInstance("IModuleCatalog", catalog);
            }
            
        }

        /// <summary>
        /// Configures default objects needed by Prism.
        /// The order of these modules is very important, do not modify!
        /// </summary>
        protected virtual void AddDefaultObjects()
        {

            RegisterTypeIfMissing("IModuleInitializer", typeof(ModuleInitializer), true);
            RegisterTypeIfMissing("IModuleManager", typeof(ModuleManager), true);
            RegisterTypeIfMissing("RegionAdapterMappings", typeof(RegionAdapterMappings), true);
            RegisterTypeIfMissing("IRegionManager", typeof(RegionManager), true);
            RegisterTypeIfMissing("IEventAggregator", typeof(EventAggregator), true);
            RegisterTypeIfMissing("IRegionViewRegistry", typeof(RegionViewRegistry), true);
            RegisterTypeIfMissing("IRegionBehaviorFactory", typeof(RegionBehaviorFactory), true);

           
            RegisterTypeIfMissing("SelectorRegionAdapter", typeof(SelectorRegionAdapter), true);
            RegisterTypeIfMissing("ItemsControlRegionAdapter", typeof(ItemsControlRegionAdapter), true);
            RegisterTypeIfMissing("ContentControlRegionAdapter", typeof(ContentControlRegionAdapter), true);

            RegisterTypeIfMissing("DelayedRegionCreationBehavior", typeof(DelayedRegionCreationBehavior), true);


            RegisterTypeIfMissing("AutoPopulateRegionBehavior", typeof(AutoPopulateRegionBehavior), true);
            RegisterTypeIfMissing("BindRegionContextToDependencyObjectBehavior", typeof(BindRegionContextToDependencyObjectBehavior), true);
            RegisterTypeIfMissing("RegionActiveAwareBehavior", typeof(RegionActiveAwareBehavior), true);
            RegisterTypeIfMissing("SyncRegionContextWithHostBehavior", typeof(SyncRegionContextWithHostBehavior), true);
            RegisterTypeIfMissing("RegionManagerRegistrationBehavior", typeof(RegionManagerRegistrationBehavior), true);

            // Registers the IoC container as a Prism service locator - important
            ConfigureServiceLocator();
        }
        
        /// <summary>
        /// Registers the Spring adaptor as the default Prism ServiceLocatorProvider
        /// </summary>
        protected override void ConfigureServiceLocator()
        {
            // The Spring container must also be registered so that it will be accessible to Prism
            RegisterSingletonInstance("IServiceLocator", new SpringServiceLocatorAdapter(_springContainer));
            ServiceLocator.SetLocatorProvider(() => (IServiceLocator)_springContainer.GetObject("IServiceLocator"));
        }

        /// <summary>
        /// Configures the region adaptors and region behaviors
        /// </summary>
        private void ConfigureRegionAdapters()
        {
            // We need the container created and exposed as a ServiceLocator here
            ConfigureRegionAdapterMappings();
            ConfigureDefaultRegionBehaviors();
        }

        /// <summary>
        /// Configures region adapter mappings.
        /// All mapping types must be registered with the container first.
        /// </summary>
        /// <returns>the configured mappings</returns>
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mapper = new RegionAdapterMapper()
                         .Map<Selector, SelectorRegionAdapter>()
                         .Map<ItemsControl, ItemsControlRegionAdapter>()
                         .Map<ContentControl, ContentControlRegionAdapter>();
            return mapper.Mappings;
        }

        /// <summary>
        /// Configures region behaviors.
        /// All behavior types must be registered with the container first.
        /// </summary>
        /// <returns>the behavior factory</returns>
        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var mapper = new RegionBehaviorFactoryMapper()
                         .Map<AutoPopulateRegionBehavior>(AutoPopulateRegionBehavior.BehaviorKey)
                         .Map<BindRegionContextToDependencyObjectBehavior>(BindRegionContextToDependencyObjectBehavior.BehaviorKey)
                         .Map<RegionActiveAwareBehavior>(RegionActiveAwareBehavior.BehaviorKey)
                         .Map<SyncRegionContextWithHostBehavior>(SyncRegionContextWithHostBehavior.BehaviorKey)
                         .Map<RegionManagerRegistrationBehavior>(RegionManagerRegistrationBehavior.BehaviorKey);
            return mapper.Factory;
        }

        /// <summary>
        /// Creates and configures the Shell
        /// </summary>
        private void ConfigureShell()
        {
            Shell = CreateShell();
            if (this.Shell != null)
            {
                InitializeShell();
                RegionManager.SetRegionManager(Shell, (IRegionManager)_springContainer.GetObject("IRegionManager"));
                RegionManager.UpdateRegions();
            }
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use custom 
        /// module loading and avoid using an <seealso cref="IModuleLoader"/> and 
        /// <seealso cref="IModuleEnumerator"/>.
        /// </summary>
        protected override void InitializeModules()
        {
            SpringModuleCatalog moduleCatalog = (SpringModuleCatalog) _springContainer.GetObject("IModuleCatalog");
            moduleCatalog.RefreshModules();

            IModuleManager manager = (IModuleManager) _springContainer.GetObject("IModuleManager");
            manager.Run();
        }

        /// <summary>
        /// Gets the module enumerator for the application
        /// </summary>
        /// <returns></returns>
        protected virtual IModuleCatalog GetModuleCatalog()
        {
            if (ModuleCatalog != null)
            {
                return ModuleCatalog;
            }
            else
            {
                ConfigureModuleCatalog();
                return ModuleCatalog;
            }
        }

        /// <summary>
        /// Registers an object in the Spring Container
        /// </summary>
        /// <param name="name">the name of the new object</param>
        /// <param name="target">the Type of the new object</param>
        /// <param name="registerAsSingleton">if the new object will be singleron scoped</param>
        protected void RegisterTypeIfMissing(string name, Type target, bool registerAsSingleton)
        {
            if (_springContainer.ContainsObject(name))
            {
                return;
            }
            else
            {
                IObjectDefinitionRegistry definitionRegistry = _springContainer as IObjectDefinitionRegistry;
                var objectDefinitionFactory = new DefaultObjectDefinitionFactory();
                var builder = ObjectDefinitionBuilder.RootObjectDefinition(objectDefinitionFactory, target);

                builder.SetSingleton(registerAsSingleton);
                builder.SetAutowireMode(AutoWiringMode.Constructor);

                definitionRegistry.RegisterObjectDefinition(name, builder.ObjectDefinition);
            }
        }

        /// <summary>
        /// Registers an object instance as a singleton in the Spring container
        /// </summary>
        /// <param name="alias">the name of the singleton object/bean</param>
        /// <param name="instance">the object instance that will be registered as singleton</param>
        private void RegisterSingletonInstance(string alias, object instance)
        {
            IConfigurableApplicationContext configurableContext = _springContainer as IConfigurableApplicationContext;

            if (configurableContext != null && !_springContainer.ContainsObjectDefinition(alias))
            {
                configurableContext.ObjectFactory.RegisterSingleton(alias, instance);
            }
        }


        /// <summary>
        /// Creates the Spring container defined in App.config
        /// </summary>
        /// <returns>the application context instance</returns>
        protected virtual void CreateContainer()
        {
            _springContainer = ContextRegistry.GetContext();

            if (_springContainer == null)
            {
                throw new InvalidOperationException("Spring container cannot be null!");
            }
        }

    }
}
