using System;
using AdPang.FileManager.Models.LogEntities.Base;

namespace AdPang.FileManager.Models.LogEntities
{
    public class ActionLog : BaseModel<long>
    {
        public Guid? OperaByUserId { get; set; }
        public string? RequsetUrl { get; set; }
        public string? ResultJson { get; set; }
        public string? RequestIPAddress { get; set; }
        public string? RequestParameter { get; set; }
    }
}
