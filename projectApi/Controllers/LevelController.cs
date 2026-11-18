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
    }
}
