using System;
using System.Windows.Media;

using Microsoft.Practices.Prism.Regions;
using Microsoft.Windows.Controls.Ribbon;
using Spring.Context.Support;

using SpectralSpring.Utils;

namespace SpectralSpring.ModuleSupport.Ui
{
    /// <summary>
    /// SwitchViewMenuItem toggles the current view selected in the MainRegion Prism Region.
    /// If the view specified is not added to the MainRegion, it will be added and activated automatically.
    /// </summary>
    public class SwitchViewMenuItem : RibbonApplicationMenuItem
    {
        private ModuleView view;

        /// <summary>
        /// Create a new menu item specifying all properties.
        /// </summary>
        /// <param name="header">The text that will be the menu's caption</param>
        /// <param name="keyTip">The menu keytip</param>
        /// <param name="iconImageSource">The menu icon</param>
        /// <param name="viewToActivate">The view to toggle</param>
        public SwitchViewMenuItem(string header, string keyTip, ImageSource iconImageSource, ModuleView viewToActivate)
        {
            ImageSource = iconImageSource;
            Header = header;
            KeyTip = keyTip;

            this.view = viewToActivate;
        }

        /// <summary>
        /// Create a new menu item specifying minimal properties.
        /// </summary>
        /// <param name="id">The menu id that will be used to retrieve the menu header and icon</param>
        /// <param name="viewToActivate">The view to toggle</param>
        public SwitchViewMenuItem(string id, ModuleView viewToActivate)
        {
            ImageSource = ResourceUtils.GetImageSourceById(id + "Icon");
            Header = ResourceUtils.GetMessage(id + "MenuHeader");

            this.view = viewToActivate;
        }
        /// <summary>
        /// Activates the view inside the MainRegion
        /// </summary>
        protected override void OnClick()
        {
            IRegionManager regionManager = (IRegionManager) ContextRegistry.GetContext().GetObject("IRegionManager");
            ModuleView loadedView = (ModuleView)regionManager.Regions["MainRegion"].GetView(view.ViewName);

            if (loadedView != null)
            {
                foreach (object viewObject in regionManager.Regions["MainRegion"].Views)
                {
                    regionManager.Regions["MainRegion"].Deactivate(viewObject);
                }

                regionManager.Regions["MainRegion"].Activate(loadedView);
                loadedView.Refresh();
            }
            else
            {
                regionManager.Regions["MainRegion"].Add(view, view.ViewName);
                regionManager.Regions["MainRegion"].Activate(view);
                view.Refresh();
            }
        }

    }
}
