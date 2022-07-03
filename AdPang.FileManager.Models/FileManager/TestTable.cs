using AdPang.FileManager.Models.FileManager.Base;
using AdPang.FileManager.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Models.FileManager
{
    public class TestTable: BaseModel<Guid>
    {
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
    }
}
