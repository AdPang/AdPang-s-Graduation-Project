using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.LocalPrivate
{
    /// <summary>
    /// 私有硬盘实体AutoMapper配置
    /// </summary>
    public class PrivateDiskInfoProfile : Profile
    {
        /// <summary>
        /// 构造
        /// </summary>
        public PrivateDiskInfoProfile()
        {
            //CreateMap<PrivateDiskInfo, PrivateDiskInfoDto>().ReverseMap();
            CreateMap<PrivateDiskInfo, PrivateDiskInfoDto>().ForMember(a => a.CanDelete, b => b.MapFrom(c => (c.PrivateFiles == null || c.PrivateFiles.Count == 0)));
            CreateMap<PrivateDiskInfoDto, PrivateDiskInfo>();

            CreateMap<PrivateDiskInfo, DiskInfoContainFileInfoDto>().ReverseMap();
        }
    }
}
