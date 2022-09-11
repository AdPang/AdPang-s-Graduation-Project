using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdPang.FileManager.EntityFrameworkCore.EntityTypeConfigs
{
    internal class PrivateDiskInfoConfig : IEntityTypeConfiguration<PrivateDiskInfo>
    {
        public void Configure(EntityTypeBuilder<PrivateDiskInfo> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.PrivateDiskInfos).HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.DiskSN).IsRequired().HasColumnType("char").HasMaxLength(64);
            builder.Property(x => x.DiskName).IsRequired().IsUnicode(true).HasMaxLength(150);
            builder.HasIndex(x => x.DiskSN).IsUnique(true);

        }
    }
}
