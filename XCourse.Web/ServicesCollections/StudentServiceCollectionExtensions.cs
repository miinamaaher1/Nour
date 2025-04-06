using XCourse.Services.Implementations.Student;
using XCourse.Services.Implementations;
using XCourse.Services.Implementations.StudentServices;
using XCourse.Services.Interfaces.Student;
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
            services.AddScoped<IEnrollStudentService, EnrollStudentService>();
            services.AddScoped<IRequestPrivateGroupService, RequestPrivateGroupService>();
            services.AddScoped<IMapService, MapService>();
            services.AddScoped<IStudentGroup, StudentGroupService>();
            services.AddScoped<IStudentHomeService, StudentHomeService>();

            return services;
        }
    }
}
