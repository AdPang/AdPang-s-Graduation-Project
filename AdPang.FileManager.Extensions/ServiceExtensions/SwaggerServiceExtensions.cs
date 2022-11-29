﻿using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AdPang.FileManager.Extensions.ServiceExtensions
{
    /// <summary>
    /// 静态swagger拓展类
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// Swagger配置服务拓展方法
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerAuthoritarian(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdPang's FileManager System API", Version = "v1" });

                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });
                Assembly assembly = Assembly.GetExecutingAssembly();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, xmlFile);
                c.IncludeXmlComments(xmlPath);
                xmlPath = Path.Combine(basePath, "AdPang.FileManager.WebAPI.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);

                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
            });

            return services;
        }
    }
}
