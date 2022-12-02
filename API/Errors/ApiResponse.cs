namespace API.Errors
{
    // I made this file to have a standard structure for every responses from my API
    public class ApiResponse
    {

        public ApiResponse(int statusCode, string? message = null, object? data = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Data = data;
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "OK",
                204 => "No content",
                400 => "You have made a bad request.",
                401 => "You are not authorized.",
                403 => "You have not permission.",
                404 => "It was not resource found.",
                405 => "Method Not Allowed.",
                500 => "Internal Server Error!",
                _ => null
            };
        }
    }
}
