using System;

namespace AdPang.FileManager.Models.FileManagerEntities.Base
{
    public class BaseModel<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
        public DateTime CreatTime { get; set; } = DateTime.Now;
        public DateTime UpdateTime { get; set; }

    }
}
