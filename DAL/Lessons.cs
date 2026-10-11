using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public string LessonDescription { get; set; }
        public string ContentType { get; set; }
        public string FileUrl { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int LevelId { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public  Categories Category { get; set; }
        public  Levels Level { get; set; }
        public  Users user { get; set; }
        public  List<ReviewsAndRatings> ReviewsAndRatings { get; set; }
    }
}
