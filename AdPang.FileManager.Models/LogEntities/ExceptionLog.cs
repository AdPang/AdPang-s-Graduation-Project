using System;
using AdPang.FileManager.Models.LogEntities.Base;

namespace AdPang.FileManager.Models.LogEntities
{
    public class ExceptionLog : BaseModel<long>
    {
        public Guid? OperaByUserId { get; set; }
        public string? RequsetUrl { get; set; }
        public string? RequestIPAddress { get; set; }
        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
    }
}
