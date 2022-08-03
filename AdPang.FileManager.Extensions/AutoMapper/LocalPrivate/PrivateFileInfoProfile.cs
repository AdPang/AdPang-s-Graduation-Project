using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
