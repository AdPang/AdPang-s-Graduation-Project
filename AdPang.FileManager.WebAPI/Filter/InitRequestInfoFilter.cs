using AdPang.FileManager.Common.Helper;
using AdPang.FileManager.Common.RequestInfoModel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AdPang.FileManager.WebAPI.Filter
{
    public class InitRequestInfoFilter : IAuthorizationFilter
    {
        private readonly RequestInfoModel requestInfoModel;
        private readonly VerifyCodeHelper verfiyCodeHelper;

        public InitRequestInfoFilter(RequestInfoModel requestInfoModel,VerifyCodeHelper verfiyCodeHelper)
        {
            this.requestInfoModel = requestInfoModel;
            this.verfiyCodeHelper = verfiyCodeHelper;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var temp = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));
            if (temp != null)
                requestInfoModel.CurrentOperaingUser = new Guid(temp.Value);
            GetVerifyResult(context.HttpContext.Request);


        }

        private void GetVerifyResult(HttpRequest request)
        {
            bool isVerify = false;
            if (request.Headers.ContainsKey("Seed")&& request.Headers.ContainsKey("VerifyCode"))
            {
                string seedStr = request.Headers["Seed"].ToString();
                if(Guid.TryParse(seedStr,out Guid seed))
                {
                    string verifyCode = request.Headers["VerifyCode"].ToString();
                    isVerify = verfiyCodeHelper.Verify4Capt(seed, verifyCode);
                }

            }


            requestInfoModel.IsVerify = isVerify;
        }
    }
}
