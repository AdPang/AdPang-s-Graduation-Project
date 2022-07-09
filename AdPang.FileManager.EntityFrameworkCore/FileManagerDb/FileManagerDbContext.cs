using AdPang.FileManager.Models.FileManagerEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.EntityFrameworkCore.FileManagerDb
{
    public class FileManagerDbContext : DbContext
    {
        public virtual DbSet<TestTable> TestTable { get; set; }
        public FileManagerDbContext(DbContextOptions<FileManagerDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
