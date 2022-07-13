using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdPang.FileManager.EntityFrameworkCore.EntityTypeConfigs
{
    internal class PrivateFileInfoConfig : IEntityTypeConfiguration<PrivateFileInfo>
    {
        public void Configure(EntityTypeBuilder<PrivateFileInfo> builder)
        {
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DiskInfo).WithMany(x => x.PrivateFiles)
                .HasForeignKey(x => x.DiskId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.FileMD5Str).HasMaxLength(256);

        }
    }
}
