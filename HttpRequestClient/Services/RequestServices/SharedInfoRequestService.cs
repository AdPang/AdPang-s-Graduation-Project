using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using HttpRequestClient.Services.IRequestServices;

namespace HttpRequestClient.Services.RequestServices
{
    public class SharedInfoRequestService : BaseRequestService<SharedFileInfoDetailDto, Guid>, ISharedInfoRequestService
    {
        public SharedInfoRequestService(HttpRestClient client) : base(client, "FileShared")
        {

        }


    }
}
