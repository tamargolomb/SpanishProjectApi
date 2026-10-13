using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReviewsAndRatings
{

    public class AddResponseDTO
    {
        public int LessonId { get; set; }

        public int Rating { get; set; }

        public string? ReviewText { get; set; }
    }

}
