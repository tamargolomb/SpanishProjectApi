using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.AIAgent
{
        public class GeneratedQuestionDto
        {
            public string Question { get; set; } = string.Empty;

            public string Level { get; set; } = string.Empty;

            public List<string> Options { get; set; } = new();

            public int CorrectAnswerIndex { get; set; }
        }
}


