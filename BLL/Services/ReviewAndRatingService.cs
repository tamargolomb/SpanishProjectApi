using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
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
        public void Add(ReviewsAndRatings reviewAndRating)
        {
            _ReviewsAndRatingsRepository.Add(reviewAndRating);
        }
        public void Update(ReviewsAndRatings reviewAndRating)
        {
            _ReviewsAndRatingsRepository.Update(reviewAndRating);
        }
        public List<ShowReviewsAndRatingsDTO> GetAllreviews()
        {
            var reviews = _ReviewsAndRatingsRepository.GetAll();
            return _mapper.Map<List<ShowReviewsAndRatingsDTO>>(reviews);
        }
    }
}
