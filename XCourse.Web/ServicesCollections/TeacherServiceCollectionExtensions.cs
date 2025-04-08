using XCourse.Services.Implementations.TeacherServices;
using XCourse.Services.Interfaces.Teachers;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Web.ServicesCollections
{
    public static class TeacherServiceCollectionExtensions
    {
        public static IServiceCollection AddTeacherServices(this IServiceCollection services)
        {
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IGroupService, GroupService>();
            return services;
        }
    }
}
