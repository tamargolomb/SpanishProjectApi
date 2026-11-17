using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.AIAgent
{
    public class AssessmentQuestionDto
    {
            public string Question { get; set; } = string.Empty;
            public string Level { get; set; } = string.Empty;
            public List<string> Options { get; set; } = new();
    }
}

