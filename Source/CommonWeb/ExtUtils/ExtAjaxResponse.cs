namespace Nuaguil.CommonWeb.ExtUtils
{

    /// <summary>
    /// Clase para retornar un objeto compatible con ExtJs2.2
    /// </summary>
    public class ExtAjaxResponse
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

        private ExtAjaxResponse() { }

        public static ExtAjaxResponse GetSuccessResponse()
        {

            ExtAjaxResponse response = new ExtAjaxResponse();

            response.invalidMessage = string.Empty;
            response.success = true;

            return response;
        }

        public static ExtAjaxResponse GetFailResponse(string invalidMessage)
        {
            ExtAjaxResponse response = new ExtAjaxResponse();

            response.invalidMessage = invalidMessage;
            response.success = false;

            return response;
        }


    }
}
