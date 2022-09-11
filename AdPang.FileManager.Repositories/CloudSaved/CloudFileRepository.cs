using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.IRepositories.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Repositories.Base;

namespace AdPang.FileManager.Repositories.CloudSaved
{
    public class CloudFileRepository : BaseRepository<CloudFileInfo>, ICloudFileRepository
    {
        public CloudFileRepository(FileManagerDbContext context) : base(context)
        {
        }
    }
}
