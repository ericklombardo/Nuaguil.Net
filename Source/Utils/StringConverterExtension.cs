using System;
using Nuaguil.Utils.Model.Dto;

namespace Nuaguil.Utils
{
    public static class StringConverterExtension
    {

        #region DateTime converter
        /// <summary>
        /// Método para convertir una cadena de texto a una fecha/hora válida
        /// El formato debe ser yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string date)
        {
            return Eval.StrToDateTime(date, "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// Método para convertir una cadena de texto a una fecha
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDate(this string date)
        {
            return Eval.StrToDateTime(date, "yyyy-MM-dd");
        }
        /// <summary>
        /// Método para convertir una cadena de texto a una fecha/hora válida
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string date, string format)
        {
            
            return Eval.StrToDateTime(date,format);
        }
        #endregion

        #region Nullable date converters
        public static DateTime? ToNullDateTime(this string date)
        {
            return Eval.StrToNullDateTime(date, "yyyy-MM-dd HH:mm:ss");
        }
        public static DateTime? ToNullDate(this string date)
        {
            return Eval.StrToNullDateTime(date, "yyyy-MM-dd");
        }
        public static DateTime? ToNullDateTime(this string date, string format)
        {
            return Eval.StrToNullDateTime(date, format);
        }
        #endregion

        #region Number converters
        /// <summary>
        /// Método para convertir un string a un entero de 32 bits válido
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this string value)
        {
            return Eval.StrToInt32(value);
        }
        /// <summary>
        /// Método para convertir un string a un entero de 64 bits válido
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this string value)
        {
            return Eval.StrToInt64(value);
        }
        /// <summary>
        /// Método para convertir un string a un decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this string value)
        {
            return Eval.StrToDecimal(value);
        }
        #endregion

        #region String converters
        public static String ToStringOrNull(this string value)
        {
            return Eval.ToStringOrNull(value);
        }
        #endregion

        #region Boolean converters
        public static Boolean ToBoolean(this string value)
        {
            return Eval.ToBoolean(value);

        }
        #endregion

        #region Nullable number converters
        /// <summary>
        /// Método para convertir un string a un Nullable Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32? ToNullInt32(this string value)
        {
            return Eval.StrToNullInt32(value);
        }
        /// <summary>
        /// Método para convertir un string a un Nullable Int64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64? ToNullInt64(this string value)
        {
            return Eval.StrToNullInt64(value);
        }
        /// <summary>
        /// Método para convertir un string a un Nullable Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal? ToNullDecimal(this string value)
        {
            return Eval.StrToNullDecimal(value);
        }
        #endregion

        #region Order converter
        public static Order ToOrder(this string property, string direction)
        {
            return Eval.StrToOrder(property,direction);
        }
        #endregion

        #region Enum converter

        public static T ToEnum<T>(this string value) where T : struct
        {
            return Eval.StrToEnum<T>(value);
        }

        #endregion

    }
}
