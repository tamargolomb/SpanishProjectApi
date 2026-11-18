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
        ShowReviewsAndRatingsDTO AddReview(int userId, AddResponseDTO dto);

        ShowReviewsAndRatingsDTO? UpdateReview(int id, UpdateRatingDTO dto, int userId);

        List<ShowReviewsAndRatingsDTO> GetAllreviews();
        bool DeleteReview(int id, int userId);
         
    }
}
