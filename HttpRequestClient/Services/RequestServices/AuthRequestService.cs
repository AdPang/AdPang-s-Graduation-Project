using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using HttpRequestClient.Services.IRequestServices;

namespace HttpRequestClient.Services.RequestServices
{
    public class AuthRequestService : IAuthRequestService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "OAuth";
        public AuthRequestService(HttpRestClient client)
        {
            this.client = client;
        }
        public async Task<ApiResponse<AuthDto>> LoginAsync(UserDto user)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{serviceName}/Login?username={user.UserName}&password={user.Password}";
            //request.Parameter = user;

            return await client.ExecuteAsync<AuthDto>(request, false);
        }

        public async Task<ApiResponse> RegisterAsync(UserDto user, Guid seed, string code)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/register";

            request.Parameter = user;
            return await client.ExecuteAsync(request, false, new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Seed",seed.ToString()),
                new KeyValuePair<string, string>("ImgVerifyCode", code)
            });
        }

        public string GetVerfiyCodeImgUrl(Guid seed)
        {
            return client.apiUrl + "api/VerfiyCode/GetImgVerfiyCode?seed=" + seed;
        }
    }
}
