using AdPang.FileManager.Shared.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class RoleDto : BaseDto<Guid>
    {
        public string Name { get; set; }
        //public string NormalizedName { get; set; }

    }
}
