using AdPang.FileManager.Common.Extensions;
using AdPang.FileManager.Common.RequestInfoModel;
using AdPang.FileManager.IServices.Common;
using AdPang.FileManager.Models.FileManagerEntities.Common;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AdPang.FileManager.WebAPI.Controllers.SystemManager
{
    /// <summary>
    /// 菜单资源控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;
        private readonly IMapper mapper;
        private readonly RoleManager<Role> roleManager;
        private readonly RequestInfoModel requestInfoModel;
        private readonly UserManager<User> userManager;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="menuService"></param>
        /// <param name="mapper"></param>
        /// <param name="roleManager"></param>
        /// <param name="requestInfoModel"></param>
        /// <param name="userManager"></param>
        public MenuController(IMenuService menuService, IMapper mapper, RoleManager<Role> roleManager, RequestInfoModel requestInfoModel,UserManager<User> userManager)
        {
            this.menuService = menuService;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.requestInfoModel = requestInfoModel;
            this.userManager = userManager;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("Gets/Admin")]
        [Authorize(Roles ="Admin")]
        public async Task<ApiResponse<IList<MenusDto>>> GetAllMenus()
        {
            var menus = await menuService.GetListAsync();
            var menusDto = mapper.Map<List<MenusDto>>(menus).OrderBy(x=>x.MenuLevel).ToList();
            foreach (var item in menusDto)
            {
                if (menusDto.Any(x => x.ParentMenuId.Equals(item.Id)))
                {
                    item.HasChildrenMenu = true;
                }
            }
            return new ApiResponse<IList<MenusDto>>(true, menusDto);
        }
        /// <summary>
        /// 获取指定菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet("Get/{menuId}/Admin")]
        [Authorize(Roles ="Admin")]
        public async Task<ApiResponse<MenuDto>> GetMenuByIdAsync(Guid menuId)
        {
            var menu = await menuService.FindAsync(x => x.Id.Equals(menuId));
            if (menu == null) return new ApiResponse<MenuDto>(false, "未找到该菜单");
            return new ApiResponse<MenuDto>(true,mapper.Map<MenuDto>(menu));
        }

        /// <summary>
        /// 获取自己的菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        [Authorize(Roles = "Ordinary")]
        public async Task<ApiResponse<IList<MenuDto>>> GetMyMenusAsync()
        {
            var userId = requestInfoModel.CurrentOperaingUser;
            if (userId == null) return new ApiResponse<IList<MenuDto>>(false, "发生错误");
            var user = userManager.Users.Where(x => x.Id.Equals(userId)).First();
            var roleStrs = await userManager.GetRolesAsync(user);
            //所有的角色
            var roles = roleManager.Roles.Where(x => roleStrs.Contains(x.Name)).Include(x => x.Menus).AsNoTracking().ToList();
            //所有的菜单
            var allMenus = await menuService.GetListAsync();
            //所有角色对应的3级菜单集合
            var all3LevelMenu = new List<Menu>();
            foreach (var role in roles)
            {
                all3LevelMenu.AddRange(role.Menus);

            }
            var tempMenus = new List<Menu>();//所有角色对应的菜单集合
            foreach (var menu in all3LevelMenu)
            {
                menu.FindParentMenu(allMenus, tempMenus);
            }
            var rootMenus = tempMenus.Where(x => x.ParentMenuId == null).ToList();//需要返回的根菜单集合
            foreach (var rootMenu in rootMenus)
            {
                tempMenus.Merge(rootMenu);
            }
            return new ApiResponse<IList<MenuDto>>(true, mapper.Map<List<MenuDto>>(rootMenus));
        }
        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        [HttpPut("Edit/{roleId}/Admin")]
        public async Task<ApiResponse> EditMenusForRoleAsync(Guid roleId,List<Guid> menuIds)
        {
            //找到所有的三级菜单
            var menus = await menuService.GetListAsync(x => menuIds.Contains(x.Id) && x.MenuLevel == 2,default,true);
            //找到角色
            var role = roleManager.Roles.Include(x => x.Menus).Where(x => x.Id.Equals(roleId)).FirstOrDefault();
            
            if (role == null || menus == null || menus.Count <= 0) return new ApiResponse(false, "角色或菜单不存在");
            //求交集
            var intersect = role.Menus.Intersect(menus).ToList();
            //需要删除的菜单的差集
            var subList = role.Menus.Except(intersect).ToList();
            //需要添加菜单的差集
            var addList = menus.Except(role.Menus).ToList();
            foreach (var item in subList)
            {
                role.Menus.Remove(item);
            }
            foreach (var item in addList)
            {
                role.Menus.Add(item);
            }
            await roleManager.UpdateAsync(role);

            return new ApiResponse(true, "完成") { Result = mapper.Map<RoleDto>(role) };
        }

        /// <summary>
        /// 获取所有权限树
        /// </summary>
        /// <returns></returns>
        [HttpGet("Tree/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<IList<MenuDto>>> GetAllMenusToTreeAsync()
        {
            var allMenus = await menuService.GetListAsync();
            var rootMenus = allMenus.Where(x => x.ParentMenuId == null).ToList();
            foreach (var rootMenu in rootMenus)
            {
                allMenus.Merge(rootMenu);
            }
            return new ApiResponse<IList<MenuDto>>(true, mapper.Map<IList<MenuDto>>(rootMenus));
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="menuDto"></param>
        /// <returns></returns>
        [HttpPost("Add/admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<MenuDto>> AddMenuAsync(MenuDto menuDto)
        {
            var menu = mapper.Map<Menu>(menuDto);
            menu.MenuLevel = 0;
            if (menu.ParentMenuId != null)
            {
                var parentMenu = await menuService.FindAsync(x => x.Id.Equals(menu.ParentMenuId));
                if (parentMenu == null) return new ApiResponse<MenuDto>(false, "父菜单不存在！");
                menu.MenuLevel = parentMenu.MenuLevel + 1;
            }
            menu.UpdateTime = DateTime.Now;
            await menuService.InsertAsync(menu, true);
            return new ApiResponse<MenuDto>(true, mapper.Map<MenuDto>(menu));
        }

        /// <summary>
        /// 修改菜单名
        /// </summary>
        /// <param name="menuDto"></param>
        /// <returns></returns>
        [HttpPut("Edit/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<MenuDto>> EditMenuAsync(MenuDto menuDto)
        {
            var menu = await menuService.FindAsync(x => x.Id.Equals(menuDto.Id));
            if (menu == null) return new ApiResponse<MenuDto>(false, "未找到该菜单");
            menu.MenuName = menuDto.MenuName;
            menu.IconStr = menuDto.IconStr;
            menu.Uri = menuDto.Uri;
            menu.UpdateTime = DateTime.Now;
            await menuService.UpdateAsync(menu, true);
            return new ApiResponse<MenuDto>(true, mapper.Map<MenuDto>(menu));

        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{menuId}/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse> DeleteMenuAsync(Guid menuId)
        {
            var menu = await menuService.FindMenuDetailAsync(x => x.Id.Equals(menuId));
            if (menu == null) return new ApiResponse(false, "未找到该菜单");
            var isParentMenu = await menuService.FindMenuDetailAsync(x => x.ParentMenuId.Equals(menu.Id));
            if (isParentMenu != null) return new ApiResponse(false, "该菜单含有子菜单，无法删除");
            await menuService.DeleteAsync(menu, true);
            return new ApiResponse(true, "删除成功");
        }

        /// <summary>
        /// 给角色添加菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPost("Add/{roleId}/{menuId}/Admin")]
        public async Task<ApiResponse> AddMenuToRoleAsync(Guid roleId,Guid menuId)
        {
            var role = roleManager.Roles.Where(x => x.Id.Equals(roleId)).FirstOrDefault();
            if (role == null) return new ApiResponse(false, "角色不存在");
            var menu = await menuService.FindAsync(x => x.Id.Equals(menuId));
            if (menu == null) return new ApiResponse(false, "菜单不存在");
            else if (menu.MenuLevel != 2) return new ApiResponse(false, "请添加三级权限！");
            role.Menus.Add(menu);
            await roleManager.UpdateAsync(role);
            return new ApiResponse(true, "添加完成");
        }

        /// <summary>
        /// 获取所有的父节点菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        private async Task GetAllParentMenu(Menu menu, List<Menu> menus)
        {
            menus.Add(menu);
            if (menu.ParentMenuId == null) return;
            var parentMenu = await menuService.FindMenuDetailAsync(x => x.Id.Equals(menu.ParentMenuId));
            if (parentMenu == null) return;
            await GetAllParentMenu(parentMenu, menus);
        }
    }
}
    