using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using static ProjectHub.Web.Infrastructure.Extensions.ApplicationBuilderExtensions;
using static ProjectHub.Common.GeneralApplicationConstants;
using ProjectHub.Web.Infrastructure.Extensions;

namespace ProjectHub.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
            string? connectionString = builder.Configuration.GetConnectionString("SQLServer") ?? throw new InvalidOperationException("Connection string 'SQLServer' not found.");

            builder.Services.AddDbContext<ProjectHubDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
				.AddDbContext<ProjectHubDbContext>(options =>
				{
				   options.UseSqlServer(connectionString);
				});

			builder.Services
				.AddIdentity<ApplicationUser, IdentityRole<Guid>>(cfg =>
				{
					ConfigureIdentity(cfg, builder);
				})
				.AddRoles<IdentityRole<Guid>>()
				.AddEntityFrameworkStores<ProjectHubDbContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddServices();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/User/Login";
            });

            WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
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

			app.SeedAdministrator(AdminEmail);

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			app.Run();
		}

        private static void ConfigureIdentity(IdentityOptions cfg, WebApplicationBuilder builder)
        {
            // Password
            cfg.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
            cfg.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            cfg.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            cfg.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
            cfg.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
            cfg.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueChars");

            // Sign In
            cfg.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            cfg.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
            cfg.SignIn.RequireConfirmedPhoneNumber = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

            // User
            cfg.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireUniqueEmail");
        }
    }
}
