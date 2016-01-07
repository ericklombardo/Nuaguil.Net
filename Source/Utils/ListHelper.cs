using System;
using System.Collections.Generic;
using System.Text;

namespace Nuaguil.Utils
{
    public static class ListHelper
    {

        public static string Join<T>(this IList<T> lst,char joinCharacter)
        {

            if (lst.Count == 0) return String.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (T item in lst)
            {
                sb.Append(joinCharacter);
                sb.Append(item.ToString());
            }
            return sb.ToString(1, sb.Length-1);
        }
    }
}
