using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Levels
{

    public class AddLevelDTO
    {
        [Required(ErrorMessage = "level name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "level name must be between 2 and 20 characters")]
        public string LevelName { get; set; } = string.Empty;

        [Required(ErrorMessage = "level description is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "level description must be between 2 and 100 characters")]
        public string? LevelDescription { get; set; }
    }

}
