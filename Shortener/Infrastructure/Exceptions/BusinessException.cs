using Microsoft.AspNetCore.Http;

namespace Infrastructure.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public int StatusCode { get; private set; }

        public BusinessException() : base() { }

        public BusinessException(string message)
            : base(message) { }

        public BusinessException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException) { }

        public BusinessException(string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
