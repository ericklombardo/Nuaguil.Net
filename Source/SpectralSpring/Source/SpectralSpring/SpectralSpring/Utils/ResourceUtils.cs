using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;

using Common.Logging;

namespace SpectralSpring.Utils
{
    public class ResourceUtils
    {
        public static string DEFAULT_IMAGE_URL = "/SpectralSpring;component/Images/Messages/ErrorIcon48.png";

        protected static readonly ILog log = LogManager.GetLogger(typeof(ResourceUtils));

        /// <summary>
        /// Gets and image source. Returns an error image source if the image is missing
        /// </summary>
        /// <param name="uri">path to image resource</param>
        /// <returns></returns>
        public static ImageSource GetImageSource(string uri)
        {
            ImageSource imageSource = null;
            try
            {
                imageSource = BitmapFrame.Create(Application.GetResourceStream(new Uri(uri, UriKind.RelativeOrAbsolute)).Stream);
            }
            catch (IOException ioe)
            {
                if (log.IsWarnEnabled)
                {
                    log.Warn("Icon not found: " + uri + " ( " + ioe.Message + " ) "); 
                }

                imageSource = BitmapFrame.Create(Application.GetResourceStream(new Uri(DEFAULT_IMAGE_URL, UriKind.RelativeOrAbsolute)).Stream);
            }

            return imageSource;
        }

        public static ImageSource GetImageSourceById(string id)
        {
            return GetImageSourceById(id, null);
        }

        /// <summary>
        /// Retrieves an ImageSource by resource id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ImageSource GetImageSourceById(string id, string alternateId)
        {
            try
            {
                if (Application.Current.Resources.Contains(id))
                    return (ImageSource)Application.Current.Resources[id];

                if (alternateId != null)
                    return GetImageSourceById(alternateId);
            }
            catch (Exception e)
            {
                if (log.IsDebugEnabled)
                    log.Debug("Cannot find ImageSource resource with id " + id + ", trying alternate", e);
            }

            // return the error
            return GetImageSource(DEFAULT_IMAGE_URL);
        }

        // FIXME: explain this
        public static string GetMessage(string id)
        {
            if (Application.Current.Resources.Contains(id))
                return (string)Application.Current.Resources[id];
           
            if (log.IsDebugEnabled)
                log.Debug("Cannot find message resource with id " + id);
           
            return id + "-NO-MESSAGE";
        }

        internal static string GetKeyTip()
        {
            throw new NotImplementedException();
        }
    }
}
