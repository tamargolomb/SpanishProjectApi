using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Lesson
{

    public class AddViewsDTO
    {
        [Required]
        public int? LessonId { get; set; }
    }

}
