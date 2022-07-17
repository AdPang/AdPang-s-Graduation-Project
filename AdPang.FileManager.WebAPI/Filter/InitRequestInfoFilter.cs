using AdPang.FileManager.Common.Helper.Mail;
using AdPang.FileManager.Common.Helper.VerifyCode;
using AdPang.FileManager.Common.RequestInfoModel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AdPang.FileManager.WebAPI.Filter
{
    public class InitRequestInfoFilter : IAuthorizationFilter
    {
        private readonly RequestInfoModel requestInfoModel;
        private readonly ImgVerifyCodeHelper imgVerfiyCodeHelper;
        private readonly MailVerifyCodeHelper mailVerifyCodeHelper;

        public InitRequestInfoFilter(RequestInfoModel requestInfoModel,ImgVerifyCodeHelper imgVerfiyCodeHelper, MailVerifyCodeHelper mailVerifyCodeHelper)
        {
            this.requestInfoModel = requestInfoModel;
            this.imgVerfiyCodeHelper = imgVerfiyCodeHelper;
            this.mailVerifyCodeHelper = mailVerifyCodeHelper;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var temp = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));
            if (temp != null)
                requestInfoModel.CurrentOperaingUser = new Guid(temp.Value);
            CheckImgVerify(context.HttpContext.Request);
            CheckMailVerify(context.HttpContext.Request);

        }

        private void CheckImgVerify(HttpRequest request)
        {
            try
            {
                bool isVerify = false;
                if (request.Headers.ContainsKey("Seed") && request.Headers.ContainsKey("ImgVerifyCode"))
                {
                    string seedStr = request.Headers["Seed"].ToString();
                    if (Guid.TryParse(seedStr, out Guid seed))
                    {
                        string verifyCode = request.Headers["ImgVerifyCode"].ToString();
                        isVerify = imgVerfiyCodeHelper.VerifyImgCode(seed, verifyCode);
                    }

                }
                requestInfoModel.ImgVerifyCodeIsVerify = isVerify;

            }
            catch (Exception)
            {
                requestInfoModel.ImgVerifyCodeIsVerify = false;
            }
        }

        private void CheckMailVerify(HttpRequest request)
        {
            try
            {
                bool isVerify = false;
                if (request.Headers.ContainsKey("Email") && request.Headers.ContainsKey("MailVerifyCode") && request.Headers.ContainsKey("OperaType"))
                {
                    string email = request.Headers["Email"].ToString();
                    string verifyCode = request.Headers["MailVerifyCode"].ToString();
                    string operaTypeStr = request.Headers["OperaType"].ToString();
                    if (int.TryParse(operaTypeStr,out int operaTypeInt))
                    {

                        isVerify = mailVerifyCodeHelper.VerifyImgCode(email, verifyCode, (MailMsgOperaType)operaTypeInt);
                    }

                }
                requestInfoModel.MailVerifyCodeIsVerify = isVerify;

            }
            catch (Exception)
            {
                requestInfoModel.MailVerifyCodeIsVerify = false;
            }
        }
    }
}
