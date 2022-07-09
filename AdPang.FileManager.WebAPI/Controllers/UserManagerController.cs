using AdPang.FileManager.Common.RequestInfoModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserManagerController : ControllerBase
    {
        private readonly RequestInfoModel requestInfoModel;

        public UserManagerController(RequestInfoModel requestInfoModel)
        {
            this.requestInfoModel = requestInfoModel;
        }

        [HttpGet]
        public string GetUserId()
        {
            return requestInfoModel.CurrentOperaingUser.ToString()??"没有用户";
        }
    }
}
