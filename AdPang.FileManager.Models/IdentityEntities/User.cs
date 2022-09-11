using System;
using System.Collections.Generic;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using Microsoft.AspNetCore.Identity;

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
