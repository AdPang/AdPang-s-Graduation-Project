using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
