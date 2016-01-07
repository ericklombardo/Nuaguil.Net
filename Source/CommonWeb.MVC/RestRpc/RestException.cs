namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.Web;

    public sealed class RestException : HttpException
    {
        public RestException(string message) : base(500, message)
        {
        }

        public RestException(int statusCode, string message) : base(statusCode, message)
        {
        }

        public RestException(int statusCode, string message, Exception innerException) : base(statusCode, message, innerException)
        {
        }
    }
}
