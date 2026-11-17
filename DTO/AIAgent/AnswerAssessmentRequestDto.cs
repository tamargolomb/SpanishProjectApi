using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.AIAgent
{
    public class AnswerAssessmentRequestDto
    {
        public Guid AssessmentId { get; set; }

        public int QuestionNumber { get; set; }

        public int SelectedAnswerIndex { get; set; }
    }

}
