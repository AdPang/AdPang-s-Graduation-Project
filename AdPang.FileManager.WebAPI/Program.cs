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
builder.Services.AddSwaggerGen();
//AppSettingע��
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
    //���IdentityDbContext

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