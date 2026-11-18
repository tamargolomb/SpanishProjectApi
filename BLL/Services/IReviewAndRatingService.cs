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
<<<<<<< HEAD
        ShowReviewsAndRatingsDTO? UpdateReview(int id, UpdateRatingDTO dto, int userId);
=======
        ShowReviewsAndRatingsDTO UpdateReview(int id, UpdateRatingDTO dto, int userId);
>>>>>>> 77443eaceb9893618d73eecb87ade84b37397ccd
        List<ShowReviewsAndRatingsDTO> GetAllreviews();
        bool DeleteReview(int id, int userId);
         
    }
}
