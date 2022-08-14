using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.IRepositories.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Repositories.CloudSaved
{
    public class DirRepository : BaseRepository<DirInfo>, IDirInfoRepository
    {
        public DirRepository(FileManagerDbContext context) : base(context)
        {
        }

        public async Task<IList<DirInfo>> GetDirDetailListAsync(Expression<Func<DirInfo, bool>> predicate)
        {
            return await DbContext().DirInfos.Where(predicate).Include(x => x.ChildrenFileInfo)/*.ThenInclude(x => x.RealFileInfo)*/.ToListAsync();
            
        }

        //public async Task<DirInfo?> GetRootDirInfosAsync(Guid userId)
        //{
        //    var dbcontext = DbContext();
        //    var root = dbcontext.DirInfos.Single(x => x.ParentDirInfoId == null && x.UserId == userId);
        //    if (root == null) return null;
        //}


        //static void PrintChildren(int indentLevel, FileManagerDbContext ctx, DirInfo parent)
        //{
        //    var children = ctx.DirInfos.Where(o => o.ParentDirInfo == parent).ToList();
        //    parent.ChildrenFileInfo = (ICollection<UserPrivateFileInfo>)children;
        //    foreach (var child in children)
        //    {
        //        //Console.WriteLine(new String('\t', indentLevel) + child);

        //        PrintChildren(indentLevel + 1, ctx, child);//递归调用,打印以当前节点的子节点
        //    }
        //}
    }
}
