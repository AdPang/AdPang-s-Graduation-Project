using AdPang.FileManager.Models.FileManagerEntities;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
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
        public virtual ICollection<PrivateFileInfo> PrivateFileInfos { get; set; }
        public virtual ICollection<PrivateDiskInfo> PrivateDiskInfos { get; set; }
        public virtual ICollection<CloudFileInfo> CloudFileInfos { get; set; }
        public virtual ICollection<DirInfo> DirInfos { get; set; }
        public virtual ICollection<UserPrivateFileInfo> UserPrivateFileInfos { get; set; }
        public virtual ICollection<SharedFileInfo> SharedFileInfos { get; set; }
    }
}
