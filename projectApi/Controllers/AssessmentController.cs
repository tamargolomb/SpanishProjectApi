
using BLL.Services;
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

        public AssessmentController(
            AssessmentService assessmentService,
            ILevelService levelService)
        {
            _assessmentService = assessmentService;
            _levelService = levelService;
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
    }

    public class GenerateQuestionRequest
    {
        public string Level { get; set; } = string.Empty;
    }
}
