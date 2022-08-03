using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AdPang.FileManager.Shared.Paremeters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers.SystemManager
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleManageController : ControllerBase
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly RequestInfoModel requestInfoModel;
        private readonly IMapper mapper;

        public RoleManageController(RoleManager<Role> roleManager, RequestInfoModel requestInfoModel, IMapper mapper, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.requestInfoModel = requestInfoModel;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="queryParameter">分页信息</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("Gets/admin")]
        public ApiResponse<PagedList<RoleDto>> GetRoles([FromQuery] QueryParameter queryParameter)
        {
            //获取符合条件的角色列表
            var roles = roleManager.Roles.Skip(queryParameter.PageIndex * queryParameter.PageSize).Take(queryParameter.PageSize).Where(x => queryParameter.Search != null ? x.Name.Contains(queryParameter.Search) || x.NormalizedName.Contains(queryParameter.Search) : true).ToList();
            //转换成RoleDto
            var roleDtos = mapper.Map<List<RoleDto>>(roles);
            //放入PagedList
            var result = new PagedList<RoleDto>(roleDtos, queryParameter.PageIndex, queryParameter.PageSize, 0);
            //将PagedList放入ApiResponse返回
            return new ApiResponse<PagedList<RoleDto>>(true, result);
        }

        /// <summary>
        /// 根据userId获取对应的角色列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpGet("GetRoles/{userId}/admin")]
        public async Task<ApiResponse<IList<string>>> GetRolesByUserId(Guid userId)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<IList<string>>(false, "用户不存在");
            var roles = await userManager.GetRolesAsync(user);
            return new ApiResponse<IList<string>>(true, roles);
        }

        /// <summary>
        /// 获取当前请求人的角色
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetRoles")]
        public async Task<ApiResponse<IList<string>>> GetRolesASync()
        {
            var user = userManager.Users.Where(x => x.Id.Equals(requestInfoModel.CurrentOperaingUser)).FirstOrDefault();
            if (user == null) return new ApiResponse<IList<string>>(false, "用户不存在");
            var role = await  userManager.GetRolesAsync(user);
            return new ApiResponse<IList<string>>(true, role);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleDto">角色信息</param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPost("add/admin")]
        public async Task<ApiResponse<string>> AddRoleAsync(RoleDto roleDto)
        {
            var role = mapper.Map<Role>(roleDto);
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new ApiResponse<string>(true,result: "添加成功");
            }
            else
            {
                var errorFist = result.Errors.FirstOrDefault();
                var msg = errorFist == null ? "添加失败" : errorFist.Description;
                return new ApiResponse<string>(false, msg);
            }
        }


        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="roleDto">角色信息</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("Edit/admin")]
        public async Task<ApiResponse> EditRoleAsync(RoleDto roleDto)
        {
            //var role = mapper.Map<Role>(roleDto);
            var role = roleManager.Roles.Where(x => x.Id.Equals(roleDto.Id)).FirstOrDefault();
            if (role == null) return new ApiResponse(false, "修改失败") { Message = "角色不存在" };
            role.Name = roleDto.Name;
            
            var result = await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return new ApiResponse(true, "修改成功");
            }
            else
            {
                var errorFist = result.Errors.FirstOrDefault();
                var msg = errorFist == null ? "修改失败" : errorFist.Description;
                return new ApiResponse(false, msg ?? "修改失败");
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{roleId}/admin")]
        public async Task<ApiResponse> DeleteRoleAsync(Guid roleId)
        {
            var role = roleManager.Roles.FirstOrDefault(x => x.Id == roleId);
            if (role == null) return new ApiResponse(false, "删除失败") { Result = "角色不存在" };
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return new ApiResponse(true, "删除成功");
            }
            else
            {
                var errorFist = result.Errors.FirstOrDefault();
                var msg = errorFist == null ? "删除失败" : errorFist.Description;
                return new ApiResponse(false, msg ?? "删除失败");
            }
        }
    }
}
