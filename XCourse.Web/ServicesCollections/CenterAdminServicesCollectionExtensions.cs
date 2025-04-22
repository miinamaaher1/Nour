using XCourse.Services.Implementations.Student;
using XCourse.Services.Implementations.CenterAdminServices;
using XCourse.Services.Interfaces.Student;
using XCourse.Services.Interfaces.CenterAdminServices;

namespace XCourse.Web.ServicesCollections
{
    public static class CenterAdminServicesCollectionExtensions
    {


        public static IServiceCollection AddCenterAdminServices(this IServiceCollection services)
        {
            services.AddScoped<ICenterAdminService, CenterAdminService>();
            services.AddScoped<ICenterAdminHomeService, CenterAdminHomeService>();


            return services;
        }

    }
}
