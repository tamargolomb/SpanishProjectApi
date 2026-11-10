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
        public ReviewsAndRatings? GetById(int id)
        {
            return _context.ReviewsAndRatings
                .Include(r => r.LessonId)
              .Include(r => r.UserId)
              .FirstOrDefault(r => r.ReviewId == id);
        }
        public void Update(ReviewsAndRatings ReviewAndRating)
        {
            _context.ReviewsAndRatings.Update(ReviewAndRating);
            _context.SaveChanges();
        }
        public bool Delete(int id)
        {
            var review = _context.ReviewsAndRatings.Find(id);
            if (review == null)
                return false;

            _context.ReviewsAndRatings.Remove(review);
            _context.SaveChanges();
            return true;
        }

        public ReviewsAndRatings? GetOwned(int id, int userId) =>
            _context.ReviewsAndRatings.FirstOrDefault(r => r.ReviewId == id && r.UserId == userId);
    }
}
