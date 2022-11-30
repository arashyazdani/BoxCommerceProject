using System.Globalization;
using API.Errors;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text;

namespace API.Middleware
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);

                //Append errors to Errors file on my server I know it's possible to see it on server log error but sometimes maybe we can't or we don't want to login on our server and it help us to see it easier. Of course it's not good for a server that has many users.
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Content/Errors.txt";
                var existFile = File.Exists(path);
                if (!existFile)
                {
                    string textToAdd = "-------------------------------------- Errors --------------------------------------";

                    FileStream fs = new FileStream(path, FileMode.CreateNew);

                    await using StreamWriter writer = new StreamWriter(fs, Encoding.Default);

                    await writer.WriteAsync(textToAdd);
                }
                await using (StreamWriter writer = new StreamWriter(path, true))
                {
                    await writer.WriteLineAsync("-----------------------------------------------------------------------------");

                    await writer.WriteLineAsync("Date : " + DateTime.Now.ToString(CultureInfo.CurrentCulture));

                    await writer.WriteLineAsync();

                    while (ex != null)
                    {
                        await writer.WriteLineAsync(ex.GetType().FullName);

                        await writer.WriteLineAsync("Message : " + ex.Message);

                        await writer.WriteLineAsync("StackTrace : " + ex.StackTrace);

                        if (ex.InnerException != null) ex = ex.InnerException;
                    }
                }
            }
        }
    }
}
