using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DevBlog.Data;
using DevBlog.Services.MailService;
using Microsoft.AspNetCore.Identity.UI.Services;
using DevBlog.Services;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.DependencyInjection;

namespace DevBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DevBlogContextConnection");

            builder.Services.AddDbContext<DevBlogContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DevBlogContext>();
            builder.Services.AddControllersWithViews();
     
            builder.Services
                .AddAuthentication()
                .AddCookie()
                .AddGoogle(options =>
                {
                    var googleAuth = builder.Configuration.GetSection("GoogleAuth");
                    options.ClientId = googleAuth["ClientId"];
                    options.ClientSecret = googleAuth["ClientSecret"];
                }
                )
                .AddFacebook(options =>
                {
                    var facebookAuth = builder.Configuration.GetSection("FacebookAuth");
                    options.AppId = facebookAuth["AppId"];
                    options.AppSecret = facebookAuth["AppSecret"];
                });
       
            #region Cấu hình MailSender
            builder.Services.AddOptions();  // Kích hoạt Options, tự đọng tim cấu hình 
            var mailSetting = builder.Configuration.GetSection("MailSetting");  // đọc config
            builder.Services.Configure<MailSetting>(mailSetting); // map mailSetting vào MailSetting class
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            #endregion

            #region Cấu hình Cloudinary
            builder.Services.AddTransient<ICloudinary, Cloudinary>(provider =>
            {
                var cloudinarySetting = builder.Configuration.GetSection("CloudinaryAccount");
                var account = new Account(
                    cloudinarySetting["Cloud"],
                    cloudinarySetting["ApiKey"],
                    cloudinarySetting["ApiSecret"]);
                return new Cloudinary(account);
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
        }
    }
}
