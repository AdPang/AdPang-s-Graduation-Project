using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Paremeters;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface IUserCloudDirInfoRequestService : IBaseRequestService<DirInfoDetailDto,Guid>
    {
        Task<ApiResponse<DirInfoDetailDto>> GetDirInfoAsync(QueryParameter queryParameter);
    }
}
