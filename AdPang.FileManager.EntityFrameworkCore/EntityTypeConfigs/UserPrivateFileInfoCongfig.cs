using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.EntityFrameworkCore.EntityTypeConfigs
{
    internal class UserPrivateFileInfoCongfig : IEntityTypeConfiguration<UserPrivateFileInfo>
    {
        public void Configure(EntityTypeBuilder<UserPrivateFileInfo> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.UserPrivateFileInfos).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.CurrentDirectoryInfo).WithMany(x => x.ChildrenFileInfo).HasForeignKey(x => x.CurrentDirectoryInfoId).OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
