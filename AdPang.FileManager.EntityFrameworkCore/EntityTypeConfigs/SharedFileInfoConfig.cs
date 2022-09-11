using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdPang.FileManager.EntityFrameworkCore.EntityTypeConfigs
{
    public class SharedFileInfoConfig : IEntityTypeConfiguration<SharedFileInfo>
    {
        public void Configure(EntityTypeBuilder<SharedFileInfo> builder)
        {
            builder.HasOne(x => x.ShardByUser).WithMany(x => x.SharedFileInfos).HasForeignKey(x => x.ShardByUserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.DirInfo).WithMany().HasForeignKey(x => x.DirId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.SingleFileInfo).WithMany().HasForeignKey(x => x.SingleFileId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.SharedPassword).HasMaxLength(6);
            builder.Property(x => x.SharedDesc).HasMaxLength(1024);
        }
    }
}
