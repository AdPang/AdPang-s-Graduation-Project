using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AdPang.FileManager.Shared.Paremeters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers.SystemManager
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserManageController : ControllerBase
    {
        private readonly RequestInfoModel requestInfoModel;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IMapper mapper;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="requestInfoModel"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="mapper"></param>
        public UserManageController(RequestInfoModel requestInfoModel, UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            this.requestInfoModel = requestInfoModel;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        /// <summary>
        /// 获取当前用户Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Ordinary")]
        public string GetUserId()
        {
            return requestInfoModel.CurrentOperaingUser.ToString() ?? "发生错误！";
        }

        /// <summary>
        /// 获得用户列表
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpGet("GetAll/admin")]
        public ApiResponse<PagedList<UserDto>> GetUsers([FromQuery] QueryParameter queryParameter)
        {
            var users = userManager.Users.Where(x => queryParameter.Search == null ? true : queryParameter.Search.Contains(x.UserName)||x.UserName.Contains(queryParameter.Search)).Take(queryParameter.PageSize).Skip(queryParameter.PageSize * queryParameter.PageIndex).ToList();
            var userDtos = mapper.Map<List<UserDto>>(users);
            return new ApiResponse<PagedList<UserDto>>(true, new PagedList<UserDto>(userDtos, queryParameter.PageIndex, queryParameter.PageSize, default));
        }

        /// <summary>
        /// 根据角色名获取用户列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpGet("Get/{roleName}/admin")]
        public async Task<ApiResponse<List<UserDto>>> GetUsersByRoleAsync(string roleName)
        {
            //var role = roleManager.Roles.Where(x => x.Id.ToString().Equals(roleStr) || x.Name.Contains(roleStr) || x.NormalizedName.Contains(roleStr)).FirstOrDefault();
            var users = await userManager.GetUsersInRoleAsync(roleName);
            if (users == null) return new ApiResponse<List<UserDto>>(false, "角色名错误");
            var userDtos = mapper.Map<List<UserDto>>(users);
            return new ApiResponse<List<UserDto>>(true, userDtos);
            
        }

        /// <summary>
        /// 给用户添加角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPut("Add/{userId}/{roleName}/admin")]
        public async Task<ApiResponse<string>> AddUserToRoleAsync(Guid userId,string roleName)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<string>(false, "用户名为空");
            var result = await userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return new ApiResponse<string>(true, "添加成功");
            }
            else
            {

                var errorFist = result.Errors.FirstOrDefault();
                var msg = errorFist == null ? "添加失败" : errorFist.Description;
                return new ApiResponse<string>(false, msg ?? "添加失败");
            }
        }

        /// <summary>
        /// 给用户添加角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleNames">角色名列表</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("Adds/{userId}/admin")]
        public async Task<ApiResponse<IList<string>>> AddUsersToRoleAsync(Guid userId, IList<string> roleNames)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<IList<string>>(false, "用户名为空");
            var result = await userManager.AddToRolesAsync(user, roleNames);
            if (result.Succeeded)
            {
                return new ApiResponse<IList<string>>(true, msg:"添加成功");
            }
            else
            {
                var errorDescList = result.Errors.Select(x => x.Description).ToList();
                return new ApiResponse<IList<string>>(false, errorDescList);
            }
        }

        /// <summary>
        /// 删除用户的角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("Remove/{userId}/{roleName}/admin")]
        public async Task<ApiResponse<string>> RemoveRoleFormUserAsync(Guid userId,string roleName)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<string>(false, "用户名为空");
            var result = await userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return new ApiResponse<string>(true, "移除成功");
            }
            else
            {
                var errorFist = result.Errors.FirstOrDefault();
                var msg = errorFist == null ? "移除失败" : errorFist.Description;
                return new ApiResponse<string>(false, msg ?? "移除失败");
            }
        }

        /// <summary>
        /// 删除用户的角色（多个角色）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleNames">角色名列表</param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPut("Rmoves/{userId}/admin")]
        public async Task<ApiResponse<IList<string>>> RemoveRolesFormUserAsync(Guid userId, IList<string> roleNames)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<IList<string>>(false, "用户名为空");
            var result = await userManager.RemoveFromRolesAsync(user, roleNames);
            if (result.Succeeded)
            {
                return new ApiResponse<IList<string>>(true, msg:"移除成功");
            }
            else
            {
                var errorDescList = result.Errors.Select(x=>x.Description).ToList();
                return new ApiResponse<IList<string>>(false, errorDescList);
            }
        }

    }
}
