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

            //获取请求路径
            string url = context.Request.Host + context.Request.Path + context.Request.QueryString;
            //获取Ip
            string ipAddress = context.Connection.RemoteIpAddress is not null ?
                context.Connection.RemoteIpAddress.ToString() : "";

            //将异常信息Exception插入到数据库
            logDbContext.ExceptionLog.Add(new ExceptionLog
            {
                ExceptionMessage = e.Message,
                OperaByUserId = requestInfoModel.CurrentOperaingUser,
                RequsetUrl = url,
                RequestIPAddress = ipAddress,
                StackTrace = e.StackTrace,
                ExceptionType = e.GetType().FullName,
            });

            await logDbContext.SaveChangesAsync();
            //打印日志
            logger.LogError(e, context.Request.Path);
            //返回错误信息
            var result = new ApiResponse()
            {
                Status = false,
                Message = e.Message
            };
            var jsonresult = JsonConvert.SerializeObject(result);
            //返回对象
            await context.Response.WriteAsync(jsonresult);
            //打印日志堆栈跟踪信息
            logger.LogError(e.StackTrace);
        }
    }

}
