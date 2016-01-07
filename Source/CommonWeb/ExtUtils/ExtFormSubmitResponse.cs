namespace Nuaguil.CommonWeb.ExtUtils
{

    /// <summary>
    /// Clase para retornar un objeto compatible con la acción submit de ExtJs2.2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtFormSubmitResponse<T>
    {

        private bool _success;
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

        private Csla.Validation.BrokenRulesCollection _errors;
        public Csla.Validation.BrokenRulesCollection errors
        {
            set
            {
                _errors = value;
            }
            get
            {
                return _errors;
            }
        }

        private T _data;
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

        private string _invalidMessage;
        public string invalidMessage
        {
            set
            {
                _invalidMessage = value;
            }
            get
            {
                return _invalidMessage;
            }
        }

        private ExtFormSubmitResponse() { }

        public static ExtFormSubmitResponse<T> GetSuccessResponse(T data)
        {

            ExtFormSubmitResponse<T> response = new ExtFormSubmitResponse<T>();

            response.errors = null;
            response.invalidMessage = string.Empty;
            response.success = true;
            response.data = data;

            return response;
        }

        public static ExtFormSubmitResponse<T> GetFailResponse(string invalidMessage,Csla.Validation.BrokenRulesCollection errors)
        {
            ExtFormSubmitResponse<T> response = new ExtFormSubmitResponse<T>();

            response.errors = errors;
            response.invalidMessage = invalidMessage;
            response.success = false;
            response.data = default(T);

            return response;
        }


        public static ExtFormSubmitResponse<T> GetFailResponse(Csla.Validation.BrokenRulesCollection errors)
        {
            ExtFormSubmitResponse<T> response = new ExtFormSubmitResponse<T>(); 

            response.errors = errors;
            response.invalidMessage = string.Empty;
            response.success = false;
            response.data = default(T);
            
            return response;

        }

        public static ExtFormSubmitResponse<T> GetFailResponse(string invalidMessage)
        {
            ExtFormSubmitResponse<T> response = new ExtFormSubmitResponse<T>();

            response.errors = null;
            response.invalidMessage = invalidMessage;
            response.success = false;
            response.data = default(T);

            return response;
        }


    }
}
