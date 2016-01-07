using System.Text;
using System.Text.RegularExpressions;

namespace Nuaguil.Utils
{
    public static class StringHelper
    {

        public static string ToCamelCasing(this string str)
        {
            string[] arr = str.Split('_');
            string tmp;
            if (arr.Length <= 1)
            {
                return str.ToLower();
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(arr[0].ToLower());
            for (int i = 1; i < arr.Length; i++)
            {
                tmp = arr[i].ToLower();
                sb.Append(tmp.Substring(0, 1).ToUpper() + tmp.Substring(1));
            }
            return sb.ToString();
        }

        public static string ToPascalCasing(this string str)
        {
            string tmp = ToCamelCasing(str);
            return tmp.Substring(0, 1).ToUpper() + tmp.Substring(1);
        }

        public static string ToFirstUpper(this string str)
        {
           return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
        }

        public static string ToCapitalizeWord(this string str)
        {
           string[] strArray = str.Split(new[] { ' ' });
           StringBuilder builder = new StringBuilder();
           for (int i = 0; i < strArray.Length; i++)
           {
              string str2 = strArray[i].ToLower();
              builder.AppendFormat(" {0}{1}", str2.Substring(0, 1).ToUpper(), str2.Substring(1));
           }
           return builder.ToString().Trim();
        }

        public static string ToLetras(this string str,NumLetras.Tipo tipo)
        {
           return NumLetras.Convert(str, tipo);
        }

        public static string Truncate(this string source, int length)
        {
           if (source != null && source.Length > length)
           {
              source = source.Substring(0, length);
           }
           return source;
        }

        public static string ToUpperOrNull(this string source)
        {
           if (source == null)
           {
              return null;
           }
           return source.ToUpper();
        }

        public static string ToLowerOrNull(this string source)
        {
           if (source == null)
           {
              return null;
           }
           return source.ToLower();
        }


        public static string Underscore(this string input, bool toLower = true)
        {
            var str = Regex.Replace(Regex.Replace(Regex.Replace(input, "([A-Z]+)([A-Z][a-z])", "$1_$2"), "([a-z\\d])([A-Z])", "$1_$2"), "[-\\s]", "_");
            return toLower ? str.ToLower() : str.ToUpper();
        }

        public static string Fmt(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

    }
}
