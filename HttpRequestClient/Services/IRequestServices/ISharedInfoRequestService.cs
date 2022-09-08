using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface ISharedInfoRequestService : IBaseRequestService<SharedFileInfoDetailDto, Guid> 
    {
        
    }
}
