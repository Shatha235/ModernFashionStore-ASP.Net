using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ModelContext>(options => options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add EmailSettings to the container
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            // Register the IEmailService with its implementation
            builder.Services.AddTransient<IEmailService, SmtpEmailService>();

            builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });

            // Add authentication services
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/SigninAndSignup/SignIn"; // Your login path
                        options.LogoutPath = "/SigninAndSignup/SignOut"; // Your logout path
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Session timeout
                    });

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

            app.UseAuthentication(); // Add this before UseAuthorization
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}