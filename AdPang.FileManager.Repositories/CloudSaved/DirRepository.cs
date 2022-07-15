using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.IRepositories.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Repositories.CloudSaved
{
    public class DirRepository : BaseRepository<DirInfo>, IDirRepository
    {
        public DirRepository(FileManagerDbContext context) : base(context)
        {
        }
    }
}
