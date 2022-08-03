using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.LocalPrivate
{
    public class DiskInfoContainFileProfile: Profile

    {
        public DiskInfoContainFileProfile()
        {
            CreateMap<PrivateDiskInfo, DiskInfoContainFileInfoDto>().ReverseMap();
        }
    }
}
