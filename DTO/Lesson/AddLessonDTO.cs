using DTO.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Lesson
{

    public class AddLessonDTO
    {
        [Required(ErrorMessage = "lesson title is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "lesson title must be between 2 and 20 characters")]
        public string LessonTitle { get; set; } = string.Empty;


        [Required(ErrorMessage = "lesson description is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "lesson description must be between 2 and 100 characters")]
        public string? LessonDescription { get; set; }


        [Required(ErrorMessage = "content type is required")]
        public string ContentType { get; set; } = string.Empty;


        [Required(ErrorMessage = "file url is required")]
        public string? FileUrl { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a level")]
        public int LevelId { get; set; }

        public int UserId {  get; set; }

        [AllowedExtensions(".jpg", ".jpeg", ".png", ".webp")]
        [MaxFileSize(5)]
        public IFormFile? image { get; set; }
    }

}
