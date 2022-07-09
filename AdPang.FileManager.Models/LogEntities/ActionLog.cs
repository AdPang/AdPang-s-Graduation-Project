
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Models.LogEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
