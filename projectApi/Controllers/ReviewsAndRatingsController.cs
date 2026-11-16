using AutoMapper;
using BLL.Services;
using DAL;
using DTO;
using DTO.Lesson;
using DTO.ReviewsAndRatings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsAndRatingsController : ControllerBase
    {
        private readonly IReviewAndRatingService _ireviewandratingservice;

        public ReviewsAndRatingsController(IReviewAndRatingService ireviewandratingservice)
        {
        _ireviewandratingservice= ireviewandratingservice;
        }

        [HttpGet]
        public ActionResult<List<ShowReviewsAndRatingsDTO>> GetAll()
        {
            return _ireviewandratingservice.GetAllreviews();
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddResponseDTO dto)
        {
            return Ok(_ireviewandratingservice.AddReview(dto));
        }


       
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_ireviewandratingservice.DeleteReview(id, User.GetUserId()))
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateRatingDTO dto)
        {
            var updatedReview = _ireviewandratingservice.UpdateReview(id, dto);
            if (updatedReview == null)
                return NotFound();

            return Ok(updatedReview);
        }

        //// GET: api/ReviewsAndRatings
        //[HttpGet]
        //public async Task<ActionResult<List<ShowReviewsAndRatingsDTO>>>
        //    GetReviews()
        //{
        //    var reviews = await _context.ReviewsAndRatings
        //        .Include(r => r.User)
        //        .ToListAsync();

        //    return Ok(
        //        _mapper.Map<List<ShowReviewsAndRatingsDTO>>(reviews));
        //}

        //// GET: api/ReviewsAndRatings/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ShowReviewsAndRatingsDTO>>
        //    GetReview(int id)
        //{
        //    var review = await _context.ReviewsAndRatings
        //        .Include(r => r.User)
        //        .FirstOrDefaultAsync(r => r.ReviewId == id);

        //    if (review == null)
        //        return NotFound("התגובה לא נמצאה");

        //    return Ok(_mapper.Map<ShowReviewsAndRatingsDTO>(review));
        //}

        //// GET: api/ReviewsAndRatings/lesson/5
        //[HttpGet("lesson/{lessonId}")]
        //public async Task<ActionResult<List<ShowReviewsAndRatingsDTO>>>
        //    GetReviewsByLesson(int lessonId)
        //{
        //    var lessonExists = await _context.Lessons
        //        .AnyAsync(l => l.LessonId == lessonId);

        //    if (!lessonExists)
        //        return NotFound("השיעור לא נמצא");

        //    var reviews = await _context.ReviewsAndRatings
        //        .Include(r => r.User)
        //        .Where(r => r.LessonId == lessonId)
        //        .ToListAsync();

        //    return Ok(
        //        _mapper.Map<List<ShowReviewsAndRatingsDTO>>(reviews));
        //}

        //// POST: api/ReviewsAndRatings?userId=1
        //[HttpPost]
        //public async Task<ActionResult<ShowReviewsAndRatingsDTO>>
        //    AddReview(
        //        [FromQuery] int userId,
        //        [FromBody] AddResponseDTO dto)
        //{
        //    if (dto.Rating < 1 || dto.Rating > 5)
        //        return BadRequest("הדירוג חייב להיות בין 1 ל־5");

        //    var lessonExists = await _context.Lessons
        //        .AnyAsync(l => l.LessonId == dto.LessonId);

        //    if (!lessonExists)
        //        return BadRequest("השיעור לא נמצא");

        //    var userExists = await _context.Users
        //        .AnyAsync(u => u.UserId == userId);

        //    if (!userExists)
        //        return BadRequest("המשתמש לא נמצא");

        //    var review = _mapper.Map<ReviewsAndRatings>(dto);

        //    review.UserId = userId;
        //    review.CreatedAt = DateTime.UtcNow;

        //    _context.ReviewsAndRatings.Add(review);
        //    await _context.SaveChangesAsync();

        //    var savedReview = await _context.ReviewsAndRatings
        //        .Include(r => r.User)
        //        .FirstAsync(r => r.ReviewId == review.ReviewId);

        //    var result =
        //        _mapper.Map<ShowReviewsAndRatingsDTO>(savedReview);

        //    return CreatedAtAction(
        //        nameof(GetReview),
        //        new { id = review.ReviewId },
        //        result);
        //}

        //// PUT: api/ReviewsAndRatings/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateRating(
        //    int id,
        //    [FromBody] UpdateRatingDTO dto)
        //{
        //    if (dto.Rating < 1 || dto.Rating > 5)
        //        return BadRequest("הדירוג חייב להיות בין 1 ל־5");

        //    var review = await _context.ReviewsAndRatings
        //        .FindAsync(id);

        //    if (review == null)
        //        return NotFound("התגובה לא נמצאה");

        //    review.Rating = dto.Rating;

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //// DELETE: api/ReviewsAndRatings/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteReview(int id)
        //{
        //    var review = await _context.ReviewsAndRatings
        //        .FindAsync(id);

        //    if (review == null)
        //        return NotFound("התגובה לא נמצאה");

        //    _context.ReviewsAndRatings.Remove(review);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}