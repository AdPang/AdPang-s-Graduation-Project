using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using Newtonsoft.Json;
using RestSharp;

namespace HttpRequestClient.Services
{
    public class HttpRestClient
    {
        private readonly IAuthModel authModel;
        public readonly string apiUrl;
        protected readonly RestClient client;
        public HttpRestClient(IAuthModel authModel, string apiUrl)
        {
            this.authModel = authModel;
            this.apiUrl = apiUrl;
            client = new RestClient();
        }
        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest, bool hasAuth = true, ICollection<KeyValuePair<string, string>>? headers = null)
        {

            RestRequest request = new RestRequest(baseRequest.Method);
            if (headers != null)
                request.AddHeaders(headers);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            if (hasAuth) AddAuthorize(request);
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                await UpdateAuthAsync();
            return new ApiResponse(false, response.ErrorMessage);
        }



        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest, bool hasAuth = true)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            if (hasAuth) AddAuthorize(request);
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                await UpdateAuthAsync();
            return new ApiResponse<T>(false, response.ErrorMessage);

        }



        private void AddAuthorize(RestRequest restRequest)
        {
            if (authModel.JwtStr != null) restRequest.AddHeader("Authorization", $"Bearer {authModel.JwtStr}");
        }

        private async Task UpdateAuthAsync()
        {
            if (authModel.UserName == null || authModel.Password == null) return;
            var baseRequest = new BaseRequest
            {
                Method = Method.GET,
                Route = $"api/oauth/login?username={authModel.UserName}&password={authModel.Password}",
                Parameter = new UserDto { UserName = authModel.UserName, Password = authModel.Password },
                ContentType = "application/json",
            };
            var request = new RestRequest(baseRequest.Method);
            //request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter));
            client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            var response = await client.ExecuteAsync(request);
            var authResult = JsonConvert.DeserializeObject<ApiResponse<AuthDto>>(response.Content);
            if (authResult == null || !authResult.Status) return;
            authModel.JwtStr = authResult.Result.JwtStr;
            authModel.UpdateTime = DateTime.Now;
        }
    }
}
