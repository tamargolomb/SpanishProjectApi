using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.AIAgent
{

        public class StartAssessmentResponseDto
        {
            public Guid AssessmentId { get; set; }

            public AssessmentQuestionDto Question { get; set; } = new();
        }
    

}
