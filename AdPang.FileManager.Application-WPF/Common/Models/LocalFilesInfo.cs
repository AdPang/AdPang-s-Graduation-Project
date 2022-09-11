using System.IO;
using Prism.Mvvm;

namespace AdPang.FileManager.Application_WPF.Common.Models
{
    public class LocalFilesInfo : BindableBase
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; RaisePropertyChanged(); }
        }

        private FileInfo file;

        public FileInfo File
        {
            get { return file; }
            set { file = value; RaisePropertyChanged(); }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; RaisePropertyChanged(); }
        }

        private string fullName;

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; RaisePropertyChanged(); }
        }

    }
}
