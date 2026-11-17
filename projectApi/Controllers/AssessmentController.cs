
using BLL.Services;
using DTO.AIAgent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssessmentController : ControllerBase
    {
        private readonly AssessmentService _assessmentService;
        private readonly ILevelService _levelService;
        private readonly IAssessmentSessionService _sessionService;
        public AssessmentController(
            AssessmentService assessmentService,
            ILevelService levelService,
            IAssessmentSessionService sessionService)
        {
            _assessmentService = assessmentService;
            _levelService = levelService;
            _sessionService = sessionService;
        }

        [HttpPost("generate-question")]
        public async Task<IActionResult> GenerateQuestion(
            [FromBody] GenerateQuestionRequest request,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Level))
            {
                return BadRequest(new
                {
                    message = "Level is required."
                });
            }

            var levels = _levelService.GetAllLevels();

            var selectedLevel = levels.FirstOrDefault(
                l => string.Equals(
                    l.LevelName,
                    request.Level.Trim(),
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (selectedLevel == null)
            {
                return BadRequest(new
                {
                    message = "The requested level does not exist."
                });
            }

            var question = await _assessmentService.GenerateQuestionAsync(
                selectedLevel.LevelName,
                ct);

            return Ok(new
            {
                question
            });
        }


        [HttpPost("start")]
        public async Task<IActionResult> Start(CancellationToken ct)
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID was not found in the token."
                });
            }

            var result = await _sessionService.StartAsync(userId, ct);

            return Ok(result);
        }

        [HttpPost("answer")]
        public async Task<IActionResult> Answer(
            [FromBody] AnswerAssessmentRequestDto request,
            CancellationToken ct)
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID was not found in the token."
                });
            }

            try
            {
                var result = await _sessionService.AnswerAsync(
                    userId,
                    request,
                    ct);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        public class GenerateQuestionRequest
        {
            public string Level { get; set; } = string.Empty;
        }
    }
}
