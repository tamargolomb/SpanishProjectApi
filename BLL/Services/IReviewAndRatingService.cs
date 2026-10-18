using DAL;
using DTO;
using DTO.ReviewsAndRatings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface IReviewAndRatingService
    {
        void Add(ReviewsAndRatings reviewAndRating);
        void Update(ReviewsAndRatings reviewAndRating);
        List<ShowReviewsAndRatingsDTO> GetAllreviews();
    }
}
