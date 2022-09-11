using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdPang.FileManager.EntityFrameworkCore.EntityTypeConfigs
{
    internal class CloudFileInfoConfig : IEntityTypeConfiguration<CloudFileInfo>
    {
        public void Configure(EntityTypeBuilder<CloudFileInfo> builder)
        {
            builder.HasOne(x => x.UploadBy).WithMany().HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.FileMD5Str).HasMaxLength(150).HasColumnName("char");

            //builder.HasMany(x => x.Users).WithMany(x => x.CloudFileInfos).UsingEntity(x => x.ToTable("UserCloudSavedFileRealition"));

        }
    }
}
