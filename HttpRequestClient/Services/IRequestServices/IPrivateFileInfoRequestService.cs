using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile;
using AdPang.FileManager.Shared.Paremeters;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface IPrivateFileInfoRequestService : IBaseRequestService<PrivateFileInfoDto, Guid>
    {
        Task<ApiResponse<PagedList<PrivateFileInfoDto>>> GetAllFileInfoAsync(PrivateFileInfoQueryParameter privateFileInfoQueryParameter);
    }
}
