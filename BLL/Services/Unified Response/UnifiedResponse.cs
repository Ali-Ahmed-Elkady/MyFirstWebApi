using System.Net;

namespace BLL.Services.Unified_Response
{
    public class UnifiedResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string>? Errors { get; set; } = new List<string>();
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public UnifiedResponse(bool success, HttpStatusCode code, T? data = default, string? message = null, List<string>? Errors = null)
        {
            Success = success;
            Data = data;
            Message = message;
            StatusCode = code;
            this.Errors = Errors;
        }

        public static UnifiedResponse<T> SuccessResult(T data, HttpStatusCode code, string? message = null)
            => new UnifiedResponse<T>(true, code, data, message );

        public static UnifiedResponse<T> ErrorResult(List<string> Errors, string message, HttpStatusCode code)
            => new UnifiedResponse<T>(false,code , default, message,Errors);
    }

}
