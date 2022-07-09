using AdPang.FileManager.Common.RequestInfoModel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AdPang.FileManager.WebAPI.Filter
{
    public class InitRequestInfoFilter : IAuthorizationFilter
    {
        private readonly RequestInfoModel requestInfoModel;

        public InitRequestInfoFilter(RequestInfoModel requestInfoModel)
        {
            this.requestInfoModel = requestInfoModel;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var temp = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));
            if (temp != null)
                requestInfoModel.CurrentOperaingUser = new Guid(temp.Value);
        }
    }
}
