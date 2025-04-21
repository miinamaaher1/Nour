using XCourse.Services.Implementations.AssistantServices;
using XCourse.Services.Interfaces.AssistantServices;


namespace XCourse.Web.ServicesCollections
{
    public static class AssistantServiceCollectionExtensions
    {
        public static IServiceCollection AddAssistantServices(this IServiceCollection services)
        {
            services.AddScoped<IPendingRequestService, PendingRequestService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupDetailsService, GroupDetailsService>();
            return services;
        }
    }
}

