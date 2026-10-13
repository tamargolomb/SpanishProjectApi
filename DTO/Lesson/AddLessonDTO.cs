using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Lesson
{

    public class AddLessonDTO
    {
        public string LessonTitle { get; set; } = string.Empty;

        public string? LessonDescription { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public string? FileUrl { get; set; }

        public int CategoryId { get; set; }

        public int LevelId { get; set; }
    }

}
