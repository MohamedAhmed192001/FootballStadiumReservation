using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectApi.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SetupController> _logger;

        public SetupController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<SetupController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string name)
        {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist)
            {
               var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
                if(roleResult.Succeeded)
                {
                    _logger.LogInformation($"The role {name} has been added successfully");
                    return Ok(new { result = $"The role {name} has been added successfully" });
                }
                else
                {
                    _logger.LogInformation($"The role {name} has not been added!");
                    return BadRequest(new { error = $"The role {name} has not been added!" });
                }
            }
            else
            {
                return BadRequest(new { error = $"Role {name} already exists!" });
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("AddUserTospecificRole")]
        public async Task<IActionResult> AddUserTospecificRole(string email, string roleName)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest(new { error = "The Role and Email can not be Null Or Empty" });
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The User with email : {email} is not found!");
                return BadRequest(new { error =  $"The User with email : {email} is not found!" });
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if(!roleExist)
            {
                _logger.LogInformation($"The role name : {roleName} is not found!");
                return BadRequest(new { error = $"The role name : {roleName} is not found!" });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if(result.Succeeded)
            {
                _logger.LogInformation($"The user with email : {email} has added to role : {roleName} successfully");
                return Ok(new { result = $"The user with email : {email} has added to role : {roleName} successfully" });
            }
            else
            {
                _logger.LogInformation($"The user with email : {email} was not able to added to the role : {roleName}");
                return BadRequest(new { error = $"The user with email : {email} was not able to added to the role : {roleName}"});
            }
        }

        [HttpGet("GetUserRolesWithEmail")]
        public async Task<IActionResult> GetUserRolesWithEmail(string email)
        {
            if (string.IsNullOrEmpty(email) )
            {
                return BadRequest(new { error = "The Email can not be Null Or Empty" });
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The User with email : {email} is not found!");
                return BadRequest(new { error = $"The User with email : {email} is not found!" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);   
        }

        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest(new { error = "The Role and Email can not be Null Or Empty" });
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The User with email : {email} is not found!");
                return BadRequest(new { error = $"The User with email : {email} is not found!" });
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation($"The role name : {roleName} is not found!");
                return BadRequest(new { error = $"The role name : {roleName} is not found!" });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                _logger.LogInformation($"The user with email : {email} has removed from role : {roleName} successfully");
                return Ok(new { result = $"The user with email : {email} has removed from role : {roleName} successfully" });
            }
            else
            {
                _logger.LogInformation($"The user with email : {email} was not able to removed from the role : {roleName}");
                return BadRequest(new { error = $"The user with email : {email} was not able to removed from the role : {roleName}" });
            }
        }
    }
}
