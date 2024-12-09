using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Web.Infrastructure.Extensions;
using static ProjectHub.Web.Infrastructure.Extensions.ApplicationBuilderExtensions;
using static ProjectHub.Web.Helpers.AdminServicesCollectionExtensions;
using static ProjectHub.Common.GeneralApplicationConstants;

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
			builder.Services.AddAdminServices();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/User/Login";
                cfg.LogoutPath = "/Home/Index";
            });

            WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error/500");
				app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

            // Seed the database
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                DatabaseSeeder.Seed(services);
            }

            app.SeedAdministrator(AdminEmail);

            app.MapControllerRoute(
                name: "areas",
                pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
				name: "default",
				pattern: "/{controller=Home}/{action=Index}/{id?}");

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
