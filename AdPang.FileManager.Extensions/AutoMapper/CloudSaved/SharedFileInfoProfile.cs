using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.CloudSaved
{
    internal class SharedFileInfoProfile : Profile
    {
        public SharedFileInfoProfile()
        {
            CreateMap<SharedFileInfoDto, SharedFileInfo>().ReverseMap();
            CreateMap<SharedFileInfoDetailDto, SharedFileInfo>().ReverseMap();
        }
    }
}
