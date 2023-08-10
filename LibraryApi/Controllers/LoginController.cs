using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        public LoginController(SignInManager<IdentityUser> signInManager, IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _config = config;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserModel loginModel)
        {
            IActionResult response = Unauthorized();
            var success = await AuthenticateUser(loginModel);

            if (success)
            {
                var token = await GenerateJsonWebToken(loginModel);
                response = Ok(token);
            }
            return response;
        }

        private async Task<bool> AuthenticateUser(UserModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: true, lockoutOnFailure: false);
            return result.Succeeded;
        }

        private async Task<string> GenerateJsonWebToken(UserModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            var userRoles =  await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var seciurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authentication:SecretKey"]));
            var credentials = new SigningCredentials(seciurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Authentication:Issuer"], _config["Authentication:Audience"],claims: authClaims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
