using System;
using System.Collections.Generic;
using System.Windows;

using Spring.Context.Support;

using SpectralSpring.CompositeSupport;
using SpectralSpring.Sample.Ui;

namespace SpectralSpring.Sample
{
    class SpectralSpringSampleBootstrapper : SpringBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return (DependencyObject)ContextRegistry.GetContext().GetObject("shellWindow");
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (MainWindowShell)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            SpringModuleCatalog catalog = new SpringModuleCatalog();
            this.ModuleCatalog = catalog;
        }

    }
}
