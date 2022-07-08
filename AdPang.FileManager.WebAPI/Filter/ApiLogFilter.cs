using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AdPang.FileManager.WebAPI.Filter
{
    public class ApiLogFilter : IAsyncActionFilter
    {
        private readonly ILogger logger;

        public ApiLogFilter(ILogger<ApiLogFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string actionArguments = JsonConvert.SerializeObject(context.ActionArguments);

            var resultContext = await next();

            string url = resultContext.HttpContext.Request.Host + resultContext.HttpContext.Request.Path + resultContext.HttpContext.Request.QueryString;

            string method = resultContext.HttpContext.Request.Method;

            dynamic result = resultContext.Result.GetType().Name == "EmptyResult" ? new { Value = "EmptyResult" } : resultContext.Result as dynamic;

            string response = JsonConvert.SerializeObject(result.Value);

            logger.LogInformation($"URL：{url} \n " +
                                  $"Method：{method} \n " +
                                  $"ActionArguments：{actionArguments}\n " +
                                  $"Response：{response}\n ");
        }
    }

}
