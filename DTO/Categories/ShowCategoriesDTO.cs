using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Categories
{
        public class ShowCategoriesDTO
        {
            public int CategoryId { get; set; }

            public string CategoryName { get; set; } = string.Empty;

            public string? CategoryDescription { get; set; }
        }

}
