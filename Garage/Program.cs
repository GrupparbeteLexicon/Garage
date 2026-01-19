using Garage.Configuration.Garage.Configuration;
using Garage.Data;
using Garage.Extensions;
using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Garage
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<IdentityOptionsConfig>(builder.Configuration.GetSection("Identity")); // protected roles in appsettings.json

            builder.Services.AddDbContext<GarageContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("GarageContext") ?? throw new InvalidOperationException("Connection string 'GarageContext' not found.")));
            
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<GarageContext>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireMember", policy => policy.RequireRole("Member", "Admin"));
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                await app.SeedDefaultData();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<GarageContext>();
                db.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
                .WithStaticAssets();

            app.Run();
        }
    }
}