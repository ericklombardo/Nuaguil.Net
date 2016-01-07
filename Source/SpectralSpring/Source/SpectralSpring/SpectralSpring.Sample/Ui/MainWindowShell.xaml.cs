using System;
using System.Globalization;
using System.Threading;

using Common.Logging;
using Microsoft.Windows.Controls.Ribbon;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Spring.Context.Support;

using SpectralSpring.ModuleSupport.Ui;

namespace SpectralSpring.Sample.Ui
{
    /// <summary>
    /// Interaction logic for MainWindowShell.xaml
    /// </summary>
    public partial class MainWindowShell : RibbonedMainWindowShell
    {
        private static ILog log = LogManager.GetLogger(typeof(MainWindowShell));

        public MainWindowShell()
        {
            InitializeComponent();

            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
            log.Info("Starting with culture: " + culture.EnglishName);

            if (culture.TwoLetterISOLanguageName.Equals("ar"))
                this.FlowDirection = System.Windows.FlowDirection.RightToLeft;
        }

        public override RibbonApplicationMenu RibbonMainMenu 
        {
            get { return MainMenu; } 
        }
    }
}
