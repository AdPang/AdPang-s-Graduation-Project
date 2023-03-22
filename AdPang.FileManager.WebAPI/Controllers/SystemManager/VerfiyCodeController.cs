using AdPang.FileManager.Common.Helper.Mail;
using AdPang.FileManager.Common.Helper.VerifyCode;
using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers.SystemManager
{
    /// <summary>
    /// 验证码控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VerfiyCodeController : ControllerBase
    {
        private readonly ImgVerifyCodeHelper imgVerifyCodeHelper;
        private readonly RequestInfoModel requestInfoModel;
        private readonly MailVerifyCodeHelper mailVerifyCodeHelper;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="imgCodeHelper"></param>
        /// <param name="requestInfoModel"></param>
        /// <param name="mailVerifyCodeHelper"></param>
        public VerfiyCodeController(ImgVerifyCodeHelper imgCodeHelper, RequestInfoModel requestInfoModel, MailVerifyCodeHelper mailVerifyCodeHelper)
        {
            imgVerifyCodeHelper = imgCodeHelper;
            this.requestInfoModel = requestInfoModel;
            this.mailVerifyCodeHelper = mailVerifyCodeHelper;
        }
        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        [HttpGet("GetImgVerfiyCode")]
        public ActionResult GetGraphic4VerfiyCode(Guid seed)
        {
            try
            {
                string verfiyCode = imgVerifyCodeHelper.Creat4ImgVerfiyCode(seed);
                var codeImage = ImgVerifyCodeHelper.CreateByteByImgVerifyCode(verfiyCode, 100, 50);
                return File(codeImage, @"image/jpeg");
            }
            catch (Exception e)
            {
                return BadRequest("生成验证 码错误" + e.Message);
            }

        }


        /// <summary>
        /// 获取邮箱验证码
        /// </summary>
        /// <param name="email">邮箱账号</param>
        /// <param name="operaType">操作类型</param>
        /// <returns></returns>
        [HttpGet("GetEmailVerfiyCode")]
        public async Task<ApiResponse> GetMailVerfiyCode(string email, MailMsgOperaType operaType)
        {
            try
            {
                string verfiyCode = mailVerifyCodeHelper.Creat6MailVerfiyCode(email, operaType);
                await MailSendHelper.SendVerfiyCodeMessageMail(email, operaType, verfiyCode);
                return new ApiResponse(true, "发送成功");
            }
            catch (Exception e)
            {
                return new ApiResponse(false, "生成验证码错误" + e.Message);
            }

        }
        //[HttpGet]
        //public ActionResult TestGetMethod()
        //{
        //    if (!requestInfoModel.ImgVerifyCodeIsVerify) return BadRequest("验证码未通过");

        //    return Ok("11111111111111111111");
        //}

        //[HttpGet]
        //public ActionResult TestMailGetMethod()
        //{
        //    if (!requestInfoModel.MailVerifyCodeIsVerify) return BadRequest("邮件验证码未通过");

        //    return Ok("22222222222222222222");
        //}
    }
}
