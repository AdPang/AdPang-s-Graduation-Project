using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdPang.FileManager.EntityFrameworkCore.EntityTypeConfigs
{
    internal class DirInfoConfig : IEntityTypeConfiguration<DirInfo>
    {
        public void Configure(EntityTypeBuilder<DirInfo> builder)
        {
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.ParentDirInfo).WithMany(x => x.ChildrenDirInfo).HasForeignKey(x=>x.ParentDirInfoId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.ChildrenFileInfo).WithOne(x => x.CurrentDirectoryInfo).HasForeignKey(x => x.CurrentDirectoryInfoId).OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
