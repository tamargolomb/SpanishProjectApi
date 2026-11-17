using AutoMapper;
using BLL.Services;
using DAL;
using DTO.Lesson;
using DTO.Levels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace projectApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize]
    public class LevelController : ControllerBase
        {
        private readonly ILevelService _ilevelservice;

        public LevelController(ILevelService ilevelservice)
        {
            _ilevelservice = ilevelservice;
        }


       

        [HttpGet]
        public ActionResult<List<ShowLevelDTO>> GetAll()
        {
            return Ok(_ilevelservice.GetAllLevels());
        }


        [HttpGet("{id}")]
        public ActionResult<ShowLevelDTO> GetById(int id)
        {
            return Ok(_ilevelservice.GetLevelById(id));
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add([FromBody] AddLevelDTO dto)
        {
            var level = _ilevelservice.AddLevel(dto);
            return CreatedAtAction(nameof(GetById),
                new { id = level.LevelId }, level);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateLevelDTO dto)
        {
            var updatedLevel = _ilevelservice.UpdateLevel(id, dto);

            if (updatedLevel == null)
                return NotFound();

            return Ok(updatedLevel);
        }
        //// GET: api/Level
        //[HttpGet]
        //public async Task<ActionResult<List<ShowLevelDTO>>> GetLevels()
        //{
        //    var levels = await _context.Levels.ToListAsync();

        //    return Ok(_mapper.Map<List<ShowLevelDTO>>(levels));
        //}

        //// GET: api/Level/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ShowLevelDTO>> GetLevel(int id)
        //{
        //    var level = await _context.Levels.FindAsync(id);

        //    if (level == null)
        //        return NotFound("הרמה לא נמצאה");

        //    return Ok(_mapper.Map<ShowLevelDTO>(level));
        //}

        //// POST: api/Level
        //[HttpPost]
        //public async Task<ActionResult<ShowLevelDTO>> AddLevel(
        //    [FromBody] AddLevelDTO dto)
        //{
        //    var level = _mapper.Map<Levels>(dto);

        //    _context.Levels.Add(level);
        //    await _context.SaveChangesAsync();

        //    var result = _mapper.Map<ShowLevelDTO>(level);

        //    return CreatedAtAction(
        //        nameof(GetLevel),
        //        new { id = level.LevelId },
        //        result);
        //}

        //// PUT: api/Level/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateLevel(
        //    int id,
        //    [FromBody] UpdateLevelDTO dto)
        //{
        //    var level = await _context.Levels.FindAsync(id);

        //    if (level == null)
        //        return NotFound("הרמה לא נמצאה");

        //    _mapper.Map(dto, level);

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //// DELETE: api/Level/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteLevel(int id)
        //{
        //    var level = await _context.Levels.FindAsync(id);

        //    if (level == null)
        //        return NotFound("הרמה לא נמצאה");

        //    _context.Levels.Remove(level);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
    }
