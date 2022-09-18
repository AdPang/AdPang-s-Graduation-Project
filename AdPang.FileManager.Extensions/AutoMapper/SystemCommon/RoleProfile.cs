using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper.SystemCommon
{
    /// <summary>
    /// 
    /// </summary>
    internal class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, RoleDetailDto>().ReverseMap();
        }
    }
}
