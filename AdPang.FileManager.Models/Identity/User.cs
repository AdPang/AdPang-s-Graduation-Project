using AdPang.FileManager.Models.FileManager;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Models.Identity
{
    public class User : IdentityUser<Guid>
    {
        public virtual TestTable TestTable { get; set; }
    }
}
