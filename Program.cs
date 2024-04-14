using Labb1EntityFramework.Data;
using Labb1EntityFramework.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Labb1EntityFramework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString") ??
                throw new InvalidOperationException("Connection string not found.");

            builder.Services.AddDbContext<LeaveAppDbContext>(options =>
                options.UseSqlServer(connectionString));

            //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<LeaveAppDbContext>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>
                (options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<LeaveAppDbContext>();

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            
            builder.Services.AddControllersWithViews();

            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = $"/Identity/Account/Login";
                option.LogoutPath = $"/Identity/Account/Logout";
                option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            builder.Services.AddRazorPages();

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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
