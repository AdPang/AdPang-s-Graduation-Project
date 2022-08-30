using System.Net;
using System.Net.Http.Handlers;
using System.Net.Http.Headers;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using HttpRequestClient.Services.IRequestServices;
using Newtonsoft.Json;

namespace HttpRequestClient.Services.RequestServices
{
    public class FileRequestService : BaseRequestService<CloudFileInfoDetailDto, Guid>,IFileRequestService
    {
        private readonly HttpRestClient client;
        private readonly IAuthModel authModel;

        public FileRequestService(HttpRestClient client, IAuthModel authModel) : base(client, "FileManage")
        {
            this.client = client;
            this.authModel = authModel;
        }

        public async Task<ApiResponse<CloudFileInfoDto>> AddFileToCloud(Guid fileId, Guid dirId, UserPrivateFileInfoDto userPrivateFileInfoDto)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/add/{fileId}/{dirId}";
            request.Parameter = userPrivateFileInfoDto;
            return await client.ExecuteAsync<CloudFileInfoDto>(request);

        }

        public async Task<string> DownloadFile(UserPrivateFileInfoDto userPrivateFileInfo, EventHandler<HttpProgressEventArgs> httpProgressEventHandler, string downloadPath)
        {
            HttpClientHandler handler = new();
            ProgressMessageHandler progressMessageHandler = new(handler);
            progressMessageHandler.HttpReceiveProgress += httpProgressEventHandler;
            try
            {
                using HttpClient httpClient = new(progressMessageHandler);
                string uri = client.apiUrl +"api/"+ serviceName + @$"/file/{userPrivateFileInfo.Id}";
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authModel.JwtStr);

                //var netStream = await httpClient.GetStreamAsync(uri);
                var t = await httpClient.GetAsync(uri);
                if (t.StatusCode.Equals(HttpStatusCode.BadRequest) || t.StatusCode.Equals(HttpStatusCode.NotFound)) 
                {
                    return string.Empty;
                }
                var fileFullName = downloadPath + "\\" + userPrivateFileInfo.FileName;
                for (int i = 1; File.Exists(fileFullName); i++) 
                {
                    fileFullName = downloadPath + "\\" + $"({i})" + userPrivateFileInfo.FileName;
                }
                using var filestream = new FileStream(fileFullName, FileMode.Create);
                var netStream = await t.Content.ReadAsStreamAsync();
                await netStream.CopyToAsync(filestream);//写入文件

                return fileFullName;
            }
            catch (Exception )
            {
                return string.Empty;
            }
        }

        public async Task<ApiResponse<IEnumerable<KeyValuePair<string, string>>>> UploadFile(FileInfo file, EventHandler<HttpProgressEventArgs> httpProgressEventHandler)
        {
            HttpClientHandler handler = new HttpClientHandler();
            ProgressMessageHandler progressMessageHandler = new ProgressMessageHandler(handler);
            progressMessageHandler.HttpSendProgress += httpProgressEventHandler;
            try
            {
                using (var httpClient = new HttpClient(progressMessageHandler))
                {
                    var content = new MultipartFormDataContent();
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authModel.JwtStr);
                    content.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(file.FullName)), "files", file.Name);
                    var uri = client.apiUrl +"api/"+ serviceName + "/FileUpload";
                    var result = await httpClient.PostAsync(uri, content).Result.Content.ReadAsStringAsync();

                    var apiResponse =  JsonConvert.DeserializeObject<ApiResponse<IEnumerable<KeyValuePair<string, string>>>>(result);
                    return apiResponse ?? new ApiResponse<IEnumerable<KeyValuePair<string, string>>>(false, "");
                }
            }
            catch (Exception e)
            {
                return new ApiResponse<IEnumerable<KeyValuePair<string, string>>>(false, e.Message);
            }

        }

    }
}
