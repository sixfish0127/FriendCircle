using ���֯Z�M��_����C�饭�xMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// �]�w��Ʈw�s��
builder.Services.AddDbContext<FriendCircleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FriendCircleContext")));

// �K�[ DBmanager
builder.Services.AddScoped<DBmanager>();

// �K�[�������ҪA��
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);// �u���b IsPersistent = true �ɥͮ�
        options.Cookie.HttpOnly = true; // ���� XSS ����
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // �ȭ� HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict; // ���� CSRF ����
    });

// �K�[����M���ϪA��
builder.Services.AddControllersWithViews();
// ����Ϥ��j�p
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // ������j�p�� 10 MB
});
var app = builder.Build();

// �t�m HTTP �ШD�޹D
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // �w�]�� HSTS �ȬO 30 �ѡC
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//�ϥΨ������ҩM���v
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();