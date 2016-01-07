using System;
using System.Collections.Generic;

using Microsoft.Windows.Controls.Ribbon;

namespace SpectralSpring.ModuleSupport.Ui
{

    /// <summary>
    /// Manages main application level menu entries. 
    /// This class is used by application modules to add their own menus.
    /// </summary>
    public class MainMenuManager
    {
        public RibbonedMainWindowShell ShellWindow { get; set; }
        private SortedDictionary<string, IList<RibbonApplicationMenuItem>> menuGroups = new SortedDictionary<string, IList<RibbonApplicationMenuItem>>();

        /// <summary>
        /// Adds a new menu item to the menu stack.
        /// </summary>
        /// <param name="group">The group that this item is part of. Groups are separated by menu separators.</param>
        /// <param name="weight">THe bigger the weight, the lower it gets in the group.</param>
        /// <param name="menuItem">The menu item to be added.</param>
        public void AddMenuItem(string group, int weight, RibbonApplicationMenuItem menuItem)
        {
            if (menuGroups.ContainsKey(group))
            {
                menuGroups[group].Add(menuItem);
            }
            else
            {
                IList<RibbonApplicationMenuItem> items = new List<RibbonApplicationMenuItem>();
                items.Add(menuItem);
                menuGroups.Add(group, items);
            }

            RefreshMenu();
        }

        /// <summary>
        /// Renders the menu.
        /// </summary>
        private void RefreshMenu()
        {
            RibbonApplicationMenu MainMenu = ShellWindow.RibbonMainMenu as RibbonApplicationMenu;

            if (MainMenu != null)
            {
                MainMenu.Items.Clear();

                bool firstGroup = true;

                foreach (string group in menuGroups.Keys)
                {
                    IList<RibbonApplicationMenuItem> items = menuGroups[group];

                    if (!firstGroup)
                        MainMenu.Items.Add(new RibbonSeparator());

                    foreach (RibbonApplicationMenuItem item in items)
                    {
                        MainMenu.Items.Add(item);
                    }

                    firstGroup = false;
                }
            }

        }
    }
}
