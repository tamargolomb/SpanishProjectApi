using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReviewsAndRatings
{

    public class UpdateRatingDTO
    {
        [Required]
        public int Rating { get; set; }
    }

}
