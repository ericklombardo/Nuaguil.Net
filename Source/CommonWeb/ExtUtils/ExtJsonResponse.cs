using System;
using System.Collections.Generic;

namespace Nuaguil.CommonWeb.ExtUtils
{
    /// <summary>
    /// Clase para retornar un objeto compatible con ExtJs2.2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtJsonResponse<T>
    {

        private Int32 _total;
        private IList<T> _data;

        public Int32 total
        {
            set
            {
                _total = value;
            }
            get
            {
                return _total;
            }
        }

        public IList<T> data
        {
            set
            {
                _data = value;
            }
            get
            {
                return _data;
            }
        }

        public ExtJsonResponse()
        {
        }

        public static ExtJsonResponse<T> GetJsonResponse(IList<T> data)
        {
            ExtJsonResponse<T> response = new ExtJsonResponse<T>();
            response.total = data.Count;
            response.data = data;

            return response;
        }

        public static ExtJsonResponse<T> GetJsonResponse(IList<T> data,T itemToAdd)
        {
            data.Insert(0, itemToAdd);
            ExtJsonResponse<T> response = ExtJsonResponse<T>.GetJsonResponse(data);
            
            return response;
        }

    }
}
