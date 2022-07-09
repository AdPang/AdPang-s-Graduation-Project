using AdPang.FileManager.Models.FileManagerEntities.Base;
using AdPang.FileManager.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Models.FileManagerEntities
{
    public class TestTable : BaseModel<Guid>
    {
        public Guid UserId { get; set; }
    }
}
