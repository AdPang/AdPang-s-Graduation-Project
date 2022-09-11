using System.Collections.ObjectModel;
using AdPang.FileManager.Application_WPF.Common.Models;

namespace AdPang.FileManager.Application_WPF.Services.IServices
{
    public interface ILocalInfoService
    {
        ObservableCollection<LocalDiskInfo> GetLocalDiskInfos();
    }
}
