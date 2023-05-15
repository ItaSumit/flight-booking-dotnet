using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using FlightBooking.ViewModel;
using FlightBooking.Database;
using FlightBooking.Models;
using System.Security.Claims;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        IConfiguration config;
        public LoginController(IConfiguration _config)
        {
            config = _config;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(Login login, [FromServices] UserRepository userRepository)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(login, userRepository);
            if (user != null)
            {
                var tokenString = GenerateJsonWebToken(user);
                if (string.IsNullOrEmpty(tokenString))
                {
                    response = Unauthorized();
                }
                response = Ok(new { token = tokenString });
            }
            return response;
        }


        private async Task<User?> AuthenticateUser(Login login, UserRepository userRepository)
        {
            var user = await userRepository.LoginUser(login);

            return user;
        }

        private string GenerateJsonWebToken(User user)
        {
            var claims = new List<Claim>();
            if(user.Role == Role.Admin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            try
            {
                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
                var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    config["JWT:Issuer"],
                    config["JWT:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
