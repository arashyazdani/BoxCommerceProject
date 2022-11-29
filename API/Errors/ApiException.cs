namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string? message, object? data, string? details) : base(statusCode, message, data)
        {
            Details = details;
        }

        public string? Details { get; set; }
    }
}
