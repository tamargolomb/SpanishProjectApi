using BLL;
using BLL.Services;
using DAL;
using DTO;
using DTO.General;
using DTO.Lesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectApi.Extensions;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _ilessonservice;
        private readonly IFileService _fileService;
        private readonly IImageService _imageService;

        public LessonController(ILessonService ilessonservice,IFileService fileService, IImageService imageService)
        {
            _ilessonservice = ilessonservice;
            _fileService = fileService;
            _imageService = imageService;
        }

        [Authorize()]
        [HttpGet]
        public ActionResult<List<ShowLessonDTO>> GetAll()
        {
            return _ilessonservice.GetLessons();
        }

        [HttpGet("lessons")]
        public IActionResult GetLessons([FromQuery] int? levelId,[FromQuery] int? categoryId)
        {
            var lessons = _ilessonservice.GetLessons(levelId, categoryId);

            return Ok(lessons);
        }


        [HttpGet("{id:int}")]
        public ActionResult<ShowLessonDTO> GetById(int id)
        {
            return Ok(_ilessonservice.GetLessonById(id));
        }

        [HttpGet("mine")]
        public ActionResult<List<ShowLessonDTO>> GetMine()
        {
            return Ok(_ilessonservice.GetMine(User.GetUserId()));
        }

        [HttpPost]
        public IActionResult Add([FromForm] AddLessonDTO dto)
        {        
                var result = _ilessonservice.AddLesson(dto, User.GetUserId());
                return CreatedAtAction(nameof(GetById), new { id = result.LessonId }, result); 
        }

        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            var stream = _fileService.GetFileStream(fileName);
            var contentType = _fileService.GetContentType(fileName);

            return File(
                stream,
                contentType,
                enableRangeProcessing: true
            );
        }

        [HttpGet("images/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            try
            {
                var stream = _imageService.GetImageStream(fileName);

                var extension = Path.GetExtension(fileName).ToLowerInvariant();

                var contentType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".webp" => "image/webp",
                    _ => "application/octet-stream"
                };

                return File(stream, contentType);
            }
            catch (FileNotFoundException)
            {
                return NotFound("Image not found");
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid image name");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_ilessonservice.DeleteLesson(id, User.GetUserId()))
                return NotFound();

            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id)
        {
            if (!_ilessonservice.UpdateLesson(id))
                return NotFound();

            return NoContent();
        }
    }
}