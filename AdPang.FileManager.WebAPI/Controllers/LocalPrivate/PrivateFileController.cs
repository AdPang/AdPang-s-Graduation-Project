using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.IServices.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile;
using AdPang.FileManager.Shared.Paremeters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers.LocalPrivate
{
    /// <summary>
    /// 私有文件控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrivateFileController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPrivateFileService privateFileService;
        private readonly UserManager<User> userManager;
        private readonly RequestInfoModel requestInfoModel;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="privateFileService"></param>
        /// <param name="userManager"></param>
        /// <param name="requestInfoModel"></param>
        public PrivateFileController(IMapper mapper, IPrivateFileService privateFileService,UserManager<User> userManager,RequestInfoModel requestInfoModel)
        {
            this.mapper = mapper;
            this.privateFileService = privateFileService;
            this.userManager = userManager;
            this.requestInfoModel = requestInfoModel;
        }

        /// <summary>
        /// 获取当前账号的文件详情列表
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [Authorize(Roles ="Ordinary")]
        public async Task<ApiResponse<IPagedList<PrivateFileInfoDto>>> GetFileInfos([FromQuery] QueryParameter queryParameter)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<IPagedList<PrivateFileInfoDto>>(false, "发生错误");
            var fileInfos = await privateFileService.GetPagedListAsync(x => queryParameter.Search == null || queryParameter.Search.Contains(x.FileName) || x.FileName.Contains(queryParameter.Search) && x.UserId.Equals(userId), queryParameter.PageIndex * queryParameter.PageSize, queryParameter.PageSize, "CreatTime", default, false);
            var fileInfoDtos = mapper.Map<List<PrivateFileInfoDto>>(fileInfos);
            return new ApiResponse<IPagedList<PrivateFileInfoDto>>(true, new PagedList<PrivateFileInfoDto>(fileInfoDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }

        /// <summary>
        /// 根据文件id获取文件详情
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        [HttpGet("Get/{fileId}")]
        [Authorize(Roles ="Ordinary")]
        public async Task<ApiResponse<PrivateFileInfoDto>> GetFileInfo(Guid fileId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<PrivateFileInfoDto>(false, "发生错误");
            var fileInfo = await privateFileService.FindAsync(x => x.Id.Equals(fileId) && x.UserId.Equals(userId));
            if (fileInfo == null) return new ApiResponse<PrivateFileInfoDto>(false, "未找到改文件信息");
            var fileInfoDto = mapper.Map<PrivateFileInfoDto>(fileInfo);
            return new ApiResponse<PrivateFileInfoDto>(true, fileInfoDto);
        }

        /// <summary>
        /// 编辑文件
        /// </summary>
        /// <param name="privateFileInfoDto"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        [Authorize(Roles="Ordinary")]
        public async Task<ApiResponse<PrivateFileInfoDto>> EditFileInfo(PrivateFileInfoDto privateFileInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<PrivateFileInfoDto>(false, "发生错误");
            var oldFileInfo = await privateFileService.FindAsync(x => x.UserId.Equals(userId) && x.Id.Equals(privateFileInfoDto.Id));
            if (oldFileInfo == null) return new ApiResponse<PrivateFileInfoDto>(false, "未找到文件信息");
            oldFileInfo.FileLength = privateFileInfoDto.FileLength;
            oldFileInfo.FileName = privateFileInfoDto.FileName;
            oldFileInfo.FilePath = privateFileInfoDto.FilePath;
            oldFileInfo.FileMD5Str = privateFileInfoDto.FileMD5Str;
            oldFileInfo.UpdateTime = DateTime.Now;
            oldFileInfo.FileType = privateFileInfoDto.FileType;
            var result = await privateFileService.UpdateAsync(oldFileInfo, true);
            return new ApiResponse<PrivateFileInfoDto>(true, mapper.Map<PrivateFileInfoDto>(result));
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{fileId}")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse> DeleteFileInfo(Guid fileId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误");
            var fileInfo = await privateFileService.FindAsync(x => x.UserId.Equals(userId) && x.Id.Equals(fileId));
            if (fileInfo == null) return new ApiResponse(false, "未找到文件信息");
            await privateFileService.DeleteAsync(fileInfo, true);
            return new ApiResponse(true, "删除成功");
        }

        /// <summary>
        /// 添加文件信息
        /// </summary>
        /// <param name="privateFileInfoDto"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<PrivateFileInfoDto>> AddFileInfo(PrivateFileInfoDto privateFileInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (userId == null || user == null) return new ApiResponse<PrivateFileInfoDto>(false, "发生错误");
            var fileInfo = mapper.Map<PrivateFileInfo>(privateFileInfoDto);
            fileInfo.UserId = user.Id;
            fileInfo.UpdateTime = DateTime.Now;
            var result = await privateFileService.InsertAsync(fileInfo, true);
            return new ApiResponse<PrivateFileInfoDto>(true, mapper.Map<PrivateFileInfoDto>(result));
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="privateFileInfoDtos"></param>
        /// <returns></returns>
        [HttpPost("Adds")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<IList<PrivateFileInfoDto>>> AddFileInfos (IList<PrivateFileInfoDto> privateFileInfoDtos)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (userId == null || user == null) return new ApiResponse<IList<PrivateFileInfoDto>>(false, "发生错误");
            var fileInfos = mapper.Map<List<PrivateFileInfo>>(privateFileInfoDtos);
            fileInfos.ForEach(x =>
            {
                x.UserId = user.Id;
                x.UpdateTime = DateTime.Now;
            });
            await privateFileService.InsertManyAsync(fileInfos, true);
            var results = mapper.Map<IList<PrivateFileInfoDto>>(fileInfos);
            return new ApiResponse<IList<PrivateFileInfoDto>>(true, results);
        }


        /// <summary>
        /// 获取指定用户的文件信息列表（管理员）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll/{userId}/admin")]
        public async Task<ApiResponse<IPagedList<PrivateFileInfoDto>>> GetFileInfos(Guid userId, [FromQuery] QueryParameter queryParameter)
        {
            var fileInfos = await privateFileService.GetPagedListAsync(x => (queryParameter.Search == null || queryParameter.Search.Contains(x.FileName) || x.FileName.Contains(queryParameter.Search)) && x.UserId.Equals(userId), queryParameter.PageIndex * queryParameter.PageSize, queryParameter.PageSize, "CreatTime", default, false);
            var fileInfoDtos = mapper.Map<List<PrivateFileInfoDto>>(fileInfos);
            return new ApiResponse<IPagedList<PrivateFileInfoDto>>(true, new PagedList<PrivateFileInfoDto>(fileInfoDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }

        /// <summary>
        /// 获取全部的文件信息列表
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll/admin")]
        public async Task<ApiResponse<IPagedList<PrivateFileInfoDto>>> GetAllFileInfos([FromQuery] QueryParameter queryParameter)
        {
            var fileInfos = await privateFileService.GetPagedListAsync(x => queryParameter.Search == null || queryParameter.Search.Contains(x.FileName) || x.FileName.Contains(queryParameter.Search), queryParameter.PageIndex * queryParameter.PageSize, queryParameter.PageSize, "CreatTime", default, false);
            var fileInfoDtos = mapper.Map<List<PrivateFileInfoDto>>(fileInfos);
            return new ApiResponse<IPagedList<PrivateFileInfoDto>>(true, new PagedList<PrivateFileInfoDto>(fileInfoDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }
    }
}
