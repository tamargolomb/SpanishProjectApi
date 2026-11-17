using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.AIAgent
{
    public class AnswerAssessmentResponseDto
    {
        public bool IsCorrect { get; set; }

        public int QuestionsAnswered { get; set; }

        public bool IsFinished { get; set; }

        public AssessmentQuestionDto? NextQuestion { get; set; }
    }
}
