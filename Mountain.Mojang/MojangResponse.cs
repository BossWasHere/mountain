using System.Net;

namespace Mountain.Mojang
{
    public class MojangResponse<T>
    {
        public T ResponseData { get; }
        public ErrorResponse ErrorResponse { get; }
        public bool Successful { get; }
        public HttpStatusCode StatusCode { get; }

        public MojangResponse(T responseData)
        {
            ResponseData = responseData;
            Successful = true;
            StatusCode = HttpStatusCode.OK;
        }

        public MojangResponse(ErrorResponse errorResponse, HttpStatusCode statusCode)
        {
            ErrorResponse = errorResponse;
            Successful = false;
            StatusCode = statusCode;
        }
    }
}
