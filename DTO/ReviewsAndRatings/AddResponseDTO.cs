using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReviewsAndRatings
{

    public class AddResponseDTO
    {
        [Required]
        public int? LessonId { get; set; }

        [Range(0, 5)]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "review text is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "review text must be between 2 and 100 characters")]
        public string? ReviewText { get; set; }
    }

}
