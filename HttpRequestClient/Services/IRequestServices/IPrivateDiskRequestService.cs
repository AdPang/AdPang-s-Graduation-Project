using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface IPrivateDiskRequestService : IBaseRequestService<PrivateDiskInfoDto,Guid>
    {

    }
}
