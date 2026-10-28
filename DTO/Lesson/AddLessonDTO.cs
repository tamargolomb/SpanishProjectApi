using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DTO.Lesson
{

    public class AddLessonDTO
    {
        public string LessonTitle { get; set; } = string.Empty;

        public string? LessonDescription { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public IFormFile FileUrl { get; set; }

        public int CategoryId { get; set; }

        public int LevelId { get; set; }

        public int UserId {  get; set; }
    }

}
