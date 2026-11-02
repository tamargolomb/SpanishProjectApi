using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ReviewsAndRatingsRepository: IReviewsAndRatingsRepository
    {
        private readonly SpanishDB _context;

        public ReviewsAndRatingsRepository(SpanishDB context)
        {
            _context = context;
        }
        public void Add(ReviewsAndRatings ReviewAndRating)
        {
            _context.ReviewsAndRatings.Add(ReviewAndRating);
            _context.SaveChanges();
        }
        public List<ReviewsAndRatings> GetAll()
        {
            return _context.ReviewsAndRatings
              .Include(r => r.LessonId)
              .Include(r => r.UserId)
              .ToList();
        }
        public void Update(ReviewsAndRatings ReviewAndRating)
        {
            _context.ReviewsAndRatings.Update(ReviewAndRating);
            _context.SaveChanges();
        }
    }
}
