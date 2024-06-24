using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MixMeal.Models;

namespace MixMeal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ModelContext>(x => x.UseOracle(builder.Configuration.GetConnectionString("MixMeal")));
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            
            builder.Services.AddSession(options => {

                options.IdleTimeout = TimeSpan.FromMinutes(30);

            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication("CookieAuth")
                  .AddCookie("CookieAuth", options =>
                {
                    options.LoginPath = "/Account/Login"; // Specify your login path
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect here when access is denied
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
            app.UseSession();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                 name: "monthlyReport",
                 pattern: "{controller=Admin}/{action=MonthlyReport}/{year?}/{month?}");

            app.MapControllerRoute(
                 name: "annualReport",
                 pattern: "{controller=Admin}/{action=AnnualReport}/{year?}");
            app.Run();
        }
    }
}