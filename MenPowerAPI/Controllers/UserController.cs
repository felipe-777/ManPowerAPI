using MenPowerAPI.Context;
using MenPowerAPI.Helpers;
using MenPowerAPI.Models;
using MenPowerAPI.Models.Interfaces;
using MenPowerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenPowerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IUserService _userService;
        private readonly ITimekeepingService _timekeepingService;
        private readonly IAuthenticationService _authenticationService;
        public UserController(AppDbContext authContext, IUserService userService, IAuthenticationService authenticationService, ITimekeepingService timekeepingService)
        {
            _authContext = authContext;
            _userService = userService;
            _authenticationService = authenticationService;
            _timekeepingService = timekeepingService;
        }

        [HttpPost ("authenticate")] 
        public async Task<IActionResult> AuthenticateUser([FromBody] User incomingUser)
        {
            try
            {
                if (incomingUser == null)
                    return BadRequest();

                var user = await _authContext.Users
                    .FirstOrDefaultAsync(x => x.UserName == incomingUser.UserName);

                if (user == null)
                    return NotFound(new { Message = "User Not Found" });

                if(!PasswordHasher.VerifyPassword(incomingUser.Password, user.Password))
                {
                    return BadRequest(new { Message = "Password is Incorrect" });
                }

                user.Token = _authenticationService.CreateJWTToken(user);

                return Ok(new
                {
                    Token = user.Token,
                    Message = "Login Success!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" } );
            }     
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] User incomingUser)
        {
            try
            {
                if (incomingUser == null)
                    return BadRequest(new { Message = "Invalid User Data" });

                var validationResultMessage =  await _userService.CreateUserRecordValidate(incomingUser);
                if(!string.IsNullOrEmpty(validationResultMessage))
                    return BadRequest(new { Message = validationResultMessage.ToString() });

                var pass = _userService.CheckPasswordStrength(incomingUser.Password);
                if (!string.IsNullOrEmpty(pass))
                    return BadRequest(new { Message = pass.ToString() });

                incomingUser.Password = PasswordHasher.HashPassword(incomingUser.Password);
                incomingUser.Role = "User";
                incomingUser.Token = "";

                await _authContext.Users.AddAsync(incomingUser);
                await _authContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "User Created Successfully"
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = $"Failed to create user: {ex.Message}" });
            }

        }

        [Authorize]
        [HttpGet("getkbyuser")]
        public async Task<IActionResult> GetTimeKeepingByUser(int userId, DateTime initialDate, DateTime finalDate)
        {
            try
            {
                var response = await _timekeepingService.GetTimeKeepingUser(userId, initialDate, finalDate);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"There was a failure while finding your clock-in/out: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPost("createtk")]
        public async Task<IActionResult> CreateTimeKeeping([FromBody] int userId)
        {
            try
            {
                var response = await _timekeepingService.CreateTimeKeepingRecord(userId);

                if(!response)
                    return BadRequest(new { Message = "You've already put in the maximum number of hours" });

                return Ok(new
                {
                    Message = "Successfully set hours"
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = $"There was a failure while recording your clock-in/out: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _authContext.Users.ToListAsync());
        }
    }
}
