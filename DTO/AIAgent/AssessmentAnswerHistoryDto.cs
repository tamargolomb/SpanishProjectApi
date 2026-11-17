using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.AIAgent
{
        public class AssessmentAnswerHistoryDto
        {
            public string Question { get; set; } = string.Empty;

            public string LevelName { get; set; } = string.Empty;

            public string SelectedAnswer { get; set; } = string.Empty;

            public string CorrectAnswer { get; set; } = string.Empty;

            public bool IsCorrect { get; set; }
        }
    

}
