using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.CloudSaved
{
    internal class UserPrivateFileInfoProfile : Profile
    {
        public UserPrivateFileInfoProfile()
        {
            CreateMap<UserPrivateFileInfoDto, UserPrivateFileInfo>().ReverseMap();
            CreateMap<UserPrivateFileInfoDetailDto, UserPrivateFileInfo>().ReverseMap();
        }
    }
}
