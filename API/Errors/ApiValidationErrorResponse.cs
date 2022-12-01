namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse(int statusCode, string? message = null, object? data = null) : base(statusCode, message, data)
        {
        }

        public ApiValidationErrorResponse() : base(400)
        {
        }

        public IEnumerable<string>? Errors { get; set; }
    }
}
