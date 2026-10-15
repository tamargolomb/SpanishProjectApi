using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.General
{
    //מגיע מהagent אז כרגע לא התייחסתי לזה
    public class LevelTestResultDTO
    {
        public int Score { get; set; }

        public int AssignedLevelId { get; set; }

        public string? FeedbackText { get; set; }
    }

}
