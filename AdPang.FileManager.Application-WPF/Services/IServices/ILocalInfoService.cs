using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common.Models;

namespace AdPang.FileManager.Application_WPF.Services.IServices
{
    public interface ILocalInfoService
    {
        ObservableCollection<LocalDiskInfo> GetLocalDiskInfos();
    }
}
