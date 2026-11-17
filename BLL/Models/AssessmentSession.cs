using DTO.AIAgent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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

        public bool IsFinished { get; set; }

        public int? FinalLevelId { get; set; }

        public string? FinalLevelName { get; set; }

        public SemaphoreSlim SyncLock { get; } = new(1, 1);
    }
}