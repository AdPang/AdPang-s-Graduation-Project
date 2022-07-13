using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Models.LogEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AdPang.FileManager.EntityFrameworkCore.FileManagerDb
{
    public class FileManagerDbContext : IdentityDbContext<User, Role, Guid>
    {
        public FileManagerDbContext(DbContextOptions<FileManagerDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<PrivateFileInfo> PrivateFileInfos { get; set; }
        public virtual DbSet<PrivateDiskInfo> PrivateDiskInfos { get; set; }
        public virtual DbSet<CloudFileInfo> CloudFileInfos { get; set; }
        public virtual DbSet<DirInfo> DirInfos { get; set; }
        public virtual DbSet<UserPrivateFileInfo> UserPrivateFileInfos { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
