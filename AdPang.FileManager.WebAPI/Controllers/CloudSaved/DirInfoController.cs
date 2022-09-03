using System.Collections.ObjectModel;
using System.Linq;
using AdPang.FileManager.Common.Extensions;
using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.IServices.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Services.CloudSaved;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AdPang.FileManager.Shared.Paremeters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdPang.FileManager.WebAPI.Controllers.CloudSaved
{
    /// <summary>
    /// 个人云盘文件夹控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DirInfoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDirService dirService;
        private readonly IUserPrivateFileService userPrivateFileService;
        private readonly ISharedFileInfoService sharedFileInfoService;
        private readonly RequestInfoModel requestInfoModel;
        private readonly UserManager<User> userManager;


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="dirService"></param>
        /// <param name="userPrivateFileService"></param>
        /// <param name="sharedFileInfoService"></param>
        /// <param name="requestInfoModel"></param>
        /// <param name="userManager"></param>
        public DirInfoController(IMapper mapper, IDirService dirService,IUserPrivateFileService userPrivateFileService,ISharedFileInfoService sharedFileInfoService,RequestInfoModel requestInfoModel, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.dirService = dirService;
            this.userPrivateFileService = userPrivateFileService;
            this.sharedFileInfoService = sharedFileInfoService;
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
        public async Task<ApiResponse<DirInfoDetailDto>> GetAllDirAsync([FromQuery]QueryParameter queryParameter)
        { 
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<DirInfoDetailDto>(false, "发生错误!");
            var dirDtos = await GetDirInfosByUserAsync((Guid)userId, queryParameter);
            var dirResult = new DirInfoDetailDto { Id = Guid.NewGuid(), ChildrenDirInfo = new ObservableCollection<DirInfoDetailDto>(), DirName = "云盘文件", ParentDirInfoId = null };
            foreach (var item in dirDtos.Items)
            {
                item.ParentDirInfoId = dirResult.Id;
                dirResult.ChildrenDirInfo.Add(item);
            }
            return new ApiResponse<DirInfoDetailDto>(true, dirResult);

        }

        /// <summary>
        /// 获取指定文件夹详情
        /// </summary>
        /// <param name="dirId"></param>
        /// <returns></returns>
        [HttpGet("Get/{dirId}")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<DirInfoDetailDto>> GetAsync(Guid dirId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<DirInfoDetailDto>(false, "发生错误！");
            var dirInfos = await dirService.GetDirDetailListAsync(x => x.UserId.Equals(userId));
            var root = dirInfos.Where(x => x.Id == dirId).FirstOrDefault();
            if (root == null) return new ApiResponse<DirInfoDetailDto>(false, "未找到该文件夹！");
            dirInfos.Merge(root);
            var disInfoDto = mapper.Map<DirInfoDetailDto>(root);
            return new ApiResponse<DirInfoDetailDto>(true, disInfoDto);
        }

        /// <summary>
        /// 添加文件夹信息
        /// </summary>
        /// <param name="dirInfoDto"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<DirInfoDetailDto>> AddDirAsync(DirInfoDetailDto dirInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<DirInfoDetailDto>(false, "发生错误！");
            var dirInfo = mapper.Map<DirInfo>(dirInfoDto);
            dirInfo.UserId = (Guid)userId;
            dirInfo.UpdateTime = DateTime.Now;
            var dir = await dirService.InsertAsync(dirInfo,true);
            return new ApiResponse<DirInfoDetailDto>(true, mapper.Map<DirInfoDetailDto>(dir));
        }

        
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dirId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{dirId}")]
        [Authorize(Roles ="Ordinary")]
        public async Task<ApiResponse> DeleteDirAsync(Guid dirId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误!");
            var dirInfos = await dirService.GetDirDetailListAsync(x => x.UserId == userId);
            var dir = dirInfos.Where(x => x.Id.Equals(dirId)).FirstOrDefault();
            if (dir == null) return new ApiResponse(false, "文件夹不存在！");
            dirInfos.Merge(dir);
            try
            {
                var deleteDirs = new List<DirInfo>();
                var deleteFiles = new List<UserPrivateFileInfo>();
                GetDeleteDirs(dir, deleteDirs, deleteFiles);

                var dirIds = deleteDirs.Select(x => x.Id).ToList();
                var fileIds = deleteFiles.Select(x => x.Id).ToList();

                var deleteShareList = await sharedFileInfoService.GetListAsync(x => (x.DirId != null && dirIds.Contains((Guid)x.DirId)) || (x.SingleFileId != null && fileIds.Contains((Guid)x.SingleFileId)));

                await sharedFileInfoService.DeleteManyAsync(deleteShareList, true);
                await userPrivateFileService.DeleteManyAsync(deleteFiles, true);
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
        public async Task<ApiResponse<DirInfoDetailDto>> EditDirAsync(DirInfoDetailDto dirInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<DirInfoDetailDto>(false, "发生错误");

            var dirInfo = await dirService.FindAsync(x => x.Id.Equals(dirInfoDto.Id) && x.UserId.Equals(userId));
            if (dirInfo == null) return new ApiResponse<DirInfoDetailDto>(false, "未找到");
            dirInfo.ParentDirInfoId = dirInfoDto.ParentDirInfoId;
            if(dirInfoDto.ParentDirInfoId != null)
            {
                var parentDir = await dirService.FindAsync(x => x.Id.Equals(dirInfoDto.ParentDirInfoId) && x.UserId.Equals(userId));
                if (parentDir == null) return new ApiResponse<DirInfoDetailDto>(false, "父文件夹不存在！");
                dirInfo.ParentDirInfo = parentDir;
                dirInfo.ParentDirInfoId = parentDir.Id;
            }
            dirInfo.DirName = dirInfoDto.DirName;
            dirInfo.UpdateTime = DateTime.Now;
            var dir = await dirService.UpdateAsync(dirInfo, true);
            return new ApiResponse<DirInfoDetailDto>(true, mapper.Map<DirInfoDetailDto>(dir));
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
             new ApiResponse<IPagedList<DirInfoDetailDto>>(true, await GetDirInfosByUserAsync(userId, queryParameter));

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
                    user.DirInfos.Merge(root);
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
        /// <param name="fileInfos"></param>
        private void GetDeleteDirs(DirInfo parent, IList<DirInfo> deleteDirs,List<UserPrivateFileInfo> fileInfos)
        {
            fileInfos.AddRange(parent.ChildrenFileInfo);
            foreach (var child in parent.ChildrenDirInfo)
            {
                if (child == null) continue;
                GetDeleteDirs(child, deleteDirs, fileInfos);
            }
            deleteDirs.Add(parent);
        }

        /// <summary>
        /// 根据userId查询
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        private async Task<IPagedList<DirInfoDetailDto>> GetDirInfosByUserAsync(Guid userId, QueryParameter queryParameter)
        {
            var dirInfos = await dirService.GetDirDetailListAsync(x => x.UserId.Equals(userId));
            var roots = dirInfos.Where(x => x.ParentDirInfoId == null).Skip(queryParameter.PageIndex * queryParameter.PageSize).Take(queryParameter.PageSize).ToList();
            foreach (var root in roots)
            {
                roots.Merge(root);
            }

            var disInfoDtos = mapper.Map<List<DirInfoDetailDto>>(roots);
            return new PagedList<DirInfoDetailDto>(disInfoDtos, queryParameter.PageIndex, queryParameter.PageSize, default);
        }


    }
}
