using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Paremeters;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface IUserCloudDirInfoRequestService : IBaseRequestService<DirInfoDetailDto, Guid>
    {
        Task<ApiResponse<DirInfoDetailDto>> GetDirInfoAsync(QueryParameter queryParameter);
    }
}
