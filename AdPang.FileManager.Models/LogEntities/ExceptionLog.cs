using AdPang.FileManager.Models.LogEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
