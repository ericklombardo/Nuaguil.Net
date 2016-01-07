using System;
using System.Collections;
using System.Windows;

using Common.Logging;

namespace SpectralSpring.Utils
{
    /// <summary>
    /// Merges into the current application resources all dictionaries passed by constructor.
    /// </summary>
    public class ResourcesConfigurer
    {
        public IEnumerable ResourceDefinitions { get; set; }
        ILog log = LogManager.GetLogger(typeof(ResourcesConfigurer));

        /// <summary>
        /// Merges all dictionaries in definitions
        /// </summary>
        /// <param name="definitions">a list of dictionary URI's to be merged</param>
        public ResourcesConfigurer(IEnumerable definitions)
        {
            ResourceDefinitions = definitions;
            MergeResources();
        }

        /// <summary>
        /// Merges the dictionaries in the current application resources.
        /// </summary>
        private void MergeResources()
        {
            foreach (String resource in ResourceDefinitions)
            {
                ResourceDictionary dict = new ResourceDictionary();
                Uri uri = new Uri(resource, UriKind.Relative);
                dict.Source = uri;
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }
    }
}
