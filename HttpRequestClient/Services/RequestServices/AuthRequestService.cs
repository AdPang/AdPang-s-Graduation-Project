using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<ApiResponse> RegisterAsync(UserDto user)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/register";
            request.Parameter = user;
            return await client.ExecuteAsync(request, false);
        }
    }
}
