using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.Extensions.ServiceExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Autofac;
using Newtonsoft.Json.Serialization;
using Autofac.Extensions.DependencyInjection;
using AdPang.FileManager.Common.Helper;
using AdPang.FileManager.WebAPI.Middleware;
using AdPang.FileManager.WebAPI.Filter;
using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.EntityFrameworkCore.LogDb;
using AdPang.FileManager.Common.Helper.Mail;
using AdPang.FileManager.Common.Helper.VerifyCode;
using AdPang.FileManager.Common.Helper.Redis;
using Microsoft.EntityFrameworkCore.Diagnostics;
using IGeekFan.AspNetCore.Knife4jUI;
using Autofac.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024;
    
});
#region autoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModuleRegister>();
});
#endregion

#region Jsonת������
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //���л�ʱkeyΪ�շ���ʽ
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;//����ѭ������
    });
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//AppSettingע��
builder.Services.AddSingleton(new Appsettings(builder.Configuration));
builder.Services.AddSingleton(new MailSendHelper(builder.Configuration));
builder.Services.AddScoped(typeof(RequestInfoModel));
builder.Services.AddScoped(typeof(ImgVerifyCodeHelper));
builder.Services.AddScoped(typeof(MailVerifyCodeHelper));

builder.Services.AddScoped(typeof(RedisHelper));

//AutoMapper
builder.Services.AddAutoMapperSetup();
//Filter
builder.Services.AddMvc(options =>
{
    //
    options.Filters.Add(typeof(InitRequestInfoFilter));
    //��־��ӡ
    options.Filters.Add(typeof(ApiLogFilter));
});
//Swagger��С��
builder.Services
    .AddJwtAddAuthentication(builder.Configuration)
    .AddSwaggerAuthoritarian();


#region dbConfig

var connStr = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
#region identityConfig
builder.Services.AddDbContext<FileManagerDbContext>(options =>
{
    options.UseSqlServer(connStr, oo => oo.MigrationsAssembly("AdPang.FileManager.EntityFrameworkCore"));
    options.ConfigureWarnings(x => x.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
});

builder.Services.AddIdentityCore<User>(options =>
{
    //��������
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    //���볤��
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

});

var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);

idBuilder
    .AddEntityFrameworkStores<FileManagerDbContext>()
    .AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>()
    ;
#endregion



//builder.Services.AddDbContext<FileManagerDbContext>(o =>
//    o.UseLazyLoadingProxies().UseSqlServer(connStr,
//        oo => oo.MigrationsAssembly("AdPang.FileManager.EntityFrameworkCore")));

#region LogDb
var logDbConnStr = builder.Configuration.GetSection("ConnectionStrings:LogDbConnStr").Value;
builder.Services.AddDbContext<LogDbContext>(o =>
    o.UseLazyLoadingProxies().UseSqlServer(logDbConnStr,
        oo => oo.MigrationsAssembly("AdPang.FileManager.EntityFrameworkCore")));
#endregion


#endregion



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //���ʵ�ַ
    app.UseSwagger();
    app.UseSwaggerUI();
    //Swaggerʹ���Զ���UI
    app.UseKnife4UI(c =>
    {
        c.RoutePrefix = string.Empty; // serve the UI at root
        c.SwaggerEndpoint("/v1/admin-filemanager-api-docs", "V1 FileManagerDocs");
    });

    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapControllers();
    //    endpoints.MapSwagger("{documentName}/api-docs");
    //});
}

//app.UseHttpsRedirection();
//�쳣�����м��
app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting(); // ·���м��һ��Ҫ���
// �ȿ�����֤
app.UseAuthentication();
// Ȼ������Ȩ�м��
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapSwagger("{documentName}/admin-filemanager-api-docs");
});
app.MapControllers();

app.Run();