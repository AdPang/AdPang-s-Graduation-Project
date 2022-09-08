using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace AdPang.FileManager.Application_WPF.Common.Models
{
    public class LocalDiskInfo : BindableBase
    {
        private string drive;
        private string driveSN;

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; RaisePropertyChanged(); }
        }

        public string Drive
        {
            get { return drive; }
            set { drive = value; RaisePropertyChanged(); }
        }


        public string DriveSN
        {
            get { return driveSN; }
            set { driveSN = value; RaisePropertyChanged(); }
        }

    }
}
