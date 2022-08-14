using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.IRepositories.CloudSaved;
using AdPang.FileManager.IServices.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Services.CloudSaved
{
    public class DirService : BaseService<DirInfo>, IDirService
    {
        private readonly IDirInfoRepository baseRepository;

        public DirService(IDirInfoRepository baseRepository) : base(baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public Task<IList<DirInfo>> GetDirDetailListAsync(Expression<Func<DirInfo, bool>> predicate)
        {
            return baseRepository.GetDirDetailListAsync(predicate);
        }

        //public Task<DirInfo> GetRootDirInfosAsync(Guid userId)
        //{
        //    return baseRepository.GetRootDirInfosAsync(userId);
        //}
    }
}
