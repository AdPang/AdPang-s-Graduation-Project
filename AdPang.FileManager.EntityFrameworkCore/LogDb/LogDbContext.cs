using AdPang.FileManager.Models.FileManagerEntities;
using AdPang.FileManager.Models.LogEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
