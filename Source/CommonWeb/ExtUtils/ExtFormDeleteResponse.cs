namespace Nuaguil.CommonWeb.ExtUtils
{
    /// <summary>
    /// Clase para retornar un objeto compatible con ExtJs2.2
    /// </summary>
    public class ExtFormDeleteResponse
    {

        private bool _success;
        private string _errorMessage;

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

        public string errorMessage
        {
            set
            {
                _errorMessage = value;
            }
            get
            {
                return _errorMessage;
            }
        }

        private ExtFormDeleteResponse() { }

        public static ExtFormDeleteResponse GetSuccessResponse()
        {
            ExtFormDeleteResponse response = new ExtFormDeleteResponse();

            response.success = true;
            response.errorMessage = string.Empty;
            return response;
        }

        public static ExtFormDeleteResponse GetFailResponse(string errorMessage)
        {
            ExtFormDeleteResponse response = new ExtFormDeleteResponse();

            response.success = false;
            response.errorMessage = errorMessage;
            return response;
        }


    }
}
