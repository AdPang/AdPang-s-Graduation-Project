using AdPang.FileManager.Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using AdPang.FileManager.Shared;

namespace AdPang.FileManager.WebAPI.Controllers.SystemManager
{
    /// <summary>
    /// 验证控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OauthController : ControllerBase
    {
        private readonly ILogger<OauthController> _logger;
        private readonly IMapper _mapper;

        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public OauthController(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<OauthController> logger, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 用户登录 （用户名密码登录）
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet("Login")]
        public async Task<ApiResponse<AuthDto>> Auth(string username,string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return new ApiResponse<AuthDto>(false, "用户名密码为空！");
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return new ApiResponse<AuthDto>(false, "用户名不存在！");
            var checkUserResult = await userManager.CheckPasswordAsync(user, password);
            if (!checkUserResult) return new ApiResponse<AuthDto>(false, "用户名或密码错误！");

            if (await userManager.IsLockedOutAsync(user)) return new ApiResponse<AuthDto>(false, $"用户已被锁定！解锁时间为：{user.LockoutEnd}");

            var claims = new List<Claim>(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            });
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return new ApiResponse<AuthDto>(true, new AuthDto { IsSuccess = true, JwtStr = JwtHelper.IssueJwt(claims), UserName = username });
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<ApiResponse> Register(UserDto userDto)
        {
            var mapperUser = _mapper.Map<User>(userDto);
            if (await roleManager.RoleExistsAsync("Ordinary") == false)
            {
                Role role = new() { Name = "Ordinary" };
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                    return new ApiResponse(false,"创建角色'Ordinary'发生错误！");
            }
            User user = await userManager.FindByNameAsync(mapperUser.UserName);
            if (user is null)
            {
                user = mapperUser;

                var result = userDto.Password is null ? await userManager.CreateAsync(user) : await userManager.CreateAsync(user, userDto.Password);
                if (!result.Succeeded)
                    return new ApiResponse(false, "注册出错！");
            }
            else
            {
                return new ApiResponse(false, "账号存在！");
            }
            if (!await userManager.IsInRoleAsync(user, "Ordinary"))
            {
                var result = await userManager.AddToRoleAsync(user, "Ordinary");
                if (!result.Succeeded)
                {
                    return new ApiResponse(false, "添加角色出错！");
                }
            }
            return new ApiResponse(true, "注册成功！");
        }
    }
}
