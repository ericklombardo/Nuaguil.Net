using System;
using System.Collections.Generic;

using SpectralSpring.ModuleSupport;
using SpectralSpring.ModuleSupport.Ui;


namespace SpectralSpring.Sample.Module1
{
    public class SampleModuleOne : Module
    {

        public SampleModuleOne()
        {

        }

        public override void Initialize()
        {
            InitializeModuleContext("assembly://SpectralSpring.Sample.Module1/SpectralSpring.Sample.Module1.Config/SampleModuleOne.config");

            MainView = (ModuleView) ModuleContext.GetObject(ModuleName + "View");

            SwitchViewMenuItem menuItem = new SwitchViewMenuItem(ModuleName , MainView);
            MainMenuManager.AddMenuItem("general", 1000, menuItem);
        }

        public override string ModuleName
        {
            get { return "SampleModuleOne"; }
        }
    }
}
