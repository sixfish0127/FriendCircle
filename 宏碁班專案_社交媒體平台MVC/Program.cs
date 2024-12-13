using 宏碁班專案_社交媒體平台MVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// 設定資料庫連接
builder.Services.AddDbContext<FriendCircleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FriendCircleContext")));

// 添加 DBmanager
builder.Services.AddScoped<DBmanager>();

// 添加身份驗證服務
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);// 只有在 IsPersistent = true 時生效
        options.Cookie.HttpOnly = true; // 防止 XSS 攻擊
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 僅限 HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict; // 防止 CSRF 攻擊
    });

// 添加控制器和視圖服務
builder.Services.AddControllersWithViews();
// 限制圖片大小
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 限制文件大小為 10 MB
});
var app = builder.Build();

// 配置 HTTP 請求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // 預設的 HSTS 值是 30 天。
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//使用身份驗證和授權
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();