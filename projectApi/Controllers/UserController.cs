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








        //// GET: api/User
        //[HttpGet]
        //public async Task<ActionResult<List<ShowUserDTO>>> GetUsers()
        //{
        //    var users = await _context.Users
        //        .Include(u => u.Level)
        //        .ToListAsync();

        //    return Ok(_mapper.Map<List<ShowUserDTO>>(users));
        //}

        //// GET: api/User/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ShowUserDTO>> GetUser(int id)
        //{
        //    var user = await _context.Users
        //        .Include(u => u.Level)
        //        .FirstOrDefaultAsync(u => u.UserId == id);

        //    if (user == null)
        //        return NotFound("המשתמש לא נמצא");

        //    return Ok(_mapper.Map<ShowUserDTO>(user));
        //}

        //// POST: api/User/register
        //[HttpPost("register")]
        //public async Task<ActionResult<ShowUserDTO>> Register(
        //    [FromBody] AddUserDTO dto)
        //{
        //    var emailExists = await _context.Users
        //        .AnyAsync(u => u.UserEmail == dto.UserEmail);

        //    if (!_userService.Register(dto))
        //        return Conflict("Email is already registered.");


        //    var user = _mapper.Map<Users>(dto);

        //    user.JoinDate = DateTime.UtcNow;
        //    user.Score = 0;

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    var result = _mapper.Map<ShowUserDTO>(user);

        //    return CreatedAtAction(
        //        nameof(GetUser),
        //        new { id = user.UserId },
        //        result);

        //}

        //// POST: api/User/login
        //[HttpPost("login")]
        //public async Task<ActionResult<AuthResponseDto>> Login(
        //    [FromBody] LoginUserDTO dto)
        //{
        //    var user = await _context.Users
        //        .Include(u => u.Level)
        //        .FirstOrDefaultAsync(u =>
        //            u.UserEmail == dto.UserEmail &&
        //            u.UserPassword == dto.UserPassword);

        //    if (user == null)
        //        return Unauthorized("האימייל או הסיסמה אינם נכונים");

        //    return Ok(_mapper.Map<ShowUserDTO>(user));
        //}

        //// PUT: api/User/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(
        //    int id,
        //    [FromBody] UpdateUserDTO dto)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //        return NotFound("המשתמש לא נמצא");

        //    var emailExists = await _context.Users
        //        .AnyAsync(u =>
        //            u.UserEmail == dto.UserEmail &&
        //            u.UserId != id);

        //    if (emailExists)
        //        return BadRequest("כתובת האימייל כבר משויכת למשתמש אחר");

        //    _mapper.Map(dto, user);

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //// DELETE: api/User/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //        return NotFound("המשתמש לא נמצא");

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}