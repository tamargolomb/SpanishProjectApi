using DTO.AIAgent;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class AssessmentSession
    {
        public Guid AssessmentId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int CurrentLevelId { get; set; }

        public GeneratedQuestionDto CurrentQuestion { get; set; } = new();

        public int QuestionsAnswered { get; set; }

        public int CorrectAnswers { get; set; }
        public List<AssessmentAnswerHistoryDto> AnswerHistory { get; set; } = new();
    }
}
