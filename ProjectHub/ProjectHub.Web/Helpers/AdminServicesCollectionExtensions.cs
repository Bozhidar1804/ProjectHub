using ProjectHub.Web.Areas.Admin.Services;
using ProjectHub.Web.Areas.Admin.Services.Interfaces;

namespace ProjectHub.Web.Helpers
{
    public static class AdminServicesCollectionExtensions
    {
        // This method exists due to circular dependency problem between ProjectHub.Web and ProjectHub.Web.Infrastructure.
        // Both can't reference each other at the same time, so I can't access Admin services in Infrastructure and access the Extensions in Web.
        // I had to create a separate AddAdminServices() method in order to register Admin services successfully.
        public static void AddAdminServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IManagementService, ManagementService>();
        }
    }
}
