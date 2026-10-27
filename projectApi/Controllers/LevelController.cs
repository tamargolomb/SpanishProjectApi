using AutoMapper;
using DAL;
using DTO.Levels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace projectApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class LevelController : ControllerBase
        {
            private readonly SpanishDB _context;
            private readonly IMapper _mapper;

            public LevelController(SpanishDB context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            // GET: api/Level
            [HttpGet]
            public async Task<ActionResult<List<ShowLevelDTO>>> GetLevels()
            {
                var levels = await _context.Levels.ToListAsync();

                return Ok(_mapper.Map<List<ShowLevelDTO>>(levels));
            }

            // GET: api/Level/5
            [HttpGet("{id}")]
            public async Task<ActionResult<ShowLevelDTO>> GetLevel(int id)
            {
                var level = await _context.Levels.FindAsync(id);

                if (level == null)
                    return NotFound("הרמה לא נמצאה");

                return Ok(_mapper.Map<ShowLevelDTO>(level));
            }

            // POST: api/Level
            [HttpPost]
            public async Task<ActionResult<ShowLevelDTO>> AddLevel(
                [FromBody] AddLevelDTO dto)
            {
                var level = _mapper.Map<Levels>(dto);

                _context.Levels.Add(level);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<ShowLevelDTO>(level);

                return CreatedAtAction(
                    nameof(GetLevel),
                    new { id = level.LevelId },
                    result);
            }

            // PUT: api/Level/5
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateLevel(
                int id,
                [FromBody] UpdateLevelDTO dto)
            {
                var level = await _context.Levels.FindAsync(id);

                if (level == null)
                    return NotFound("הרמה לא נמצאה");

                _mapper.Map(dto, level);

                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/Level/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteLevel(int id)
            {
                var level = await _context.Levels.FindAsync(id);

                if (level == null)
                    return NotFound("הרמה לא נמצאה");

                _context.Levels.Remove(level);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }
