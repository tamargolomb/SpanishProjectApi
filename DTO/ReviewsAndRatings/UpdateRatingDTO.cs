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

        [Range(0, 5)]
        public int Rating { get; set; }
    }

}
