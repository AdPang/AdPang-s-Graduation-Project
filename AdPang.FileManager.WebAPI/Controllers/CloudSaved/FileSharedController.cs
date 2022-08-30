using AdPang.FileManager.Common.Extensions;
using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.IServices.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using AdPang.FileManager.Shared.Paremeters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers.CloudSaved
{
    /// <summary>
    /// 文件分享控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileSharedController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISharedFileInfoService sharedFileInfoService;
        private readonly RequestInfoModel requestInfoModel;
        private readonly UserManager<User> userManager;
        private readonly IDirService dirService;
        private readonly IUserPrivateFileService userPrivateFileService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="sharedFileInfoService"></param>
        /// <param name="requestInfoModel"></param>
        /// <param name="userManager"></param>
        /// <param name="dirService"></param>
        /// <param name="userPrivateFileService"></param>
        public FileSharedController(IMapper mapper, ISharedFileInfoService sharedFileInfoService, RequestInfoModel requestInfoModel, UserManager<User> userManager, IDirService dirService, IUserPrivateFileService userPrivateFileService)
        {
            this.mapper = mapper;
            this.sharedFileInfoService = sharedFileInfoService;
            this.requestInfoModel = requestInfoModel;
            this.userManager = userManager;
            this.dirService = dirService;
            this.userPrivateFileService = userPrivateFileService;
        }

        /// <summary>
        /// 删除分享信息
        /// </summary>
        /// <param name="sharedId">分享信息Id</param>
        /// <returns></returns>
        [HttpDelete("Delete/{sharedId}")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse> DeleteSharedInfo(Guid sharedId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误！");
            var sharedInfo = await sharedFileInfoService.FindAsync(x => x.Id.Equals(sharedId) && x.ShardByUserId.Equals(userId));
            if (sharedInfo == null) return new ApiResponse(false, "未找到该分享信息!");
            await sharedFileInfoService.DeleteAsync(sharedInfo, true);
            return new ApiResponse(true, "删除成功！");
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="sharedId">分享信息Id</param>
        /// <param name="password">分享信息密码</param>
        /// <returns></returns>
        [HttpGet("Get/{sharedId}")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<DirInfoDetailDto>> GetFileShared(Guid sharedId, string? password)
        {
            var sharedFileInfo = await sharedFileInfoService.FindAsync(x => x.Id.Equals(sharedId));
            if (sharedFileInfo == null) return new ApiResponse<DirInfoDetailDto>(false, "不存在该分享！");
            if (sharedFileInfo.SharedPassword != null)
            {
                if (!sharedFileInfo.SharedPassword.Equals(password)) return new ApiResponse<DirInfoDetailDto>(false, "密码错误！");
            }
            if (sharedFileInfo.HasExpired && (DateTime.Now > sharedFileInfo.ExpiredTime)) return new ApiResponse<DirInfoDetailDto>(false, "分享已过期");
            DirInfo dirInfo;
            if (sharedFileInfo.IsSingleFile)
            {
                var singleFile = await userPrivateFileService.FindAsync(x => x.Id.Equals(sharedFileInfo.SingleFileId));
                dirInfo = new DirInfo
                {
                    DirName = "Root",
                    ChildrenFileInfo = new List<UserPrivateFileInfo>() { singleFile }
                };
            }
            else
            {
                var dirInfos = await dirService.GetDirDetailListAsync(x => x.UserId.Equals(sharedFileInfo.ShardByUserId));
                dirInfo = await dirService.FindAsync(x => x.Id.Equals(sharedFileInfo.DirId));
                dirInfos.Merge(dirInfo);
            }


            var dirInfoDto = mapper.Map<DirInfoDetailDto>(dirInfo);
            return new ApiResponse<DirInfoDetailDto>(true, dirInfoDto);
        }

        

        /// <summary>
        /// 创建分享
        /// </summary>
        /// <param name="beSharedInfoId">分享Id</param>
        /// <param name="isSingleFile">是否是单个文件分享</param>
        /// <param name="sharedPassword">分享密码</param>
        /// <param name="sharedDesc">文件分享描述</param>
        /// <param name="hasExpired">是否有过期时间</param>
        /// <param name="expiredTime">过期时间(天数)</param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<SharedFileInfoDto>> AddFileShared(Guid beSharedInfoId, bool isSingleFile, string? sharedPassword, string? sharedDesc, bool hasExpired = true, int expiredTime = 7)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<SharedFileInfoDto>(false, "发生错误！");
            var sharedInfo = new SharedFileInfo
            {
                SharedDesc = sharedDesc,
                HasExpired = hasExpired,
                ExpiredTime = DateTime.Now.AddDays(expiredTime),
                CreatTime = DateTime.Now,
                ShardByUserId = (Guid)userId,
                SharedPassword = sharedPassword,
                IsSingleFile = isSingleFile,
                UpdateTime = DateTime.Now,
            };
            if (isSingleFile)
            {
                var fileInfo = await userPrivateFileService.FindAsync(x => x.Id.Equals(beSharedInfoId) && userId.Equals(x.UserId));
                if (fileInfo == null) return new ApiResponse<SharedFileInfoDto>(false, "文件不存在！");
                sharedInfo.SingleFileInfo = fileInfo;
                sharedInfo.SingleFileId = fileInfo.Id;
            }
            else
            {
                var dirInfo = await dirService.FindAsync(x => x.Id.Equals(beSharedInfoId) && userId.Equals(x.UserId));
                if (dirInfo == null) return new ApiResponse<SharedFileInfoDto>(false, "文件不存在！");
                sharedInfo.DirInfo = dirInfo;
                sharedInfo.DirId = dirInfo.Id;
            }
            await sharedFileInfoService.InsertAsync(sharedInfo, true);
            var sharedInfoDto = mapper.Map<SharedFileInfoDto>(sharedInfo);
            return new ApiResponse<SharedFileInfoDto>(true, sharedInfoDto);
        }


        /// <summary>
        /// 删除分享信息（管理员）
        /// </summary>
        /// <param name="sharedId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{sharedId}/Admin")]
        [Authorize(Roles ="Admin")]
        public async Task<ApiResponse> DeleteSharedInfoAdmin(Guid sharedId)
        {
            var sharedInfo = await sharedFileInfoService.FindAsync(x => x.Id.Equals(sharedId));
            if (sharedInfo == null) return new ApiResponse(false, "未找到该分享信息!");
            await sharedFileInfoService.DeleteAsync(sharedInfo, true);
            return new ApiResponse(true, "删除成功！");
        }

        /// <summary>
        /// 获取分享信息列表（管理员）
        /// </summary>
        /// <param name="queryParameter">查询条件</param>
        /// <returns></returns>
        [HttpGet("GetAll/Admin")]
        [Authorize(Roles ="Admin")]
        public async Task<ApiResponse<IPagedList<SharedFileInfoDto>>> GetSharedInfos([FromQuery] QueryParameter queryParameter)
        {
            var sharedInfos = await sharedFileInfoService.GetPagedListAsync(x => queryParameter.Search == null || (x.SharedDesc != null && x.SharedDesc.Contains(queryParameter.Search) || (x.SharedDesc != null && queryParameter.Search.Contains(x.SharedDesc))), queryParameter.PageSize * queryParameter.PageIndex, queryParameter.PageSize, "CreatTime", default);
            if (sharedInfos.Count == 0 || sharedInfos == null) return new ApiResponse<IPagedList<SharedFileInfoDto>>(false, "未找到！");
            var sharedInfoDtos = mapper.Map<IList<SharedFileInfoDto>>(sharedInfos);
            return new ApiResponse<IPagedList<SharedFileInfoDto>>(true, new PagedList<SharedFileInfoDto>(sharedInfoDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }
    }
}
