using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AdPang.FileManager.Shared.Paremeters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll/Admin")]
        public async Task<ApiResponse<PagedList<UserDetailDto>>> GetUsersAsync([FromQuery] QueryParameter queryParameter)
        {
            queryParameter.PageIndex--;
            var users = userManager.Users.Where(x => queryParameter.Search == null ? true : queryParameter.Search.Contains(x.UserName) || x.UserName.Contains(queryParameter.Search)).AsNoTracking().ToList();
            var totalCount = users.Count();
            
            var userDtos = mapper.Map<List<UserDetailDto>>(users);
            for(int i = 0; i < users.Count; i++)
            {
                var roles = await userManager.GetRolesAsync(users[i]);

                userDtos[i].RolesStr = string.Join(',',roles.ToArray());
            }
            return new ApiResponse<PagedList<UserDetailDto>>(true, new PagedList<UserDetailDto>(userDtos, queryParameter.PageIndex, queryParameter.PageSize, default) { TotalCount = totalCount });
        }
        /// <summary>
        /// 根据Id获取角色详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("Get/{userId}/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<UserDetailDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await userManager.Users.Where(x => x.Id.Equals(userId)).AsNoTracking().FirstOrDefaultAsync();
            if (user == null) return new ApiResponse<UserDetailDto>(false, "未找到该用户");
            var roleStrs = await userManager.GetRolesAsync(user);
            var mapperUser = mapper.Map<UserDetailDto>(user);
            var roles = await roleManager.Roles.Where(x => roleStrs.Contains(x.Name)).AsNoTracking().ToListAsync();
            mapperUser.Roles.AddRange(mapper.Map<IList<RoleDto>>(roles));
            mapperUser.RolesStr = string.Join(',', mapperUser.Roles.Select(x=>x.Name));
            return new ApiResponse<UserDetailDto>(true, mapperUser);

        }

        /// <summary>
        /// 根据角色名获取用户列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("Gets/{roleName}/Admin")]
        public async Task<ApiResponse<List<UserDto>>> GetUsersByRoleAsync(string roleName)
        {
            //var role = roleManager.Roles.Where(x => x.Id.ToString().Equals(roleStr) || x.Name.Contains(roleStr) || x.NormalizedName.Contains(roleStr)).FirstOrDefault();
            var users = await userManager.GetUsersInRoleAsync(roleName);
            if (users == null) return new ApiResponse<List<UserDto>>(false, "角色名错误");
            var userDtos = mapper.Map<List<UserDto>>(users);
            return new ApiResponse<List<UserDto>>(true, userDtos);

        }

        /// <summary>
        /// 添加用户（管理员）
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("Add/Admin")]
        public async Task<ApiResponse> AddUser(UserDto userDto)
        {
            User? user = await userManager.Users.Where(x => x.Id.Equals(userDto) || x.UserName.Equals(userDto.UserName)).FirstOrDefaultAsync();
            if (user != null)
            {
                return new ApiResponse(false, "该用户已存在");
            }
            var isExistsOrdinaryRole = await roleManager.RoleExistsAsync("Ordinary");
            if(!isExistsOrdinaryRole)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "Ordinary"
                });
                return new ApiResponse(false, "角色：Ordinary不存在！发生此错误请联系管理员！");
            }
            user = mapper.Map<User>(userDto);
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "Ordinary");
            return new ApiResponse(true, "添加用户成功");
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPut("Edit/{userId}/Admin")]
        public async Task<ApiResponse> EditAsync(Guid userId,UserDto userDto)
        {
            var user = await userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefaultAsync();
            if (user == null) return new ApiResponse(false, "未找到该用户");
            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;
            await userManager.UpdateAsync(user);
            return new ApiResponse(true, "编辑成功");
        }


        /// <summary>
        /// 解封账号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isSuspended"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("EditStatus/{userId}/{isSuspended}/Admin")]
        public async Task<ApiResponse> AccountRestoreAsync(Guid userId,bool isSuspended)
        {
            return await EditAccountStatus(userId, isSuspended);
        }

        private async Task<ApiResponse> EditAccountStatus(Guid userId,bool isSuspended)
        {
            var user = await userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefaultAsync();
            if (user == null) return new ApiResponse(false, "未找到该用户");
            await userManager.SetLockoutEnabledAsync(user, !isSuspended);
            if (isSuspended)
            {
                await userManager.SetLockoutEndDateAsync(user,DateTime.Now.AddYears(100));
                return new ApiResponse(true, "账号停封封成功");
            }
            else
            {
                
                await userManager.SetLockoutEndDateAsync(user, DateTime.Now);
                return new ApiResponse(true, "账号解封成功");
            }
        }


        /// <summary>
        /// 给用户添加角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("Add/{userId}/{roleName}/Admin")]
        public async Task<ApiResponse<string>> AddUserToRoleAsync(Guid userId, string roleName)
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
        /// <param name="roleIds">角色Id列表</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("Adds/{userId}/Admin")]
        public async Task<ApiResponse<IList<string>>> AddUsersToRoleAsync(Guid userId, IList<Guid> roleIds)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<IList<string>>(false, "用户名为空");
            var roles = await roleManager.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();
            var rolesNameList = roles.Select(x => x.Name).ToList();
            var rolesBeforeStrList = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, rolesBeforeStrList);
            

            var result = await userManager.AddToRolesAsync(user, rolesNameList);

            if (result.Succeeded)
            {
                return new ApiResponse<IList<string>>(true, msg: "添加成功");
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
        [HttpPut("Remove/{userId}/{roleName}/Admin")]
        public async Task<ApiResponse<string>> RemoveRoleFormUserAsync(Guid userId, string roleName)
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
        [Authorize(Roles = "Admin")]
        [HttpPut("Rmoves/{userId}/Admin")]
        public async Task<ApiResponse<IList<string>>> RemoveRolesFormUserAsync(Guid userId, IList<string> roleNames)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<IList<string>>(false, "用户名为空");
            var result = await userManager.RemoveFromRolesAsync(user, roleNames);
            if (result.Succeeded)
            {
                return new ApiResponse<IList<string>>(true, msg: "移除成功");
            }
            else
            {
                var errorDescList = result.Errors.Select(x => x.Description).ToList();
                return new ApiResponse<IList<string>>(false, errorDescList);
            }
        }

    }
}
