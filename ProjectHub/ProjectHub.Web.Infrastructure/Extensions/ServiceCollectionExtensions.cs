using Microsoft.Extensions.DependencyInjection;

using ProjectHub.Services.Data;
using ProjectHub.Services.Data.Interfaces;

namespace ProjectHub.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBaseService, BaseService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IProjectService, ProjectService>();
            serviceCollection.AddScoped<ICandidatureService, CandidatureService>();
        }
    }
}
