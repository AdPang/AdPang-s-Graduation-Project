using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Models.LogEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdPang.FileManager.EntityFrameworkCore.IdentityDb
{
    public class IdentityDbContext : IdentityDbContext<User, Role, Guid>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }
    }
}
