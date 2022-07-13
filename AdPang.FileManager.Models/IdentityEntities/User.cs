using AdPang.FileManager.Models.FileManagerEntities;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.LogEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Models.IdentityEntities
{
    public class User : IdentityUser<Guid>
    {
        public virtual ICollection<CloudFileInfo> CloudFileInfos { get; set; } 
    }
}
