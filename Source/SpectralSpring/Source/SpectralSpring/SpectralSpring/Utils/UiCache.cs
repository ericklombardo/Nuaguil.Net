using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Common.Logging;

namespace SpectralSpring.Utils
{
    // FIXME: explain this
    class UiCache
    {
        private Dictionary<string, UserControl> uiCache = new Dictionary<string,UserControl>();
        private static ILog logger = LogManager.GetLogger(typeof(UiCache));

        public void Add(string key, UserControl control )
        {
            if (uiCache.ContainsKey(key))
            {
                logger.Info("Replacing usercontrol with key " + key + " in the uiCache");
                uiCache.Remove(key);
            }

            uiCache.Add(key, control);
        }

        public UserControl Get(string key)
        {
            if (uiCache.ContainsKey(key))
                return uiCache[key];
            else return null;
        }

    }
}
