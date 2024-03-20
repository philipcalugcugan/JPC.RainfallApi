namespace JPC.Application.Shared.Rainfall.Dto
{
    public class RainfallApiException : HttpRequestException
    {
        public RainfallApiException()
        {
        }

        public RainfallApiException(string message)
            : base(message)
        {
        }

        public RainfallApiException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public RainfallApiException(string message, Exception inner, System.Net.HttpStatusCode statusCode)
            : base(message, inner, statusCode)
        {
        }
    }
}
