using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Categories
{

    public class AddCategoriesDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? CategoryDescription { get; set; }
    }

}
