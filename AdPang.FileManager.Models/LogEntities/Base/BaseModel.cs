using System;

namespace AdPang.FileManager.Models.LogEntities.Base
{
    public class BaseModel<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
        public DateTime CreatTime { get; set; } = DateTime.Now;


    }
}
