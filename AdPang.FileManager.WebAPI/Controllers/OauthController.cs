using AdPang.FileManager.Common.Helper;
using AdPang.FileManager.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdPang.FileManager.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OauthController : ControllerBase
    {
        private readonly UserManager<User> userManager; 
        private readonly RoleManager<Role> roleManager;
        public OauthController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult> Auth(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest("用户名密码为空！");
            var user =  await userManager.FindByNameAsync(username);
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
    }
}
