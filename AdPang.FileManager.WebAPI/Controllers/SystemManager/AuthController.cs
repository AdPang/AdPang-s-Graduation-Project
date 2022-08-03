using AdPang.FileManager.Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared.Dtos.SystemCommon;

namespace AdPang.FileManager.WebAPI.Controllers.SystemManager

{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OauthController : ControllerBase
    {
        private readonly ILogger<OauthController> _logger;
        private readonly IMapper _mapper;

        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        public OauthController(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<OauthController> logger, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Auth(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest("用户名密码为空！");
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("用户名不存在！");
            var checkUserResult = await userManager.CheckPasswordAsync(user, password);
            if (!checkUserResult) return BadRequest("用户名或密码错误！");

            if (await userManager.IsLockedOutAsync(user)) return BadRequest($"用户已被锁定！解锁时间为：{user.LockoutEnd}");

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
            return Ok(JwtHelper.IssueJwt(claims));
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserDto userDto)
        {
            var mapperUser = _mapper.Map<User>(userDto);
            if (await roleManager.RoleExistsAsync("Ordinary") == false)
            {
                Role role = new() { Name = "Ordinary" };
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                    return BadRequest("创建角色'Ordinary'发生错误！");
            }
            User user = await userManager.FindByNameAsync(mapperUser.UserName);
            if (user is null)
            {
                user = mapperUser;

                var result = userDto.Password is null ? await userManager.CreateAsync(user) : await userManager.CreateAsync(user, userDto.Password);
                if (!result.Succeeded)
                    return BadRequest("注册出错！");
            }
            else
            {
                return BadRequest("账号存在！");
            }
            if (!await userManager.IsInRoleAsync(user, "Ordinary"))
            {
                var result = await userManager.AddToRoleAsync(user, "Ordinary");
                if (!result.Succeeded)
                {
                    return BadRequest("添加角色出错！");
                }
            }
            return Ok("注册成功！");
        }
    }
}
