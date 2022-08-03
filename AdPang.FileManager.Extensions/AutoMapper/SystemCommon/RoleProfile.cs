using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
