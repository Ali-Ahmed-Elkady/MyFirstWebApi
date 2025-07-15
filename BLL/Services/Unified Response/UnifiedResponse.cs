using System.Net;

namespace BLL.Services.Unified_Response
{
    public class UnifiedResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public UnifiedResponse(bool success, T? data = default, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static UnifiedResponse<T> SuccessResult(T data, string? message = null)
            => new UnifiedResponse<T>(true, data, message);

        public static UnifiedResponse<T> ErrorResult(string message)
            => new UnifiedResponse<T>(false, default, message);
    }

}
