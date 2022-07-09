using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Models.LogEntities.Base
{
    public class BaseModel<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
        public DateTime CreatTime { get; set; } = DateTime.Now;


    }
}
