using AdPang.FileManager.Models.FileManagerEntities.Common;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.SystemCommon
{
    internal class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuDto>().ReverseMap();
            CreateMap<Menu, MenusDto>().ReverseMap();
        }
    }
}
