using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.CloudSaved
{
    internal class DirInfoProfile : Profile
    {
        public DirInfoProfile()
        {
            CreateMap<DirInfoDto, DirInfo>().ReverseMap();
            CreateMap<DirInfoDetailDto, DirInfo>().ReverseMap();
        }
    }
}
