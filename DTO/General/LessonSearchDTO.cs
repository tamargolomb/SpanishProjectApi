using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.General
{
    //לשמירת בקשת חיפוש והעברתה לשרת
    public class LessonSearchDTO
    {
        public string? SearchText { get; set; }

        public int? CategoryId { get; set; }

        public int? LevelId { get; set; }
    }
}
