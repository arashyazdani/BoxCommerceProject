namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse(int statusCode, string? message = null, object? data = null) : base(statusCode, message, data)
        {
        }
        public IEnumerable<string>? Errors { get; set; }
    }
}
