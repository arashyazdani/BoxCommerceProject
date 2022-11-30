namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string? message = null, object? data = null, string? details = null) : base(statusCode, message, data)
        {
            Details = details;
        }

        public string? Details { get; set; }
    }
}
