using AdPang.FileManager.Models.FileManagerEntities.Common;
using AdPang.FileManager.Models.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdPang.FileManager.EntityFrameworkCore.EntityTypeConfigss
{
    public class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasMany(x => x.Roles).WithMany(x => x.Menus).UsingEntity(x => x.ToTable("RoleMenuRelation"));
            builder.HasOne(x => x.ParentMenu).WithMany(x => x.ChildrenMenu).HasForeignKey(x => x.ParentMenuId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
