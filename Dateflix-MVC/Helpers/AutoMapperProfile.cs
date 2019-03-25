using System.Collections.Generic;
using DateflixMVC.Dtos;
using DateflixMVC.Models;
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
            //CreateMap<List<UserDto>, List<User>>();
            //CreateMap<List<User>, List<UserDto>>();

            // Role map
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();

            // RoleUser map
            CreateMap<RolerUser, RoleUserDto>();
            CreateMap<RoleUserDto, RolerUser>();

            // Likes map
            CreateMap<LikeDto, Likes>();
            CreateMap<Likes, LikeDto>();

            // Blocks map
            CreateMap<BlockDto, Blocks>();
            CreateMap<Blocks, BlockDto>();

            // Bans map
            CreateMap<Bans, BanDto>();
            CreateMap<BanDto, Bans>();

            // UserPreference map
            CreateMap<UserPreference, UserPreferenceDto>();
            CreateMap<UserPreferenceDto, UserPreference>();

            // Inquiry map
            CreateMap<Inquiries, InquiryDto>();
            CreateMap<InquiryDto, Inquiries>();
        }
    }
}
