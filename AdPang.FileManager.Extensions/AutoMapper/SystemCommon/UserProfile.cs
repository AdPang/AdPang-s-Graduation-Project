using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.SystemCommon
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserDetailDto>().ReverseMap();
            CreateMap<User, UserInfoContainDiskInfoDto>().ReverseMap();
            CreateMap<User, UserInfoContainDirInfoDto>().ReverseMap();
            CreateMap<User, UserInfoContainCloudFileInfoDto>().ReverseMap();
        }
    }
}