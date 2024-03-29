﻿using AdPang.FileManager.Common.Helper;
using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.IServices.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Common.Helper;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
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
    /// 文件管理控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileManageController : ControllerBase
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IUserPrivateFileService userPrivateFileService;
        private readonly IDirService dirService;
        private readonly IMapper mapper;
        private readonly ICloudFileService cloudFileService;
        
        private readonly UserManager<User> userManager;
        private readonly RequestInfoModel requestInfoModel;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="cloudFileService"></param>
        /// <param name="userManager"></param>
        /// <param name="requestInfoModel"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="userPrivateFileService"></param>
        /// <param name="dirService"></param>
        public FileManageController(IMapper mapper, ICloudFileService cloudFileService, UserManager<User> userManager, RequestInfoModel requestInfoModel, IWebHostEnvironment hostingEnvironment, IUserPrivateFileService userPrivateFileService, IDirService dirService)
        {
            this.mapper = mapper;
            this.cloudFileService = cloudFileService;
            this.userManager = userManager;
            this.requestInfoModel = requestInfoModel;
            this.hostingEnvironment = hostingEnvironment;
            this.userPrivateFileService = userPrivateFileService;
            this.dirService = dirService;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [Authorize(Roles = "Ordinary")]
        [HttpPost("FileUpload")]
        public async Task<ApiResponse<IEnumerable<KeyValuePair<string, string>>>> UploadFile(IList<IFormFile> files)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<IEnumerable<KeyValuePair<string, string>>>(false, "发生错误");
            var keyValue = new List<KeyValuePair<string, string>>();
            long size = files.Sum(f => f.Length);
            string webRootPath = hostingEnvironment.WebRootPath;
            string contentRootPath = hostingEnvironment.ContentRootPath;
            var fileInfoList = new List<CloudFileInfo>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = GetFileExt(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    var filePath = contentRootPath + "/upload/" + newFileName;
                    string md5 = string.Empty;
                    keyValue.Add(new KeyValuePair<string, string>(formFile.FileName, newFileName));

                    using (FileStream? stream = new(filePath, FileMode.OpenOrCreate))
                    {
                        await formFile.CopyToAsync(stream);

                    }

                    var fileInfo = new CloudFileInfo
                    {
                        FileLength = fileSize,
                        FileName = newFileName,
                        FileMD5Str = MD5Helper.GetMD5HashCodeFromFile(filePath),
                        CreatTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        FileType = fileExt,
                        FilePath = filePath,
                        UserId = (Guid)userId,
                        UserCount = 0
                    };
                    fileInfoList.Add(fileInfo);
                }
            }
            await cloudFileService.InsertManyAsync(fileInfoList, true);
            return new ApiResponse<IEnumerable<KeyValuePair<string,string>>>(true, keyValue);
        }

        /// <summary>
        /// 添加文件到云
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="dirId"></param>
        /// <param name="userPrivateFileInfoDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Ordinary")]
        [HttpPost("Add/{fileId}/{dirId}")]
        public async Task<ApiResponse> AddFileToCloud(Guid fileId,Guid dirId, UserPrivateFileInfoDto  userPrivateFileInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误！");
            //查找云盘是否存在文件
            var cloudFile = await cloudFileService.FindAsync(x => x.Id.Equals(fileId));
            if (cloudFile == null) return new ApiResponse(false, "文件不存在！");
            //查找用户是否含有该文件夹！
            var dirInfo = await dirService.FindAsync(x => x.Id.Equals(dirId) && userId.Equals(x.UserId));
            if (dirInfo == null) return new ApiResponse(false, "文件夹不存在！");
            var userPrivateFileInfo = mapper.Map<UserPrivateFileInfo>(userPrivateFileInfoDto);
            userPrivateFileInfo.UpdateTime = DateTime.Now;
            //将文件添加到指定目录
            userPrivateFileInfo.CurrentDirectoryInfoId = dirId;
            userPrivateFileInfo.UserId = (Guid)userId;
            userPrivateFileInfo.RealFileInfoId = fileId;
            await userPrivateFileService.InsertAsync(userPrivateFileInfo, true);
            //将用户保存文件数量+1
            cloudFile.UserCount++;
            await cloudFileService.UpdateAsync(cloudFile);
            return new ApiResponse(true, "添加成功！");

        }

        /// <summary>
        /// 修改文件（文件名、文件所在文件夹）
        /// </summary>
        /// <param name="dirId"></param>
        /// <param name="userPrivateFileInfoDto"></param>
        /// <returns></returns>
        [HttpPut("Edit/{dirId}")]
        [Authorize(Roles= "Ordinary")]
        public async Task<ApiResponse> EditFileInfo(Guid dirId,UserPrivateFileInfoDto userPrivateFileInfoDto)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误");
            var dirInfo = await dirService.FindAsync(x => x.Id.Equals(dirId) && x.UserId.Equals(userId));
            if (dirInfo == null) return new ApiResponse(false, "文件夹不存在！");
            var fileInfo = await userPrivateFileService.FindAsync(x => x.Id.Equals(userPrivateFileInfoDto.Id) && x.UserId.Equals(userId));
            if (fileInfo == null) return new ApiResponse(false, "文件不存在！");
            fileInfo.CurrentDirectoryInfoId = dirInfo.Id;
            fileInfo.FileName = userPrivateFileInfoDto.FileName;
            fileInfo.UpdateTime = DateTime.Now;
            await userPrivateFileService.UpdateAsync(fileInfo,true);
            return new ApiResponse(true, "修改完成！");
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{fileId}")]
        [Authorize(Roles= "Ordinary")]
        public async Task<ApiResponse> DeleteFileInfo(Guid fileId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse(false, "发生错误！");
            var fileInfo = await userPrivateFileService.FindAsync(x => x.Id.Equals(fileId) && userId.Equals(x.UserId));
            if (fileInfo == null) return new ApiResponse(false, "文件不存在！");
            var cloudFile = await cloudFileService.FindAsync(x => x.Id.Equals(fileInfo.RealFileInfoId));
            cloudFile.UserCount--;
            cloudFile.UpdateTime = DateTime.Now;
            await userPrivateFileService.DeleteAsync(fileInfo,true);
            await cloudFileService.UpdateAsync(cloudFile);  
            return new ApiResponse(true, "删除成功！");
        }

        /// <summary>
        /// 下载云盘文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet("file/{fileId}")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ActionResult> DownloadFile(Guid fileId)
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return BadRequest("发生错误!");

            var privateFile = await userPrivateFileService.FindAsync(x => x.Id.Equals(fileId) && x.UserId.Equals(userId));
            if (privateFile == null) return NotFound("未找到该文件！");
            var realFileInfo = await cloudFileService.FindAsync(x => x.Id.Equals(privateFile.RealFileInfoId));

            var stream = System.IO.File.OpenRead(realFileInfo.FilePath);  //创建文件流
            
            //FileStreamResult fileStreamResult = new FileStreamResult()
            //需要调用的时候
            string contentType = MimeMapping.GetMimeMapping(realFileInfo.FileName);
            //Console。WriteLine("{0}'s MIME TYPE: {1}", file, contentType);
            return File(stream, contentType, privateFile.FileName);
        }

        //[HttpGet("shared/{fileId}")]
        //[Authorize(Roles = "Ordinary")]
        //public async Task<ApiResponse> SharedFileInfo(SharedFileInfoDto sharedFileInfoDto)
        //{
        //    var userId = requestInfoModel.CurrentOperaingUser;
        //    if (userId == null) return new ApiResponse(false, "发生错误");

        //    var fileInfo = await userPrivateFileService.FindAsync(x => x.Id.Equals(sharedFileInfoDto.FileId) && x.UserId.Equals(userId));
        //    if (fileInfo == null) return new ApiResponse(false, "文件不存在！");

        //    return new ApiResponse(true, "");

        //}


        /// <summary>
        /// 获取所有用户（包含用户文件信息）
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpGet("GetUsers/Admin")]
        public async Task<ApiResponse<IPagedList<UserInfoContainCloudFileInfoDto>>> GetUserFileInfos([FromQuery] QueryParameter queryParameter)
        {
            var users = await userManager.Users.Where(x => queryParameter.Search == null || queryParameter.Search.Contains(x.UserName) || x.UserName.Contains(queryParameter.Search)).Include(x => x.UserPrivateFileInfos).ThenInclude(x=>x.RealFileInfo).OrderBy(x => x.UserName).Take(queryParameter.PageSize).Skip(queryParameter.PageSize * queryParameter.PageIndex).ToListAsync();
            var userDtos = mapper.Map<IList<UserInfoContainCloudFileInfoDto>>(users);
            return new ApiResponse<IPagedList<UserInfoContainCloudFileInfoDto>>(true, new PagedList<UserInfoContainCloudFileInfoDto>(userDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }


        /// <summary>
        /// 获取所有已保存到云的文件
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [HttpGet("GetAll/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<IPagedList<CloudFileInfoDetailDto>>> GetCloudFileInfos([FromQuery] QueryParameter queryParameter)
        {
            var cloudFileInfos = await cloudFileService.GetPagedListAsync(x => queryParameter.Search == null || (x.FileType != null && queryParameter.Search.Contains(x.FileType)) || (x.FileType != null && x.FileType.Contains(queryParameter.Search)), queryParameter.PageIndex * queryParameter.PageSize, queryParameter.PageSize, "CreatTime", default);
            if (cloudFileInfos.Count == 0 || cloudFileInfos == null) return new ApiResponse<IPagedList<CloudFileInfoDetailDto>>(false, "未找到");
            var cloudFileInfoDetailDtos = mapper.Map<IList<CloudFileInfoDetailDto>>(cloudFileInfos);
            return new ApiResponse<IPagedList<CloudFileInfoDetailDto>>(true, new PagedList<CloudFileInfoDetailDto>(cloudFileInfoDetailDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }


        /// <summary>
        /// 删除文件（管理员）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet("Delete/{userId}/{fileId}/Admin")]
        [Authorize(Roles="Admin")]
        public async Task<ApiResponse> DeleteFileInfo(Guid userId,Guid fileId)
        {
            var fileInfo = await userPrivateFileService.FindAsync(x => x.UserId.Equals(userId) && x.Id.Equals(fileId));
            if (fileInfo == null) return new ApiResponse(false, "文件不存在！");
            var cloudFile = await cloudFileService.GetAsync(x => x.Id.Equals(fileInfo.RealFileInfoId));
            cloudFile.UserCount--;
            cloudFile.UpdateTime = DateTime.Now;
            await cloudFileService.UpdateAsync(cloudFile, true);
            await userPrivateFileService.DeleteAsync(fileInfo);
            return new ApiResponse(true, "删除成功！");
        }

        



        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetFileExt(string fileName)
        {
            if (fileName.Contains('.'))
            {
                var ext = fileName.Split('.');
                return ext[^1];
            }
            return fileName;
        }
    }
}
