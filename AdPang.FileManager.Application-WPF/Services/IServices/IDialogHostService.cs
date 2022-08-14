using System.Threading.Tasks;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.Services.IServices
{
    public interface IDialogHostService : IDialogService
    {
        Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root");
    }
}
