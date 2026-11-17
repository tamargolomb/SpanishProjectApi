using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Categories;
using DTO.ReviewsAndRatings;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class ReviewAndRatingService: IReviewAndRatingService
    {
        private readonly IReviewsAndRatingsRepository _ReviewsAndRatingsRepository;
        private readonly IMapper _mapper;
        public ReviewAndRatingService(IReviewsAndRatingsRepository reviewsAndRatingsRepository, IMapper mapper)
        {
            _ReviewsAndRatingsRepository = reviewsAndRatingsRepository;
            _mapper = mapper;
        }
        public ShowReviewsAndRatingsDTO AddReview(int userId, AddResponseDTO dto)
        {
            var review = _mapper.Map<ReviewsAndRatings>(dto);

            review.UserId = userId;
            review.CreatedAt = DateTime.UtcNow;

            _ReviewsAndRatingsRepository.Add(review);

            return _mapper.Map<ShowReviewsAndRatingsDTO>(review);
        }
        public ShowReviewsAndRatingsDTO? UpdateReview(int id, UpdateRatingDTO dto, int userId)
        {
            var existingReview =
                _ReviewsAndRatingsRepository.GetOwned(id, userId);

            if (existingReview == null)
                return null;

            existingReview.Rating = dto.Rating;

            _ReviewsAndRatingsRepository.Update(existingReview);

            return _mapper.Map<ShowReviewsAndRatingsDTO>(existingReview);
        }
        public List<ShowReviewsAndRatingsDTO> GetAllreviews()
        {
            var reviews = _ReviewsAndRatingsRepository.GetAll();
            return _mapper.Map<List<ShowReviewsAndRatingsDTO>>(reviews);
        }
        public bool DeleteReview(int id, int userId)
        {
            var review = _ReviewsAndRatingsRepository.GetOwned(id, userId);
            if (review is null)
                return false;

            _ReviewsAndRatingsRepository.Delete(id);
            return true;
        }
    
    }
}
