using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using HttpRequestClient.Services.IRequestServices;

namespace HttpRequestClient.Services.RequestServices
{
    public class PrivateDiskRequestService : BaseRequestService<PrivateDiskInfoDto, Guid>, IPrivateDiskRequestService
    {
        public PrivateDiskRequestService(HttpRestClient client) : base(client, "PrivateDisk")
        {
        }
    }
}
