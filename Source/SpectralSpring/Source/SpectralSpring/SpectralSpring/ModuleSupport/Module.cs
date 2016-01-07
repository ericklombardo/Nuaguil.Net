using System;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Spring.Context.Support;
using Spring.Context;
using SpectralSpring.ModuleSupport.Ui;


namespace SpectralSpring.ModuleSupport
{
    // FIXME: explain this
    public abstract class Module : IModule
    {
        private IRegionManager manager;
        private MainMenuManager mainMenuManager;

        private IApplicationContext moduleContext;
        private ModuleView mainView;

        public abstract String ModuleName { get; }
        
        public IRegionManager RegionManager
        {
            get {
                if (manager != null)
                {
                    return manager;
                }
                else
                {
                    manager = (IRegionManager) ContextRegistry.GetContext().GetObject("IRegionManager");
                    return manager;
                }
            }
        }

        public MainMenuManager MainMenuManager
        {
            get
            {
                if (mainMenuManager != null)
                {
                    return mainMenuManager;
                }
                else
                {
                    mainMenuManager = (MainMenuManager)ContextRegistry.GetContext().GetObject("mainMenuManager");
                    return mainMenuManager;
                }
            }
        }

        public IApplicationContext ModuleContext
        {
            get
            {
                if (moduleContext != null)
                {
                    return moduleContext;
                }
                else
                {
                    throw new ModuleInitializationException(ModuleName, "Module context is not initialized, call InitializeModuleContext with a valid context path.");
                }
            }
        }

        public ModuleView MainView
        {
            get
            {
                return mainView;
            }

            protected set
            {
                this.mainView = value;
            }
        }

        protected virtual void InitializeModuleContext(string contextPath)
        {
            string[] configLocation = new string[] { contextPath };
            this.moduleContext = new XmlApplicationContext(ModuleName + "Context", true, ContextRegistry.GetContext(), configLocation);
        }

        public abstract void Initialize();
    }
}
