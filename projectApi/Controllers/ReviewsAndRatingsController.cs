using AutoMapper;
using BLL.Services;
using DAL;
using DTO;
using DTO.Lesson;
using DTO.ReviewsAndRatings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectApi.Extensions;

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
        public IActionResult Add(int id,[FromBody] AddResponseDTO dto)
        {
            var userId = User.GetUserId();

            var review = _ireviewandratingservice.AddReview(userId, dto);

            return Ok(review);
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
            var updatedReview = _ireviewandratingservice.UpdateReview(id, dto, User.GetUserId());
            if (updatedReview == null)
                return NotFound();

            return Ok(updatedReview);
        }
    }
}