using XCourse.Services.Implementations;
using XCourse.Services.Implementations.StudentServices;
using XCourse.Services.Interfaces.StudentServices;
using XCourse.Services.Interfaces.Teachers;

namespace XCourse.Web.ServicesCollections
{
    public static class StudentServiceCollectionExtensions
    {
        public static IServiceCollection AddStudentServices(this IServiceCollection services)
        {
            services.AddScoped<ITeacherProfileService, TeacherProfileService>();
            services.AddScoped<IGroupService,GroupService>();
            return services;
        }
    }
}
