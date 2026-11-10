using AutoMapper;
using BLL.Services;
using DAL;
using DTO;
using DTO.Lesson;
using DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _iuserService;

        public UserController(IUserService iuserService)
        {
            _iuserService = iuserService;
        }

        [HttpPost("register")]
        public IActionResult Register(AddUserDTO dto)
        {
            _iuserService.Register(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<AuthResponseDto> Login(LoginUserDTO dto)
        {
            var result = _iuserService.Login(dto);
            if (result is null)
                return Unauthorized("Wrong email or password.");

            return Ok(result);
        }
        [HttpGet]
        public ActionResult<List<ShowUserDTO>> GetAll()
        {
            return _iuserService.GetAllUsers();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_iuserService.DeleteUser(id))
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateUserDTO dto)
        {
            var updatedLesson = _iuserService.UpdateUser(id, dto);
            if (updatedLesson == null)
                return NotFound();

            return Ok(updatedLesson);
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