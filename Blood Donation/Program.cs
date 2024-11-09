using Blood_Donation.Data;
using Blood_Donation.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;

namespace Blood_Donation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register repositories
            builder.Services.AddScoped<IBloodRepo, BloodRepo>();
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();

            // Configure cookie-based authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LogoutPath = "/Account/Logout";
                    options.LoginPath = "/Account/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Customize cookie expiration
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Always use secure cookies in production

                });

            // Register the DbContext with connection string
            builder.Services.AddDbContext<BloodContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add session configuration
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
                options.Cookie.HttpOnly = true; // HTTP-only cookie
                options.Cookie.IsEssential = true; // GDPR compliance
            });

            // Add logging (optional but recommended)
            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Authentication middleware
            app.UseAuthorization();  // Authorization middleware

            app.UseSession();  // Enable session

            // Set up default routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            app.Run();
        }
    }
}
