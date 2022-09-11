using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile;
using AdPang.FileManager.Shared.Paremeters;
using HttpRequestClient.Services.IRequestServices;

namespace HttpRequestClient.Services.RequestServices
{
    public class PrivateFileInfoRequestService : BaseRequestService<PrivateFileInfoDto, Guid>, IPrivateFileInfoRequestService
    {
        private readonly HttpRestClient client;

        public PrivateFileInfoRequestService(HttpRestClient client) : base(client, "PrivateFile")
        {
            this.client = client;
        }


        public async Task<ApiResponse<PagedList<PrivateFileInfoDto>>> GetAllFileInfoAsync(PrivateFileInfoQueryParameter privateFileInfoQueryParameter)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{serviceName}/GetAll" +
                $"?pageIndex={privateFileInfoQueryParameter.PageIndex}" +
                $"&pageSize={privateFileInfoQueryParameter.PageSize}" +
                $"&search={privateFileInfoQueryParameter.Search}" +
                $"&diskId={privateFileInfoQueryParameter.DiskId}" +
                $"&requestMode={privateFileInfoQueryParameter.RequestMode}";
            var result = await client.ExecuteAsync<PagedList<PrivateFileInfoDto>>(request);
            return result;
        }
    }
}
