using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface IReviewsAndRatingsRepository
    {
        void Add(ReviewsAndRatings ReviewAndRating);
        List<ReviewsAndRatings> GetAll();
        ReviewsAndRatings GetById(int id);
        void Update(ReviewsAndRatings ReviewAndRating);
        void Delete(int id);
    }
}
