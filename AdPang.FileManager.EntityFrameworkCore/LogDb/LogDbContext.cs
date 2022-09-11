using AdPang.FileManager.Models.LogEntities;
using Microsoft.EntityFrameworkCore;

namespace AdPang.FileManager.EntityFrameworkCore.LogDb
{
    public class LogDbContext : DbContext
    {
        public virtual DbSet<ActionLog> ActionLog { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {

        }
    }
}
