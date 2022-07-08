using AdPang.FileManager.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdPang.FileManager.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await ExceptionHandlerAsync(context, e);
            }
        }

        private async Task ExceptionHandlerAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            logger.LogError(e, context.Request.Path);

            var result = new ApiResponse()
            {
                Status = false,
                Message = e.Message,
                Result = 500
            };
            var jsonresult = JsonConvert.SerializeObject(result);
            await context.Response.WriteAsync(jsonresult);
        }
    }

}
