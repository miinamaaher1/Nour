using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using XCourse.Core.ViewModels.AssistantViewModels;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Services.Implementations.AssistantServices
{
    public class GroupDetailsService : IGroupDetailsService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public GroupDetailsService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public AssistantGroupDetails Details(int id)
        {
            var group = _unitOfWork.Groups.Find(g => g.ID == id,
                new string[] { "Address", "DefaultRoom.Center", "Teacher.AppUser", "Subject", "GroupDefaults" }
            );

            if (group == null)
            {
                new AssistantGroupDetails();
            }

            AssistantGroupDetails details = new AssistantGroupDetails()
            {
                Id = group.ID,
                Address = group.Address,
                Location = group.Location,
                Key = _configuration["GoogleMaps:ApiKey"],
                DefaultRoom = group.DefaultRoom,
                Sessions = _unitOfWork.Sessions.FindAll(s => s.GroupID == group.ID, new string[] { "RoomReservation.Room" }, null, 3).ToList(),
                Teacher = group.Teacher,
                IsOnline = group.IsOnline,
                IsPrivate = group.IsPrivate,
                CoverPicture = group.CoverPicture,
                Subject = group.Subject,
                PricePerSession = group.PricePerSession,
                Defaults = new List<DefaultTimeVM>()
            };

            foreach (var def in group.GroupDefaults)
            {
                var time = new DefaultTimeVM()
                {
                    WeekDay = def.WeekDay,
                    StartTime = def.StartTime,
                    EndTime = def.EndTime
                };
                details.Defaults.Add(time);
            }

            return details;
        }

    }
}
