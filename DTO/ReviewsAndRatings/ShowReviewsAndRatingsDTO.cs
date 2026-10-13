using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReviewsAndRatings
{

    public class ShowReviewsAndRatingsDTO
    {
        public int ReviewId { get; set; }

        public int LessonId { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public int Rating { get; set; }

        public string? ReviewText { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
