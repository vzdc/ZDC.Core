using AutoMapper;
using ZDC.Models;

namespace ZDC.Core.Dtos
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserStaffDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<User, StatsDto>()
                .ForMember(x => x.Hours, opt => opt.Ignore());
            CreateMap<Hours, HoursDto>();
            CreateMap<Announcement, AnnouncementDto>();
            CreateMap<EventRegistration, EventRegistrationDto>();
        }
    }
}