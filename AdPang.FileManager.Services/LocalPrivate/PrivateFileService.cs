using System.Linq.Expressions;
using AdPang.FileManager.IRepositories.LocalPrivate;
using AdPang.FileManager.IServices.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Services.Base;

namespace AdPang.FileManager.Services.LocalPrivate
{
    public class PrivateFileService : BaseService<PrivateFileInfo>, IPrivateFileService
    {
        private readonly IPrivateFileRepository privateFileRepository;

        public PrivateFileService(IPrivateFileRepository privateFileRepository) : base(privateFileRepository)
        {
            this.privateFileRepository = privateFileRepository;
        }

        public Task<List<PrivateFileInfo>> GetAllDuplicateAsync(Expression<Func<PrivateFileInfo, bool>> predicate)
        {
            return privateFileRepository.GetAllDuplicateAsync(predicate);
        }
    }
}
