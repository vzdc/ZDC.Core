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
        }
    }
}