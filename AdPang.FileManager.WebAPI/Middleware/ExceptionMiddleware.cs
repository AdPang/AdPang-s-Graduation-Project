using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.EntityFrameworkCore.LogDb;
using AdPang.FileManager.Models.LogEntities;
using AdPang.FileManager.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AdPang.FileManager.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestInfoModel requestInfoModel;
        private readonly LogDbContext logDbContext;
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,RequestInfoModel requestInfoModel,LogDbContext logDbContext)
        {
            this.logger = logger;
            this.requestInfoModel = requestInfoModel;
            this.logDbContext = logDbContext;
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

            
            string url = context.Request.Host + context.Request.Path + context.Request.QueryString;
            string ipAddress = context.Connection.RemoteIpAddress is not null ?
                context.Connection.RemoteIpAddress.ToString() : "";

            
            logDbContext.ExceptionLog.Add(new ExceptionLog
            {
                ExceptionMessage = e.Message,
                OperaByUserId = requestInfoModel.CurrentOperaingUser,
                RequsetUrl = url,
                RequestIPAddress = ipAddress,
                StackTrace = e.StackTrace,
            });
            var a = context.TraceIdentifier;
            await logDbContext.SaveChangesAsync();

            logger.LogError(e, context.Request.Path);

            var result = new ApiResponse()
            {
                Status = false,
                Message = e.Message,
                Result = 500
            };
            var jsonresult = JsonConvert.SerializeObject(result);
            await context.Response.WriteAsync(jsonresult);

            logger.LogError(e.StackTrace);
        }
    }

}
