using System;
using System.Collections;
using System.Threading;
using System.Globalization;
using System.Windows;

using Common.Logging;

namespace SpectralSpring.Utils
{
    // FIXME: explain this
    public class MessageResourcesConfigurer
    {
        public IEnumerable Bundles { get; set; }
        ILog log = LogManager.GetLogger(typeof(MessageResourcesConfigurer));

        public MessageResourcesConfigurer(IEnumerable bundles)
        {

            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
            string code = culture.TwoLetterISOLanguageName;

            if (code.Equals("en"))
                code = "";
            else
                code = "." + code;

            foreach (string resource in bundles)
            {
                MergeResources(resource + code + ".xaml", resource + ".xaml");
            }

        }


        /// <summary>
        /// Merges the dictionaries in the current application resources.
        /// </summary>
        private void MergeResources(string baseBundle, string alternateBundle)
        {
            try
            {
                ResourceDictionary dict = new ResourceDictionary();
                Uri uri = new Uri(baseBundle, UriKind.Relative);
                dict.Source = uri;
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch (Exception e)
            {
                if (log.IsDebugEnabled)
                    log.Debug("Cannot load message bundle " + baseBundle, e);

                if(alternateBundle!=null)
                    MergeResources(alternateBundle, null);

            }
        }
    }
}
