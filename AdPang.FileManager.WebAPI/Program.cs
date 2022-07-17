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

var builder = WebApplication.CreateBuilder(args);
#region autoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModuleRegister>();
});
#endregion

#region Json转换设置
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //序列化时key为驼峰样式
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;//忽略循环引用
    });
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//AppSetting注入
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
    //日志打印
    options.Filters.Add(typeof(ApiLogFilter));
});
//Swagger加小锁
builder.Services
    .AddJwtAddAuthentication(builder.Configuration)
    .AddSwaggerAuthoritarian();




#region dbConfig

var connStr = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
#region identityConfig
builder.Services.AddDbContext<FileManagerDbContext>(options =>
{
    options.UseSqlServer(connStr, oo => oo.MigrationsAssembly("AdPang.FileManager.EntityFrameworkCore"));
});

builder.Services.AddIdentityCore<User>(options =>
{
    //锁定次数
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    //密码长度
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    //添加IdentityDbContext

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

// 先开启认证
app.UseAuthentication();
// 然后是授权中间件
app.UseAuthorization();


app.MapControllers();

app.Run();