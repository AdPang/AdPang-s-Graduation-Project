using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AdPang.FileManager.Shared;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface IAuthRequestService
    {
        Task<ApiResponse<AuthDto>> LoginAsync(UserDto user);
        Task<ApiResponse> RegisterAsync(UserDto user);
    }
}
