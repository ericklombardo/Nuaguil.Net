using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Windows.Controls.Ribbon;

namespace SpectralSpring.ModuleSupport.Ui
{
    public class RibbonedMainWindowShell : RibbonWindow
    {

        public virtual RibbonApplicationMenu RibbonMainMenu { get {return null;} }

    }
}
