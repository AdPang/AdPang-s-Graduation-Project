using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Handlers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface IFileRequestService : IBaseRequestService<CloudFileInfoDetailDto, Guid>
    {
        Task<string> DownloadFile(UserPrivateFileInfoDto userPrivateFileInfo, EventHandler<HttpProgressEventArgs> httpProgressEventHandler, string downloadPath);

        Task<ApiResponse<IEnumerable<KeyValuePair<string, string>>>> UploadFile(FileInfo file, EventHandler<HttpProgressEventArgs> httpProgressEventHandler);

        Task<ApiResponse<CloudFileInfoDto>> AddFileToCloud(Guid fileId, Guid dirId, UserPrivateFileInfoDto userPrivateFileInfoDto);
    }
}
