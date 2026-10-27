using AutoMapper;
using DAL;
using DTO;
using DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SpanishDB _context;
        private readonly IMapper _mapper;

        public UserController(SpanishDB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<List<ShowUserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Level)
                .ToListAsync();

            return Ok(_mapper.Map<List<ShowUserDTO>>(users));
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowUserDTO>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Level)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound("המשתמש לא נמצא");

            return Ok(_mapper.Map<ShowUserDTO>(user));
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<ActionResult<ShowUserDTO>> Register(
            [FromBody] AddUserDTO dto)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.UserEmail == dto.UserEmail);

            if (emailExists)
                return BadRequest("כתובת האימייל כבר קיימת במערכת");

            var user = _mapper.Map<Users>(dto);

            user.JoinDate = DateTime.UtcNow;
            user.Score = 0;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ShowUserDTO>(user);

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.UserId },
                result);
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<ActionResult<ShowUserDTO>> Login(
            [FromBody] LoginUserDTO dto)
        {
            var user = await _context.Users
                .Include(u => u.Level)
                .FirstOrDefaultAsync(u =>
                    u.UserEmail == dto.UserEmail &&
                    u.UserPassword == dto.UserPassword);

            if (user == null)
                return Unauthorized("האימייל או הסיסמה אינם נכונים");

            return Ok(_mapper.Map<ShowUserDTO>(user));
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UpdateUserDTO dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("המשתמש לא נמצא");

            var emailExists = await _context.Users
                .AnyAsync(u =>
                    u.UserEmail == dto.UserEmail &&
                    u.UserId != id);

            if (emailExists)
                return BadRequest("כתובת האימייל כבר משויכת למשתמש אחר");

            _mapper.Map(dto, user);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("המשתמש לא נמצא");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}