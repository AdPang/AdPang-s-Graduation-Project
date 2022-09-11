using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.SystemCommon;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface IAuthRequestService
    {
        Task<ApiResponse<AuthDto>> LoginAsync(UserDto user);
        Task<ApiResponse> RegisterAsync(UserDto user, Guid seed, string code);
        string GetVerfiyCodeImgUrl(Guid seed);


    }
}
