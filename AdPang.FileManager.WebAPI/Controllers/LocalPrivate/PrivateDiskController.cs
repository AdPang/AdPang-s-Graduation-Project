using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.IServices.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AdPang.FileManager.Shared.Paremeters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdPang.FileManager.WebAPI.Controllers.LocalPrivate
{
    /// <summary>
    /// 私有硬盘控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrivateDiskController : ControllerBase
    {
        private readonly RequestInfoModel requestInfoModel;
        private readonly IMapper mapper;
        private readonly IPrivateDiskService privateDiskService;
        private readonly UserManager<User> userManager;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="requestInfoModel"></param>
        /// <param name="mapper"></param>
        /// <param name="privateDiskService"></param>
        /// <param name="userManager"></param>

        public PrivateDiskController(RequestInfoModel requestInfoModel, IMapper mapper, IPrivateDiskService privateDiskService, UserManager<User> userManager)
        {
            this.requestInfoModel = requestInfoModel;
            this.mapper = mapper;
            this.privateDiskService = privateDiskService;
            this.userManager = userManager;
        }

        /// <summary>
        /// 获取当前用户下所有的硬盘列表
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<PagedList<PrivateDiskInfoDto>>> GetDiskListFromUserAsync([FromQuery] QueryParameter queryParameter)
        {
            if (requestInfoModel.CurrentOperaingUser == null) return new ApiResponse<PagedList<PrivateDiskInfoDto>>(false, "发生错误");
            var diskList = await privateDiskService.GetDiskPagedListAsync(x => (requestInfoModel.CurrentOperaingUser.Equals(x.UserId)) && (queryParameter.Search == null || queryParameter.Search.Contains(x.DiskName) || queryParameter.Search.Contains(x.DiskSN) || x.DiskSN.Contains(queryParameter.Search) || x.DiskName.Contains(queryParameter.Search)), queryParameter.PageSize * queryParameter.PageIndex, queryParameter.PageSize, "CreatTime", default);
            if (diskList == null) return new ApiResponse<PagedList<PrivateDiskInfoDto>>(false, "发生错误");
            var diskDtos = mapper.Map<List<PrivateDiskInfoDto>>(diskList);
            return new ApiResponse<PagedList<PrivateDiskInfoDto>>(true, new PagedList<PrivateDiskInfoDto>(diskDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }

        /// <summary>
        /// 添加硬盘
        /// </summary>
        /// <param name="privateDiskInfoDto"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<PrivateDiskInfoDto>> AddDiskAsync(PrivateDiskInfoDto privateDiskInfoDto)
        {
            var disk = mapper.Map<PrivateDiskInfo>(privateDiskInfoDto);
            var user = userManager.Users.Where(x => x.Id.Equals(requestInfoModel.CurrentOperaingUser)).FirstOrDefault();
            if (user == null) return new ApiResponse<PrivateDiskInfoDto>(false, "发生错误");
            var tempDisk = await privateDiskService.FindAsync(x => x.DiskSN == privateDiskInfoDto.DiskSN);
            if (tempDisk != null) return new ApiResponse<PrivateDiskInfoDto>(false, "该硬盘已经被注册！");
            disk.User = user;
            disk.UserId = user.Id;
            disk.UpdateTime = DateTime.Now;
            var result = await privateDiskService.InsertAsync(disk, true);
            return new ApiResponse<PrivateDiskInfoDto>(true, mapper.Map<PrivateDiskInfoDto>(result));
        }

        /// <summary>
        /// 修改硬盘信息
        /// </summary>
        /// <param name="privateDiskInfoDto"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<PrivateDiskInfoDto>> EditDiskAsync(PrivateDiskInfoDto privateDiskInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<PrivateDiskInfoDto>(false, "发生错误");
            var oldDisk = await privateDiskService.FindAsync(x => x.Id.Equals(privateDiskInfoDto.Id) && x.UserId.Equals(userId));
            if (oldDisk == null) return new ApiResponse<PrivateDiskInfoDto>(false, "硬盘不存在！");
            oldDisk.UpdateTime = DateTime.Now;
            oldDisk.DiskName = privateDiskInfoDto.DiskName;
            oldDisk.DiskSN = privateDiskInfoDto.DiskSN;
            var result = await privateDiskService.UpdateAsync(oldDisk, true);
            return new ApiResponse<PrivateDiskInfoDto>(true, mapper.Map<PrivateDiskInfoDto>(result));
        }

        /// <summary>
        /// 删除硬盘信息
        /// </summary>
        /// <param name="diskId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{diskId}")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse> DeleteDiskAsync(Guid diskId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误");
            var disk = await privateDiskService.FindAsync(x => x.Id.Equals(diskId) && userId.Equals(userId));
            if (disk == null) return new ApiResponse(false, "硬盘不存在！");
            await privateDiskService.DeleteAsync(disk, true);
            return new ApiResponse(true, "删除成功！");
        }

        /// <summary>
        /// 删除硬盘信息（管理员）
        /// </summary>
        /// <param name="diskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{diskId}/{userId}/admin")]
        public async Task<ApiResponse> DeleteDiskAsync(Guid diskId, Guid userId)
        {
            var disk = await privateDiskService.FindAsync(x => x.Id.Equals(diskId) && x.UserId.Equals(userId));
            if (disk == null) return new ApiResponse(false, "发生错误");
            await privateDiskService.DeleteAsync(disk, true);
            return new ApiResponse(true, "删除成功！");
        }
        /// <summary>
        /// 添加硬盘信息（管理员）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="diskInfoDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("Add/{userId}/admin")]
        public async Task<ApiResponse> AddDiskAsync(Guid userId, PrivateDiskInfoDto diskInfoDto)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse(false, "用户不存在");
            var disk = mapper.Map<PrivateDiskInfo>(diskInfoDto);
            disk.User = user;
            disk.UserId = userId;
            disk.UpdateTime = DateTime.Now;
            await privateDiskService.InsertAsync(disk, true);
            return new ApiResponse(true, "添加成功！");
        }

        /// <summary>
        /// 修改硬盘信息（管理员）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="diskInfoDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("Edit/{userId}/admin")]
        public async Task<ApiResponse> EditDiskAsync(Guid userId, PrivateDiskInfoDto diskInfoDto)
        {
            //var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            //if (user == null) return new ApiResponse(false, "用户不存在");
            var oldDisk = await privateDiskService.FindAsync(x => x.Id.Equals(diskInfoDto.Id) && x.UserId.Equals(userId));
            if (oldDisk == null) return new ApiResponse(false, "硬盘不存在！");
            oldDisk.UpdateTime = DateTime.Now;
            oldDisk.DiskName = diskInfoDto.DiskName;
            oldDisk.DiskSN = diskInfoDto.DiskSN;
            await privateDiskService.UpdateAsync(oldDisk, true);
            return new ApiResponse(true, "修改成功");
        }

        /// <summary>
        /// 获取所有硬盘信息，包括文件（管理员）
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll/admin")]
        public async Task<ApiResponse<IPagedList<DiskInfoContainFileInfoDto>>> GetDiskListAsync([FromQuery] QueryParameter queryParameter)
        {
            var disks = await privateDiskService.GetDiskInfoContainFileInfoPagedListAsync(x => queryParameter.Search == null || queryParameter.Search.Contains(x.DiskSN) || x.DiskSN.Contains(queryParameter.Search) || x.DiskName.Contains(queryParameter.Search) || queryParameter.Search.Contains(x.DiskName), queryParameter.PageIndex * queryParameter.PageSize, queryParameter.PageSize, "CreatTime");

            var diskDtos = mapper.Map<List<DiskInfoContainFileInfoDto>>(disks);
            return new ApiResponse<IPagedList<DiskInfoContainFileInfoDto>>(true, new PagedList<DiskInfoContainFileInfoDto>(diskDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }

        /// <summary>
        /// 获取指定用户的所有硬盘，包括文件（管理员）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetDiskDetial/{userId}/admin")]
        public ApiResponse<UserInfoContainDiskInfoDto> GetDiskDetail(Guid userId)
        {
            var user = userManager.Users.Include(x => x.PrivateDiskInfos).ThenInclude(x => x.PrivateFiles).Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<UserInfoContainDiskInfoDto>(false, "用户不存在");
            var userDto = mapper.Map<UserInfoContainDiskInfoDto>(user);
            return new ApiResponse<UserInfoContainDiskInfoDto>(true, userDto);
        }
        /// <summary>
        /// 获取所有用户的所有硬盘，包括文件（管理员）
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllDiskDetail/admin")]
        public async Task<ApiResponse<IPagedList<UserInfoContainDiskInfoDto>>> GetDiskDetail([FromQuery] QueryParameter queryParameter)
        {
            var users = await userManager.Users.Include(x => x.PrivateDiskInfos).ThenInclude(x => x.PrivateFiles).Where(x => queryParameter.Search == null || x.UserName.Contains(queryParameter.Search) || queryParameter.Search.Contains(x.UserName)).OrderBy(x => x.UserName).Take(queryParameter.PageSize).Skip(queryParameter.PageSize * queryParameter.PageIndex).ToListAsync();
            if (users == null || users.Count <= 0) return new ApiResponse<IPagedList<UserInfoContainDiskInfoDto>>(false, "发生错误");
            var userDto = mapper.Map<IList<UserInfoContainDiskInfoDto>>(users);
            return new ApiResponse<IPagedList<UserInfoContainDiskInfoDto>>(true, new PagedList<UserInfoContainDiskInfoDto>(userDto, queryParameter.PageIndex, queryParameter.PageSize, default));
        }

    }
}
