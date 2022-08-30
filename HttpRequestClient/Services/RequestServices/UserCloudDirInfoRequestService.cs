using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Paremeters;
using HttpRequestClient.Services.IRequestServices;

namespace HttpRequestClient.Services.RequestServices
{
    public class UserCloudDirInfoRequestService : BaseRequestService<DirInfoDetailDto, Guid>, IUserCloudDirInfoRequestService
    {
        private readonly HttpRestClient client;

        public UserCloudDirInfoRequestService(HttpRestClient client) : base(client, "DirInfo")
        {
            this.client = client;
        }

        public async Task<ApiResponse<DirInfoDetailDto>> GetDirInfoAsync(QueryParameter queryParameter)
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"api/{serviceName}/Get?PageIndex={queryParameter.PageIndex}&PageSize={queryParameter.PageSize}",
                Parameter = queryParameter
            };
            return await client.ExecuteAsync<DirInfoDetailDto>(request);
        }
    }
}
