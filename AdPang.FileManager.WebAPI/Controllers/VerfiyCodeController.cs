using AdPang.FileManager.Common.Helper.Mail;
using AdPang.FileManager.Common.Helper.VerifyCode;
using AdPang.FileManager.Common.RequestInfoModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VerfiyCodeController : ControllerBase
    {
        private readonly ImgVerifyCodeHelper imgVerifyCodeHelper;
        private readonly RequestInfoModel requestInfoModel;
        private readonly MailVerifyCodeHelper mailVerifyCodeHelper;

        public VerfiyCodeController(ImgVerifyCodeHelper imgCodeHelper, RequestInfoModel requestInfoModel, MailVerifyCodeHelper mailVerifyCodeHelper)
        {
            this.imgVerifyCodeHelper = imgCodeHelper;
            this.requestInfoModel = requestInfoModel;
            this.mailVerifyCodeHelper = mailVerifyCodeHelper;
        }

        [HttpGet]
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
                return BadRequest("生成验证码错误"+e.Message);
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetMailVerfiyCode(string email, MailMsgOperaType operaType)
        {
            try
            {
                string verfiyCode = mailVerifyCodeHelper.Creat6MailVerfiyCode(email, operaType);
                await MailSendHelper.SendVerfiyCodeMessageMail(email, operaType, verfiyCode);
                return Ok("发送成功");
            }
            catch (Exception e)
            {
                return BadRequest("生成验证码错误" + e.Message);
            }

        }



        [HttpGet]
        public ActionResult TestGetMethod()
        {
            if (!requestInfoModel.ImgVerifyCodeIsVerify) return BadRequest("验证码未通过");

            return Ok("11111111111111111111");
        }

        [HttpGet]
        public ActionResult TestMailGetMethod()
        {
            if (!requestInfoModel.MailVerifyCodeIsVerify) return BadRequest("邮件验证码未通过");

            return Ok("22222222222222222222");
        }
    }
}
