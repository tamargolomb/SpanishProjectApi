using DTO.AIAgent;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface IAssessmentSessionService
    {
        Task<StartAssessmentResponseDto> StartAsync(
            string userId,
            CancellationToken ct);
        Task<AnswerAssessmentResponseDto> AnswerAsync(
          string userId,
          AnswerAssessmentRequestDto request,
          CancellationToken ct);
    }
}

