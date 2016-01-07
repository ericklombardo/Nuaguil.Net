using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Common.Web.MVC
{
   public sealed class JsonNetValueProviderFactory : ValueProviderFactory
   {
      private static void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
      {
         IDictionary<string, object> dictionary = value as IDictionary<string, object>;
         if (dictionary != null)
         {
            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
               AddToBackingStore(backingStore, MakePropertyKey(prefix, keyValuePair.Key), keyValuePair.Value);
         }
         else
         {
            IList list = value as IList;
            if (list != null)
            {
               for (int index = 0; index < list.Count; ++index)
                  AddToBackingStore(backingStore, MakeArrayKey(prefix, index), list[index]);
            }
            else
               backingStore[prefix] = value;
         }
      }

      private static object GetDeserializedObject(ControllerContext controllerContext)
      {
         if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            return null;
         string input = new StreamReader(controllerContext.HttpContext.Request.InputStream).ReadToEnd();
         if (string.IsNullOrEmpty(input))
            return null;

         return JsonConvert.DeserializeObject(input);
      }

      public override IValueProvider GetValueProvider(ControllerContext controllerContext)
      {
         if (controllerContext == null)
            throw new ArgumentNullException("controllerContext");
         object deserializedObject = GetDeserializedObject(controllerContext);
         if (deserializedObject == null)
            return null;
         Dictionary<string, object> backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
         AddToBackingStore(backingStore, string.Empty, deserializedObject);
         return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
      }

      private static string MakeArrayKey(string prefix, int index)
      {
         return prefix + "[" + index.ToString((IFormatProvider)CultureInfo.InvariantCulture) + "]";
      }

      private static string MakePropertyKey(string prefix, string propertyName)
      {
         if (!string.IsNullOrEmpty(prefix))
            return prefix + "." + propertyName;
         return propertyName;
      }
   }
}
