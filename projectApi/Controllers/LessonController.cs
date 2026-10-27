using AutoMapper;
using DAL;
using DTO;
using DTO.General;
using DTO.Lesson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly SpanishDB _context;
        private readonly IMapper _mapper;

        public LessonController(SpanishDB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // טעינת שיעור עם הקטגוריה, הרמה והמשתמש שהעלה אותו
        private IQueryable<Lesson> GetLessonsWithDetails()
        {
            return _context.Lessons
                .Include(l => l.Category)
                .Include(l => l.Level)
                .Include(l => l.user);
        }

        // GET: api/Lesson
        [HttpGet]
        public async Task<ActionResult<List<ShowLessonDTO>>> GetLessons()
        {
            var lessons = await GetLessonsWithDetails().ToListAsync();

            var result = _mapper.Map<List<ShowLessonDTO>>(lessons);

            // חישוב ממוצע הדירוג לכל שיעור
            foreach (var item in result)
            {
                item.Rating = await _context.ReviewsAndRatings
                    .Where(r => r.LessonId == item.LessonId)
                    .Select(r => (double?)r.Rating)
                    .AverageAsync() ?? 0;
            }

            return Ok(result);
        }

        // GET: api/Lesson/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowLessonDTO>> GetLesson(int id)
        {
            var lesson = await GetLessonsWithDetails()
                .FirstOrDefaultAsync(l => l.LessonId == id);

            if (lesson == null)
                return NotFound("השיעור לא נמצא");

            var result = _mapper.Map<ShowLessonDTO>(lesson);

            result.Rating = await _context.ReviewsAndRatings
                .Where(r => r.LessonId == id)
                .Select(r => (double?)r.Rating)
                .AverageAsync() ?? 0;

            return Ok(result);
        }

        // POST: api/Lesson
        [HttpPost]
        public async Task<ActionResult<ShowLessonDTO>> AddLesson(
            [FromBody] AddLessonDTO dto)
        {
            // בדיקה שהקטגוריה קיימת
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryId == dto.CategoryId);

            if (!categoryExists)
                return BadRequest("הקטגוריה שנבחרה לא קיימת");

            // בדיקה שהרמה קיימת
            var levelExists = await _context.Levels
                .AnyAsync(l => l.LevelId == dto.LevelId);

            if (!levelExists)
                return BadRequest("הרמה שנבחרה לא קיימת");

            // בדיקה שהמשתמש קיים
            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == dto.UserId);

            if (!userExists)
                return BadRequest("המשתמש לא קיים");

            var lesson = _mapper.Map<Lesson>(dto);

            lesson.ViewCount = 0;
            lesson.CreatedAt = DateTime.UtcNow;

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            // טעינה מחדש כדי לקבל גם את פרטי הקשרים
            var savedLesson = await GetLessonsWithDetails()
                .FirstAsync(l => l.LessonId == lesson.LessonId);

            var result = _mapper.Map<ShowLessonDTO>(savedLesson);
            result.Rating = 0;

            return CreatedAtAction(
                nameof(GetLesson),
                new { id = lesson.LessonId },
                result);
        }

        // POST: api/Lesson/search
        [HttpPost("search")]
        public async Task<ActionResult<List<ShowLessonDTO>>> SearchLessons(
            [FromBody] LessonSearchDTO dto)
        {
            var query = GetLessonsWithDetails();

            if (!string.IsNullOrWhiteSpace(dto.LessonTitle))
            {
                query = query.Where(l =>
                    l.LessonTitle.Contains(dto.LessonTitle));
            }

            if (dto.CategoryId.HasValue)
            {
                query = query.Where(l =>
                    l.CategoryId == dto.CategoryId.Value);
            }

            if (dto.LevelId.HasValue)
            {
                query = query.Where(l =>
                    l.LevelId == dto.LevelId.Value);
            }

            var lessons = await query.ToListAsync();
            var result = _mapper.Map<List<ShowLessonDTO>>(lessons);

            foreach (var item in result)
            {
                item.Rating = await _context.ReviewsAndRatings
                    .Where(r => r.LessonId == item.LessonId)
                    .Select(r => (double?)r.Rating)
                    .AverageAsync() ?? 0;
            }

            // קודם השיעורים עם הדירוג הגבוה ביותר
            result = result.OrderByDescending(l => l.Rating).ToList();

            return Ok(result);
        }

        // POST: api/Lesson/views
        [HttpPost("views")]
        public async Task<IActionResult> AddView(
            [FromBody] AddViewsDTO dto)
        {
            var lesson = await _context.Lessons
                .FindAsync(dto.LessonId);

            if (lesson == null)
                return NotFound("השיעור לא נמצא");

            lesson.ViewCount++;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "הצפייה נוספה בהצלחה",
                lesson.ViewCount
            });
        }

        // PUT: api/Lesson/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(
            int id,
            [FromBody] AddLessonDTO dto)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
                return NotFound("השיעור לא נמצא");

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryId == dto.CategoryId);

            var levelExists = await _context.Levels
                .AnyAsync(l => l.LevelId == dto.LevelId);

            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == dto.UserId);

            if (!categoryExists || !levelExists || !userExists)
                return BadRequest("הקטגוריה, הרמה או המשתמש אינם קיימים");

            // שומרים את נתוני המערכת שאסור לאפס בעדכון
            var currentViewCount = lesson.ViewCount;
            var currentCreatedAt = lesson.CreatedAt;

            _mapper.Map(dto, lesson);

            lesson.ViewCount = currentViewCount;
            lesson.CreatedAt = currentCreatedAt;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Lesson/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
                return NotFound("השיעור לא נמצא");

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}