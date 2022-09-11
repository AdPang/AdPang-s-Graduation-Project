using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.LocalPrivate
{
    public class PrivateFileInfoProfile : Profile
    {
        public PrivateFileInfoProfile()
        {
            CreateMap<PrivateFileInfo, PrivateFileInfoDto>().ReverseMap();
        }
    }
}
