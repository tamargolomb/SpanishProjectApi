
using BLL.Models;
using DTO.AIAgent;
using DTO.AIAgent;
using Microsoft.Extensions.Caching.Memory;

namespace BLL.Services
{
    public class AssessmentSessionService : IAssessmentSessionService
    {
        private const int MaxQuestions = 10;

        private static readonly TimeSpan SessionLifetime =
            TimeSpan.FromMinutes(30);

        private readonly IMemoryCache _cache;
        private readonly AssessmentService _assessmentService;
        private readonly ILevelService _levelService;
        private readonly IUserService _userService;

        public AssessmentSessionService(
            IMemoryCache cache,
            AssessmentService assessmentService,
            ILevelService levelService,
            IUserService userService)
        {
            _cache = cache;
            _assessmentService = assessmentService;
            _levelService = levelService;
            _userService = userService;
        }

        public async Task<StartAssessmentResponseDto> StartAsync(
            string userId,
            CancellationToken ct)
        {
            if (!int.TryParse(userId, out _))
                throw new ArgumentException("Invalid user ID.");

            var levels = _levelService.GetAllLevels();

            if (levels.Count == 0)
                throw new InvalidOperationException(
                    "No levels are available.");

            var firstLevelId =
                await _assessmentService.ChooseNextLevelAsync(
                    levels,
                    new List<AssessmentAnswerHistoryDto>(),
                    ct);

            var firstLevel = levels.First(
                l => l.LevelId == firstLevelId);

            var firstQuestion =
                await _assessmentService.GenerateQuestionAsync(
                    firstLevel.LevelName,
                    ct);

            var session = new AssessmentSession
            {
                AssessmentId = Guid.NewGuid(),
                UserId = userId,
                CurrentLevelId = firstLevel.LevelId,
                CurrentQuestion = firstQuestion,
                QuestionsAnswered = 0,
                CorrectAnswers = 0
            };

            _cache.Set(
                session.AssessmentId,
                session,
                SessionLifetime);

            return new StartAssessmentResponseDto
            {
                AssessmentId = session.AssessmentId,
                Question = new AssessmentQuestionDto
                {
                    Question = firstQuestion.Question,
                    Level = firstLevel.LevelName,
                    Options = firstQuestion.Options.ToList()
                }
            };
        }

        public async Task<AnswerAssessmentResponseDto> AnswerAsync(
            string userId,
            AnswerAssessmentRequestDto request,
            CancellationToken ct)
        {
            if (!_cache.TryGetValue(
                request.AssessmentId,
                out AssessmentSession? session) || session == null)
            {
                throw new KeyNotFoundException(
                    "Assessment was not found or has expired.");
            }

            if (session.UserId != userId)
                throw new UnauthorizedAccessException(
                    "This assessment belongs to another user.");

            await session.SyncLock.WaitAsync(ct);

            try
            {
                if (session.IsFinished)
                    throw new InvalidOperationException(
                        "Assessment is already finished.");

                if (session.QuestionsAnswered >= MaxQuestions)
                    throw new InvalidOperationException(
                        "Maximum number of questions reached.");

                var expectedQuestionNumber =
                    session.QuestionsAnswered + 1;

                if (request.QuestionNumber != expectedQuestionNumber)
                    throw new InvalidOperationException(
                        "This question was already answered or is not current.");

                var currentQuestion = session.CurrentQuestion;

                if (request.SelectedAnswerIndex < 0 ||
                    request.SelectedAnswerIndex >= currentQuestion.Options.Count)
                {
                    throw new ArgumentException(
                        "Invalid answer index.");
                }

                var isCorrect =
                    request.SelectedAnswerIndex ==
                    currentQuestion.CorrectAnswerIndex;

                var answer = new AssessmentAnswerHistoryDto
                {
                    Question = currentQuestion.Question,
                    LevelName = currentQuestion.Level,
                    SelectedAnswer = currentQuestion.Options[
                        request.SelectedAnswerIndex],
                    CorrectAnswer = currentQuestion.Options[
                        currentQuestion.CorrectAnswerIndex],
                    IsCorrect = isCorrect
                };

                var updatedHistory =
                    new List<AssessmentAnswerHistoryDto>(
                        session.AnswerHistory)
                    {
                        answer
                    };

                var newQuestionsAnswered =
                    session.QuestionsAnswered + 1;

                var newCorrectAnswers =
                    session.CorrectAnswers + (isCorrect ? 1 : 0);

                var levels = _levelService.GetAllLevels();

                if (levels.Count == 0)
                    throw new InvalidOperationException(
                        "No levels are available.");

                if (newQuestionsAnswered == MaxQuestions)
                {
                    var finalLevelId =
                        await _assessmentService.ChooseNextLevelAsync(
                            levels,
                            updatedHistory,
                            ct,
                            isFinal: true);

                    var finalLevel = levels.First(
                        l => l.LevelId == finalLevelId);

                    if (!int.TryParse(userId, out var numericUserId))
                        throw new InvalidOperationException(
                            "Invalid user ID.");

                    var saved =
                        await _userService.UpdateUserLevelAsync(
                            numericUserId,
                            finalLevelId,
                            ct);

                    if (!saved)
                        throw new KeyNotFoundException(
                            "User was not found.");

                    session.AnswerHistory = updatedHistory;
                    session.QuestionsAnswered = newQuestionsAnswered;
                    session.CorrectAnswers = newCorrectAnswers;
                    session.FinalLevelId = finalLevelId;
                    session.FinalLevelName = finalLevel.LevelName;
                    session.IsFinished = true;

                    _cache.Set(
                        session.AssessmentId,
                        session,
                        SessionLifetime);

                    return new AnswerAssessmentResponseDto
                    {
                        IsCorrect = isCorrect,
                        QuestionsAnswered = newQuestionsAnswered,
                        IsFinished = true,
                        NextQuestion = null,
                        FinalLevelId = finalLevelId,
                        FinalLevelName = finalLevel.LevelName
                    };
                }

                var nextLevelId =
                    await _assessmentService.ChooseNextLevelAsync(
                        levels,
                        updatedHistory,
                        ct);

                var nextLevel = levels.First(
                    l => l.LevelId == nextLevelId);

                var nextQuestion =
                    await _assessmentService.GenerateQuestionAsync(
                        nextLevel.LevelName,
                        ct);

                session.AnswerHistory = updatedHistory;
                session.QuestionsAnswered = newQuestionsAnswered;
                session.CorrectAnswers = newCorrectAnswers;
                session.CurrentLevelId = nextLevelId;
                session.CurrentQuestion = nextQuestion;

                _cache.Set(
                    session.AssessmentId,
                    session,
                    SessionLifetime);

                return new AnswerAssessmentResponseDto
                {
                    IsCorrect = isCorrect,
                    QuestionsAnswered = newQuestionsAnswered,
                    IsFinished = false,
                    NextQuestion = new AssessmentQuestionDto
                    {
                        Question = nextQuestion.Question,
                        Level = nextLevel.LevelName,
                        Options = nextQuestion.Options.ToList()
                    }
                };
            }
            finally
            {
                session.SyncLock.Release();
            }
        }
    }
}
