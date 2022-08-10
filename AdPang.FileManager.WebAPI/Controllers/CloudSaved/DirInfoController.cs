using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.IServices.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AdPang.FileManager.Shared.Paremeters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdPang.FileManager.WebAPI.Controllers.CloudSaved
{
    /// <summary>
    /// 个人云盘文件夹控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DirInfoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDirService dirService;
        private readonly RequestInfoModel requestInfoModel;
        private readonly UserManager<User> userManager;


        /// <summary>
        /// 构造
        /// </summary>
        public DirInfoController(IMapper mapper, IDirService dirService, RequestInfoModel requestInfoModel, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.dirService = dirService;
            this.requestInfoModel = requestInfoModel;
            this.userManager = userManager;
        }


        /// <summary>
        /// 获取当前账户的文件夹信息
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<IPagedList<DirInfoDetailDto>>> GetAllDirAsync([FromQuery]QueryParameter queryParameter)
        { 
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<IPagedList<DirInfoDetailDto>>(false, "发生错误!");
            
            return new ApiResponse<IPagedList<DirInfoDetailDto>>(true, await GetDirInfosByUser((Guid)userId, queryParameter));

        }

        

        /// <summary>
        /// 添加文件夹信息
        /// </summary>
        /// <param name="dirInfoDto"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse> AddDirAsync(DirInfoDto dirInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误！");
            var dirInfo = mapper.Map<DirInfo>(dirInfoDto);
            dirInfo.UserId = (Guid)userId;
            dirInfo.UpdateTime = DateTime.Now;
            await dirService.InsertAsync(dirInfo,true);
            return new ApiResponse(true, "添加成功！");
        }

        
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dirId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{dirId}")]
        [Authorize(Roles ="Ordinary")]
        public async Task<ApiResponse> DeleteDir(Guid dirId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误!");
            var dirInfos = await dirService.GetListAsync(x => x.UserId == userId);
            var dir = dirInfos.Where(x => x.Id.Equals(dirId)).FirstOrDefault();
            if (dir == null) return new ApiResponse(false, "文件夹不存在！");
            Merge(dir, dirInfos);
            try
            {
                var deleteDirs = new List<DirInfo>();
                GetDeleteDirs(dir, deleteDirs);
                await dirService.DeleteManyAsync(deleteDirs,true);
                return new ApiResponse(true, "删除成功");
            }
            catch (Exception e)
            {
                return new ApiResponse(false, e.Message);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="dirInfoDto"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        [Authorize(Roles ="Ordinary")]
        public async Task<ApiResponse> EditDir(DirInfoDto dirInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误");

            var dirInfo = await dirService.FindAsync(x => x.Id.Equals(dirInfoDto.Id) && x.UserId.Equals(userId));
            if (dirInfo == null) return new ApiResponse(false, "未找到");
            dirInfo.ParentDirInfoId = dirInfoDto.ParentDirInfoId;
            if(dirInfoDto.ParentDirInfoId != null)
            {
                var parentDir = await dirService.FindAsync(x => x.Id.Equals(dirInfoDto.ParentDirInfoId) && x.UserId.Equals(userId));
                if (parentDir == null) return new ApiResponse(false, "父文件夹不存在！");
                dirInfo.ParentDirInfo = parentDir;
                dirInfo.ParentDirInfoId = parentDir.Id;
            }
            dirInfo.DirName = dirInfoDto.DirName;
            dirInfo.UpdateTime = DateTime.Now;
            await dirService.UpdateAsync(dirInfo, true);
            return new ApiResponse(true, "更新成功");
        }

        /// <summary>
        /// 获取指定用户的所有文件夹详情信息（包含层级关系）（管理员）
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("Get/{userId}/Admin")]
        [Authorize(Roles ="Admin")]
        public async Task<ApiResponse<IPagedList<DirInfoDetailDto>>> GetAllDirAsync([FromQuery] QueryParameter queryParameter, Guid userId) =>
             new ApiResponse<IPagedList<DirInfoDetailDto>>(true, await GetDirInfosByUser(userId, queryParameter));

        /// <summary>
        /// 获取所有用户的文件夹列表（管理员）
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [HttpGet("GetAll/Admin")]
        [Authorize(Roles ="Admin")]
        public async Task<ApiResponse<IPagedList<UserInfoContainDirInfoDto>>> GetAllUserDirAsync([FromQuery] QueryParameter queryParameter)
        {
            var users = await userManager.Users.Where(x => queryParameter.Search == null || queryParameter.Search.Contains(x.UserName) || x.UserName.Contains(queryParameter.Search)).Take(queryParameter.PageSize).Skip(queryParameter.PageSize * queryParameter.PageIndex).Include(x => x.DirInfos).ThenInclude(x=>x.ChildrenFileInfo).ToListAsync();
            foreach (var user in users)
            {
                var roots = user.DirInfos.Where(x => x.ParentDirInfoId == null).ToList();
                foreach (var root in roots)
                {
                    Merge(root, user.DirInfos);
                }
                user.DirInfos = roots;
            }
            var userDtos = mapper.Map<List<UserInfoContainDirInfoDto>>(users);
            return new ApiResponse<IPagedList<UserInfoContainDirInfoDto>>(true, new PagedList<UserInfoContainDirInfoDto>(userDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }

        /// <summary>
        /// 获取即将删除的文件夹所有文件夹信息
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="deleteDirs"></param>
        private void GetDeleteDirs(DirInfo parent, IList<DirInfo> deleteDirs)
        {
            foreach (var child in parent.ChildrenDirInfo)
            {
                if (child == null) continue;
                GetDeleteDirs(child, deleteDirs);
            }
            deleteDirs.Add(parent);
        }

        /// <summary>
        /// 根据userId查询
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        private async Task<IPagedList<DirInfoDetailDto>> GetDirInfosByUser(Guid userId, QueryParameter queryParameter)
        {
            var dirInfos = await dirService.GetDirDetailListAsync(x => x.UserId.Equals(userId));
            var roots = dirInfos.Where(x => x.ParentDirInfoId == null).Skip(queryParameter.PageIndex * queryParameter.PageSize).Take(queryParameter.PageSize).ToList();
            foreach (var root in roots)
            {
                Merge(root, dirInfos);
            }
            var disInfoDtos = mapper.Map<List<DirInfoDetailDto>>(roots);
            return new PagedList<DirInfoDetailDto>(disInfoDtos, queryParameter.PageIndex, queryParameter.PageSize, default);
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="dirInfos"></param>
        private static void Merge(DirInfo parent, IEnumerable<DirInfo> dirInfos)
        {
            var children = dirInfos.Where(x => x.ParentDirInfoId == parent.Id);
            foreach (var child in children)
            {
                parent.ChildrenDirInfo.Add(child);
                Merge(child, dirInfos);
            }
        }

    }
}
