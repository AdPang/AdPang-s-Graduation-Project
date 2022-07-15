using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.EntityFrameworkCore.LogDb;
using AdPang.FileManager.Models.LogEntities;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AdPang.FileManager.WebAPI.Filter
{
    public class ApiLogFilter : IAsyncActionFilter
    {
        private readonly ILogger logger;
        private readonly RequestInfoModel requestInfoModel;
        private readonly LogDbContext logDbContext;

        public ApiLogFilter(ILogger<ApiLogFilter> logger,RequestInfoModel requestInfoModel, LogDbContext logDbContext)
        {
            this.logger = logger;
            this.requestInfoModel = requestInfoModel;
            this.logDbContext = logDbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var claims = context.HttpContext.User.Claims.FirstOrDefault(x=>x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            
            
            string actionArguments = JsonConvert.SerializeObject(context.ActionArguments);

            var resultContext = await next();

            string url = resultContext.HttpContext.Request.Host + resultContext.HttpContext.Request.Path + resultContext.HttpContext.Request.QueryString;

            string method = resultContext.HttpContext.Request.Method;

            var result = resultContext.Result;
            if (result == null) return;

            //var resultDynamic = result.GetType().Name == "EmptyResult" ? new { Value = "EmptyResult" } : resultContext.Result as dynamic;
            string response = string.Empty;
            if (result.GetType().Name == "EmptyResult")
            {
                response = "EmptyResult";
            }
            else
            {
                response = JsonConvert.SerializeObject(resultContext.Result);
            }
            string ipAddress = context.HttpContext.Connection.RemoteIpAddress is not null ?
                context.HttpContext.Connection.RemoteIpAddress.ToString(): "";
            logDbContext.ActionLog.Add(new ActionLog
            {
                RequsetUrl = url,
                OperaByUserId = requestInfoModel.CurrentOperaingUser,
                RequestIPAddress = ipAddress,
                ResultJson = response,
                RequestParameter = actionArguments
            });
            await logDbContext.SaveChangesAsync();

            logger.LogInformation(
                $"OperaByUser：{requestInfoModel.CurrentOperaingUser} \n " +
                $"URL：{url} \n " +
                $"Method：{method} \n " +
                $"ActionArguments：{actionArguments}\n " +
                $"Response：{response}\n ");
        }


        
    }

}
