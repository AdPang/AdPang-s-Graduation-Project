using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.EntityFrameworkCore.IdentityDb;
using AdPang.FileManager.Extensions.ServiceExtensions;
using AdPang.FileManager.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Autofac;
using Newtonsoft.Json.Serialization;
using Autofac.Extensions.DependencyInjection;
using AdPang.FileManager.Common.Helper;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModuleRegister>();
});

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
builder.Services.AddSwaggerGen();
//AppSetting注入
builder.Services.AddSingleton(new Appsettings(builder.Configuration));

#region dbConfig

var connStr = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
#region identityConfig
builder.Services.AddDbContext<IdentityDbContext>(options =>
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
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>()
    ;
#endregion



builder.Services.AddDbContext<FileManagerDbContext>(o =>
    o.UseLazyLoadingProxies().UseSqlServer(connStr,
        oo => oo.MigrationsAssembly("AdPang.FileManager.EntityFrameworkCore")));


#endregion



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();