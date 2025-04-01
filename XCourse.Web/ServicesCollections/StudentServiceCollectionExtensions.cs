using XCourse.Services.Implementations.StudentServices;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Web.ServicesCollections
{
    public static class StudentServiceCollectionExtensions
    {
        public static IServiceCollection AddStudentServices(this IServiceCollection services)
        {
            services.AddScoped<ITeacherProfileService, TeacherProfileService>();
            services.AddScoped<IStudentGroup, StudentGroupService>();

            return services;
        }
    }
}
