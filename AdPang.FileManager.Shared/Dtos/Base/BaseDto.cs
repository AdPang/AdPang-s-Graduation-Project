using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdPang.FileManager.Shared.Dtos.Base
{
    public class BaseDto<Tkey> where Tkey : struct
    {
        public Tkey? Id { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 实现通知更新
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
