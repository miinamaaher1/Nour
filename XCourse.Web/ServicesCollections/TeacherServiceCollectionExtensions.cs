﻿using XCourse.Services.Implementations.TeacherServices;
using XCourse.Services.Interfaces.Teachers;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Web.ServicesCollections
{
    public static class TeacherServiceCollectionExtensions
    {
        public static IServiceCollection AddTeacherServices(this IServiceCollection services)
        {
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IAssistantService, AssistantService>();
            return services;
        }
    }
}
