using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using RouletteAPI.Services;
using System.Threading.Tasks;

namespace RouletteAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<User>> GetUser(string name)
        {
            var user = await _userService.GetUserByNameAsync(name);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> SaveUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Name))
            {
                return BadRequest("Datos de usuario inválidos");
            }

            var result = await _userService.SaveUserAsync(user);
            return Ok(result);
        }
    }
}