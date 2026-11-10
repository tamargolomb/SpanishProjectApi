using BLL;
using BLL.Services;
using DAL;
using DTO;
using DTO.General;
using DTO.Lesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWeb;


namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _ilessonservice;

        public LessonController(ILessonService ilessonservice)
        {
            _ilessonservice = ilessonservice;
        }

        [HttpGet]
        public ActionResult<List<ShowLessonDTO>> GetAll()
        {
            return _ilessonservice.GetAllLessons();
        }

        [HttpGet("{id:int}")]
        public ActionResult<ShowLessonDTO> GetById(int id)
        {
            return Ok(_ilessonservice.GetLessonById(id));
        }
        [HttpPost]
        public IActionResult Add([FromForm] AddLessonDTO dto)
        {
            return Ok(_ilessonservice.AddLesson(dto));
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_ilessonservice.DeleteLesson(id, User.GetUserId()))
                return NotFound();

            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ShowLessonDTO dto)
        {
            var updatedLesson = _ilessonservice.UpdateLesson(id, dto);
            if (updatedLesson == null)
                return NotFound();

            return Ok(updatedLesson);
        }
    }
}