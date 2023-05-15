using FlightBooking.Database;
using FlightBooking.Models;
using FlightBooking.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAsync([FromServices] UserRepository userRepository)
        {
            var users = await userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Register register, [FromServices] UserRepository userRepository)
        {
            if (ModelState.IsValid)
            {
                var existing = await userRepository.GetUser(u => u.EmailId == register.EmailId);

                if(existing == null)
                {
                    await userRepository.AddUser(register);
                    return Ok("User saved successfully");
                }
                else
                {
                    return Conflict();
                }
                
            }
           return BadRequest(ModelState);
        }
    }
}
