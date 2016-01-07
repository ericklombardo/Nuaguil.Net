using System;
using System.Collections.Generic;
using System.Reflection;
using Nuaguil.Utils.DesignByContract;

namespace Nuaguil.Utils
{
    /// <summary>
    /// Clase con utilidades para manipular tipo de datos enum
    /// </summary>
    public static class EnumHelper
    {

        private static Dictionary<Type, Dictionary<String, String>> _enums=new Dictionary<Type,Dictionary<string,string>>(); 
        
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        /// <summary>
        /// Convierte un tipo enum a un diccionario(Hash)
        /// </summary>
        /// <typeparam name="T">Tipo enumerado</typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> EnumToDictionary<T>()
        {
            int i = 0;
            Type enumType = typeof(T);
            Check.Require(enumType.BaseType == typeof(Enum), "El tipo T debe ser un System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            string[] names = Enum.GetNames(enumType);
            Dictionary<string, int> enumValList = new Dictionary<string, int>();
            foreach (int val in enumValArray)
            {
                enumValList.Add(names[i++],val);
            }

            return enumValList;
        }

        public static Dictionary<String, String> GetStringValues<T>()
        {
            Type enumType = typeof(T);
            Check.Require(enumType.BaseType == typeof(Enum), "El tipo T debe ser un System.Enum");
            
            if (_enums.ContainsKey(enumType))
            {
                return _enums[enumType];
            }
            else
            {
                _enums.Add(enumType, new Dictionary<string, string>());
                string[] names = Enum.GetNames(enumType);
                foreach (string name in names)
                {
                    // Get fieldinfo for this type
                    FieldInfo fieldInfo = enumType.GetField(name);
                    // Get the stringvalue attributes
                    StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                        typeof(StringValueAttribute), false) as StringValueAttribute[];

                    if (attribs.Length > 0)
                    {
                        _enums[enumType].Add(name, attribs[0].StringValue);
                    }
                }
                return _enums[enumType];
            }
        }

        public static Dictionary<String, String> GetValueAndStringValue<T>()
        {
           Type enumType = typeof(T);
           Check.Require(enumType.BaseType == typeof(Enum), "El tipo T debe ser un System.Enum");

           if (_enums.ContainsKey(enumType))
           {
              return _enums[enumType];
           }
           else
           {
              _enums.Add(enumType, new Dictionary<string, string>());
              var values = Enum.GetValues(enumType);
              foreach (string name in values)
              {
                 // Get fieldinfo for this type
                 FieldInfo fieldInfo = enumType.GetField(name);
                 // Get the stringvalue attributes
                 StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                     typeof(StringValueAttribute), false) as StringValueAttribute[];

                 if (attribs.Length > 0)
                 {
                    _enums[enumType].Add(name, attribs[0].StringValue);
                 }
              }
              return _enums[enumType];
           }
        }
       
       public static bool IsStringValueDefined<T>(string value)
        {
            Dictionary<string, string> dict = GetStringValues<T>();
            return dict.ContainsValue(value);
        }

        public static bool IsDefined<T>(object value)
        {
            Type enumType = typeof(T);
            Check.Require(enumType.BaseType == typeof(Enum), "El tipo T debe ser un System.Enum");
            bool ret = false;

            if (value == null) return false;

            if (value is String)
            {
                string str = value.ToString();
                if (String.IsNullOrEmpty(str)) return false;
                
                if (str.Length == 1)
                {
                    int esInt;
                    if(Int32.TryParse(str,out esInt))
                        return Enum.IsDefined(enumType, esInt);
                    
                    int newVal = str[0];
                    return Enum.IsDefined(enumType, newVal);
                }
            }
            
            ret = Enum.IsDefined(enumType, value);

            return ret;
        }
    

    }
}
