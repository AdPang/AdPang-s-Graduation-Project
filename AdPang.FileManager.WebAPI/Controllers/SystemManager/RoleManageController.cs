using AdPang.FileManager.Common.Extensions;
using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.Extensions.Extensions;
using AdPang.FileManager.IServices.Common;
using AdPang.FileManager.Models.FileManagerEntities.Common;
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
    /// 角色控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManageController : ControllerBase
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IMenuService menuService;
        private readonly RequestInfoModel requestInfoModel;
        private readonly IMapper mapper;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="requestInfoModel"></param>
        /// <param name="mapper"></param>
        /// <param name="userManager"></param>
        /// <param name="menuService"></param>
        public RoleManageController(RoleManager<Role> roleManager, RequestInfoModel requestInfoModel, IMapper mapper, UserManager<User> userManager,IMenuService menuService)
        {
            this.roleManager = roleManager;
            this.requestInfoModel = requestInfoModel;
            this.mapper = mapper;
            this.userManager = userManager;
            this.menuService = menuService;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpGet("GetAll/Admin")]
        public async Task<ApiResponse<IList<RoleDto>>> GetRolesAsync()
        {
            var roles = await roleManager.Roles.AsNoTracking().ToListAsync();
            return new ApiResponse<IList<RoleDto>>(true, mapper.Map<IList<RoleDto>>(roles));
        }

        /// <summary>
        /// 获取角色详情列表
        /// </summary>
        /// <param name="queryParameter">分页信息</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("Gets/admin")]
        public async Task<ApiResponse<PagedList<RoleDetailDto>>> GetRolesDetailAsync([FromQuery] QueryParameter queryParameter)
        {
            //获取符合条件的角色列表
            var roles = roleManager.Roles.Include(x=>x.Menus).Skip(queryParameter.PageIndex * queryParameter.PageSize).Take(queryParameter.PageSize).Where(x => queryParameter.Search != null ? x.Name.Contains(queryParameter.Search) || x.NormalizedName.Contains(queryParameter.Search) : true).AsNoTracking().ToList();
            var allMenus = await menuService.GetListAsync();
            foreach (var role in roles)
            {
                var tempRoleAllMenus = new List<Menu>();
                foreach (var item in role.Menus)
                {
                    item.FindParentMenu(ObjectCopier.CloneJson(allMenus), tempRoleAllMenus);
                }
                tempRoleAllMenus = tempRoleAllMenus.DistinctBy(x => x.Id).ToList();
                var rootMenus = tempRoleAllMenus.Where(x => x.ParentMenuId == null).ToList();
                role.Menus.Clear();
                foreach (var rootMenu in rootMenus)
                {
                    rootMenu.ChildrenMenu.Clear();
                    ObjectCopier.CloneJson(tempRoleAllMenus).Merge(rootMenu);
                    role.Menus.Add(rootMenu);
                }
            }
            
            //转换成RoleDto
            var roleDtos = mapper.Map<List<RoleDetailDto>>(roles);
            //放入PagedList
            var result = new PagedList<RoleDetailDto>(roleDtos, queryParameter.PageIndex, queryParameter.PageSize, 0);
            //将PagedList放入ApiResponse返回
            return new ApiResponse<PagedList<RoleDetailDto>>(true, result);
        }

        /// <summary>
        /// 根据userId获取对应的角色列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetRoles/{userId}/admin")]
        public async Task<ApiResponse<IList<string>>> GetRolesByUserIdAsync(Guid userId)
        {
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            if (user == null) return new ApiResponse<IList<string>>(false, "用户不存在");
            var roles = await userManager.GetRolesAsync(user);
            return new ApiResponse<IList<string>>(true, roles);
        }
        /// <summary>
        /// 根据角色id获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpGet("{roleId}/admin")]
        public async Task<ApiResponse<RoleDto>> GetRoleByIdAsync(Guid roleId)
        {
            var role = await roleManager.Roles.Where(x => x.Id.Equals(roleId)).FirstOrDefaultAsync();
            if (role == null) return new ApiResponse<RoleDto>(false, "未找到该角色");
            return new ApiResponse<RoleDto>(true, mapper.Map<RoleDto>(role));
        }

        /// <summary>
        /// 获取当前请求人的角色
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetRoles")]
        public async Task<ApiResponse<IList<string>>> GetMyRolesAsync()
        {
            var user = userManager.Users.Where(x => x.Id.Equals(requestInfoModel.CurrentOperaingUser)).FirstOrDefault();
            if (user == null) return new ApiResponse<IList<string>>(false, "用户不存在");
            var role = await userManager.GetRolesAsync(user);
            return new ApiResponse<IList<string>>(true, role);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleDto">角色信息</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("add/admin")]
        public async Task<ApiResponse<string>> AddRoleAsync(RoleDto roleDto)
        {
            var role = mapper.Map<Role>(roleDto);
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new ApiResponse<string>(true, result: "添加成功");
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
