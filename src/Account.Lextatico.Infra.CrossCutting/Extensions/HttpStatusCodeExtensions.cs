using System.Net;

namespace Account.Lextatico.Infra.CrossCutting.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccess(this HttpStatusCode httpStatusCode)
        {
            var statusCode = (int) httpStatusCode;

            return statusCode >= 200 && statusCode <= 299;
        }

        public static bool IsSuccess(int statusCode) => statusCode >= 200 && statusCode <= 299;
    }
}
