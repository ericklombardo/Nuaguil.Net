using System;
using System.Globalization;
using Nuaguil.Utils.DesignByContract;
using Nuaguil.Utils.Model.Dto;

namespace Nuaguil.Utils
{
    /// <summary>
    /// Clase que permite realizar conversiones de strings a 
    /// otros tipos de datos
    /// </summary>
    public static class Eval
    {
        #region DateTime converter
        /// <summary>
        /// Método para convertir una cadena de texto a una fecha/hora válida
        /// El formato debe ser yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StrToDateTime(string date)
        {
            return StrToDateTime(date,"yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// Método para convertir una cadena de texto a una fecha
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StrToDate(string date)
        {
            return StrToDateTime(date, "yyyy-MM-dd");
        }
        /// <summary>
        /// Método para convertir una cadena de texto a una fecha/hora válida
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime StrToDateTime(string date,string format)
        {
            DateTime result;
            Check.Require(DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result),
                "Error al convertir fecha. Formato no válido (debe ser " + format +")");

            return result;
        }
        #endregion

        #region Nullable date converters
        public static DateTime? StrToNullDateTime(string date)
        {
            return StrToNullDateTime(date, "yyyy-MM-dd HH:mm:ss");
        }
        public static DateTime? StrToNullDate(string date)
        {
            return StrToNullDateTime(date, "yyyy-MM-dd");
        }
        public static DateTime? StrToNullDateTime(string date, string format)
        {

            if (String.IsNullOrEmpty(date)) return null;

            DateTime result;
            Check.Require(DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result),
                "Error al convertir fecha. Formato no válido (debe ser " + format + ")");

            return result;
        }
        #endregion

        #region Number converters
        /// <summary>
        /// Método para convertir un string a un entero de 32 bits válido
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 StrToInt32(string value)
        {
            int result;

            if (String.IsNullOrEmpty(value)) return 0; 

            Check.Require(Int32.TryParse(value, out result), "Error al convertir la cadena a número");
            return result;
        }
        /// <summary>
        /// Método para convertir un string a un entero de 64 bits válido
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 StrToInt64(string value)
        {
            long result;

            if (String.IsNullOrEmpty(value)) return 0;

            Check.Require(Int64.TryParse(value, out result), "Error al convertir la cadena a número");
            return result;
        }
        /// <summary>
        /// Método para convertir un string a un decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal StrToDecimal(string value)
        {
            decimal result;

            if (String.IsNullOrEmpty(value)) return 0;

            Check.Require(decimal.TryParse(value, out result), "Error al convertir la cadena a número");
            return result;
        }

        /// <summary>
        /// Método para convertir un string a un decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>        
        public static double StrToDouble(string value)
        {
            double result;

            if (String.IsNullOrEmpty(value)) return 0;

            Check.Require(double.TryParse(value, out result), "Error al convertir la cadena a número");
            return result;
        }

        
        #endregion

        #region String converters
        public static String ToStringOrNull(string value)
        {
            return String.IsNullOrEmpty(value) ? null : value;
        }
        #endregion

        #region Boolean converters
        public static Boolean ToBoolean(string value)
        {
            return (value == "1" || value == "on" || value == "true");

        }
        #endregion

        #region Nullable number converters
        /// <summary>
        /// Método para convertir un string a un Nullable Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32? StrToNullInt32(string value)
        {
            int result;
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            Check.Require(Int32.TryParse(value, out result), "Error al convertir la cadena a número");
            return result;
        }
        /// <summary>
        /// Método para convertir un string a un Nullable Int64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64? StrToNullInt64(string value)
        {
            long result;
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            Check.Require(Int64.TryParse(value, out result), "Error al convertir la cadena a número");
            return result;
        }
        /// <summary>
        /// Método para convertir un string a un Nullable Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal? StrToNullDecimal(string value)
        {
            decimal result;
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }
            
            Check.Require(decimal.TryParse(value, out result), "Error al convertir la cadena a número");
            return result;
        }
        /// <summary>
        /// Método para convertir un string a un Nullable Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double? StrToNullDouble(string value)
        {
            Double result;
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            Check.Require(Double.TryParse(value, out result), "Error al convertir la cadena a número");
            return result;
        }
        #endregion

        #region Order converter
        public static Order StrToOrder(string property, string direction)
        {
            //Validando precondiciones
            Check.Require(!String.IsNullOrEmpty(property), "El nombre de la propiedad no puede ser nullo");
            Check.Require(!String.IsNullOrEmpty(direction), "Debe especificar la dirección del orden (ASC/DESC)");

            Order order = new Order();
            order.Property = property;
            order.Direction = (OrderDirection)Enum.Parse(typeof(OrderDirection), direction);

            return order;
        }
        #endregion

        #region Enum converter

        public static T StrToEnum<T>(string value) where T : struct
        {
            T enumToRet;
            if (Enum.TryParse(value, true, out enumToRet))
                return enumToRet;

            throw new Exception("Error al convertir el string a enumerado");
        }

        #endregion

    }
}
