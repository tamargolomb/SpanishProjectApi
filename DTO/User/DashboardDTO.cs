using DTO.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{

    public class DashboardDTO
    {
        public string UserName { get; set; } = string.Empty;

        public string CurrentLevel { get; set; } = string.Empty;

        public int TotalScore { get; set; }

        public List<ShowLessonDTO> RecommendedLessons { get; set; }
            = new List<ShowLessonDTO>();
    }

}
