using Microsoft.AspNetCore.Authentication.Cookies;

namespace SV21T1020620.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews()
                .AddMvcOptions(option =>
                {
                    option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(option =>
                            {
                                option.Cookie.Name = "AuthenticationCookie";            //Tên Cookie
                                option.LoginPath = "/Account/Login";                    //URL trang đăng nhập
                                option.AccessDeniedPath = "/Account/AccessDenined";     //URL đến trang cấm truy cập
                                option.ExpireTimeSpan = TimeSpan.FromDays(360);         //Thời gian 'sống' của Cookie
                            });
            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(60);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });
            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            ApplicationContext.Configure(
                context: app.Services.GetRequiredService<IHttpContextAccessor>(),
                enviroment: app.Services.GetRequiredService<IWebHostEnvironment>()
                );
            // khởi tạo cấu hình cho Bussinesslayer
            string connectióntring = builder.Configuration.GetConnectionString("LiteCommerceDB");
            SV21T1020620.BusinessLayers.Configuration.Initialize(connectióntring);
            app.Run();
        }
    }
}
