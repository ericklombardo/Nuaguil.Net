using System;
using System.Windows.Controls;

namespace SpectralSpring.ModuleSupport.Ui
{
    public class ModuleView : UserControl
    {
        public virtual void Refresh(){}
        public virtual string ViewName { get { return "DefaultModuleViewName"; } }

    }
}
