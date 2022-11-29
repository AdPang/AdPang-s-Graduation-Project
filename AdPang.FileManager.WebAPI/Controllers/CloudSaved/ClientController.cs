using AdPang.FileManager.Common.Helper;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers.CloudSaved
{
    /// <summary>
    /// 客户端控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        /// <summary>
        /// 客户端下载
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Ordinary")]
        public FileResult DownloadClient()
        {
            var realFileInfo = new System.IO.FileInfo("D:\\Data\\FileManagerClient.zip");
            var stream = System.IO.File.OpenRead(realFileInfo.FullName);  //创建文件流
            string contentType = MimeMapping.GetMimeMapping(realFileInfo.Name);
            //Console。WriteLine("{0}'s MIME TYPE: {1}", file, contentType);
            return File(stream, contentType, "Client.zip");
        }
    }
}
