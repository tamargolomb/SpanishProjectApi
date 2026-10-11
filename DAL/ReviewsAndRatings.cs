using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL
{
    public class ReviewsAndRatings
    {
        [Key]
        public int ReviewId { get; set; }
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public  Users User { get; set; }
        public  Lesson Lesson { get; set; }
    }
}
