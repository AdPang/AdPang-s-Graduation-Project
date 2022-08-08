using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Extensions.AutoMapper.CloudSaved
{
    internal class CloudFileInfoProfile : Profile
    {
        public CloudFileInfoProfile()
        {
            CreateMap<CloudFileInfoDetailDto, CloudFileInfo>().ReverseMap();
            CreateMap<CloudFileInfoDto, CloudFileInfo>().ReverseMap();
        }
    }
}
