using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Categories
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public SubCategory SubCategory { get; set; }
        public  List<Lesson> Lessons { get; set; }

    }
    public enum SubCategory
    {
        Beginner,
        Intermediate,
        Advanced
    }
}
