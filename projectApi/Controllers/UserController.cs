using AutoMapper;
using BLL.Services;
using DAL;
using DTO;
using DTO.Lesson;
using DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService iuserService)
        {
            _userService = iuserService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddUserDTO dto)
        {
            try
            {
                // קריאה לפונקציית ההרשמה האסינכרונית
                await _userService.Register(dto);

                // אם הגענו לכאן, סימן שהכל הצליח והמייל נשלח בהצלחה
                return Ok(new
                {
                    message = "Registration successful. Please check your email for the verification code."
                });
            }
            catch (InvalidOperationException ex)
            {
                // תפיסת שגיאות צפויות (כמו אימייל שכבר קיים במערכת)
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // תפיסת שגיאות לא צפויות (כמו כשל בשליחת המייל)
                return StatusCode(500, new
                {
                    message = "An error occurred during registration. Please try again later.",
                    details = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("verify-code")]
        public ActionResult<AuthResponseDto> VerifyCode([FromBody] VerifyCodeDTO dto)
        {
            try
            {
                var authResponse = _userService.VerifyCodeAndLogin(dto);

                if (authResponse == null)
                {
                    return BadRequest(new
                    {
                        message = "Invalid or expired verification code."
                    });
                }

                // מחזיר את הטוקן והפרטים לריאקט בהצלחה!
                return Ok(authResponse);
            }
            catch (InvalidOperationException ex)
            {
                // תפיסת שגיאות לוגיות (כמו קוד שגוי או פג תוקף)
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // שגיאות מערכת לא צפויות
                return StatusCode(500, new
                {
                    message = "An error occurred while verifying the code.",
                    details = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthResponseDto> Login([FromBody] LoginUserDTO dto)
        {
            var result = _userService.Login(dto);
            if (result is null)
                return Unauthorized("Wrong email or password.");

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<List<ShowUserDTO>> GetAll()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_userService.DeleteUser(id))
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateUserDTO dto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (id != currentUserId)
                return Forbid();

            try
            {
                var updatedUser = _userService.UpdateUser(id, dto);

                if (updatedUser == null)
                    return NotFound();

                return Ok(updatedUser);
            }
            catch (InvalidOperationException)
            {
                return Conflict(new
                {
                    message = "An account with this email already exists."
                });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<ShowUserDTO> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var user = _userService.GetByUserId(userId);

            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ShowUserDTO> GetById(int id)
        {
            var user = _userService.GetByUserId(id);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            await _userService.ForgotPassword(dto);

            return Ok(new { message = "If the email exists in the system, a recovery code will be sent to it.." });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var result = _userService.ResetPassword(dto);

            if (!result)
                return BadRequest(new { message = "Password reset failed!" });

            return Ok(new { message = "The password has been reset successfully!" });
        }
    }
}





        