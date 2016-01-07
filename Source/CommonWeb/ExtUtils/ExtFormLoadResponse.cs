namespace Nuaguil.CommonWeb.ExtUtils
{
    /// <summary>
    /// Clase para retornar un objeto compatible con la acción load de ExtJs2.2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtFormLoadResponse<T>
    {

        private bool _success;
        private T _data;

        public bool success
        {
            set
            {
                _success = value;
            }
            get
            {
                return _success;
            }
        }

        public T data
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

        private ExtFormLoadResponse()
        {
        }

        public static ExtFormLoadResponse<T> GetSuccessResponse(T data)
        {
            ExtFormLoadResponse<T> response = new ExtFormLoadResponse<T>();
            response.success = true;
            response.data = data;

            return response;
        }


    }
}
