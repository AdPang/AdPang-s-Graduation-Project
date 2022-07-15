using AdPang.FileManager.Common.Helper;
using AdPang.FileManager.Common.RequestInfoModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VerfiyCodeController : ControllerBase
    {
        private readonly VerifyCodeHelper codeHelper;
        private readonly RequestInfoModel requestInfoModel;

        public VerfiyCodeController(VerifyCodeHelper codeHelper, RequestInfoModel requestInfoModel)
        {
            this.codeHelper = codeHelper;
            this.requestInfoModel = requestInfoModel;
        }

        [HttpGet]
        public ActionResult GetGraphic4VerfiyCode(Guid seed)
        {
            try
            {
                string verfiyCode = codeHelper.Creat4VerfiyCode(seed);
                var codeImage = VerifyCodeHelper.CreateByteByImgVerifyCode(verfiyCode, 100, 50); 
                return File(codeImage, @"image/jpeg");
            }
            catch (Exception)
            {
                return BadRequest("生成验证码错误");
            }
           
        }



        [HttpGet]
        public ActionResult TestGetMethod()
        {
            if (!requestInfoModel.IsVerify) return BadRequest("验证码未通过");

            return Ok("11111111111111111111");
        }
    }
}
