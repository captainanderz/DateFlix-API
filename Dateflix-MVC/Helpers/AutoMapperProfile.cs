using DateflixMVC.Dtos;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Helpers
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            // User map
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            // Role map
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();

            // RoleUser map
            CreateMap<RolerUser, RoleUserDto>();
            CreateMap<RoleUserDto, RolerUser>();
        }
    }
}
