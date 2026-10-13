using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Lesson
{

    public class ShowLessonDTO
    {
        public int LessonId { get; set; }

        public string LessonTitle { get; set; } = string.Empty;

        public string? LessonDescription { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public string? FileUrl { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int LevelId { get; set; }

        public string? LevelName { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public int ViewCount { get; set; }

        public double Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
