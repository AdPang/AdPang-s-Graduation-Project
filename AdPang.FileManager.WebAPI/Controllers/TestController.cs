using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.EntityFrameworkCore.IdentityDb;
using AdPang.FileManager.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdPang.FileManager.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly FileManagerDbContext fileManagerDbContext;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public TestController(FileManagerDbContext fileManagerDbContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.fileManagerDbContext = fileManagerDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(string username,string password)
        {
            if (await roleManager.RoleExistsAsync("Ordinary") == false)
            {
                Role role = new() { Name = "Ordinary" };
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                    return BadRequest("CreatRoleError");
            }
            User user = await userManager.FindByNameAsync(username);
            if (user is null)
            {
                user = new User { UserName = username };

                var result = password is null ? await userManager.CreateAsync(user) : await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    return BadRequest("CreatAccountError");
            }
            else
            {
                return BadRequest("AccountExists");
            }
            if (!await userManager.IsInRoleAsync(user, "Ordinary"))
            {
                var result = await userManager.AddToRoleAsync(user, "Ordinary");
                if (!result.Succeeded)
                {
                    return BadRequest("AddRoleError");
                }
            }
            return Ok("AccountRegisterSuccess");
        }

        /*public async Task<ActionResult> Login(string username,string password)
        {

        }*/
    }
}
