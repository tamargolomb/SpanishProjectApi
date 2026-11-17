using BLL.Models;
using DTO.AIAgent;
using DTO.AIAgent;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class AssessmentSessionService : IAssessmentSessionService
    {
        private const int MaxQuestions = 10;
        private readonly IMemoryCache _cache;
        private readonly AssessmentService _assessmentService;
        private readonly ILevelService _levelService;

        public AssessmentSessionService(
            IMemoryCache cache,
            AssessmentService assessmentService,
            ILevelService levelService)
        {
            _cache = cache;
            _assessmentService = assessmentService;
            _levelService = levelService;
        }

        public async Task<StartAssessmentResponseDto> StartAsync(
            string userId,
            CancellationToken ct)
        {
            var levels = _levelService.GetAllLevels();

            if (levels.Count == 0)
            {
                throw new InvalidOperationException(
                    "No assessment levels are configured.");
            }

              var firstLevelId = await _assessmentService.ChooseNextLevelAsync(levels,
                new List<AssessmentAnswerHistoryDto>(),ct);

            var firstLevel = levels.First(l => l.LevelId == firstLevelId);

            var generatedQuestion =
                await _assessmentService.GenerateQuestionAsync(
                    firstLevel.LevelName,
                    ct);

            var session = new AssessmentSession
            {
                AssessmentId = Guid.NewGuid(),
                UserId = userId,
                CurrentLevelId = firstLevel.LevelId,
                CurrentQuestion = generatedQuestion,
                QuestionsAnswered = 0,
                CorrectAnswers = 0
            };

            _cache.Set(
                session.AssessmentId,
                session,
                TimeSpan.FromMinutes(30));

            return new StartAssessmentResponseDto
            {
                AssessmentId = session.AssessmentId,
                Question = new AssessmentQuestionDto
                {
                    Question = generatedQuestion.Question,
                    Level = firstLevel.LevelName,
                    Options = generatedQuestion.Options.ToList()
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
            {
                throw new UnauthorizedAccessException(
                    "This assessment belongs to another user.");
            }

            if (request.SelectedAnswerIndex < 0 ||
                request.SelectedAnswerIndex >= session.CurrentQuestion.Options.Count)
            {
                throw new ArgumentException(
                    "Invalid answer index.");
            }

            if (session.QuestionsAnswered >= MaxQuestions)
            {
                throw new InvalidOperationException(
                    "Assessment is already finished.");
            }

            var currentQuestion = session.CurrentQuestion;

            var isCorrect =
                request.SelectedAnswerIndex ==
                currentQuestion.CorrectAnswerIndex;

            var selectedAnswer =
                currentQuestion.Options[request.SelectedAnswerIndex];

            var correctAnswer =
                currentQuestion.Options[currentQuestion.CorrectAnswerIndex];

            session.AnswerHistory.Add(new AssessmentAnswerHistoryDto
            {
                Question = currentQuestion.Question,
                LevelName = currentQuestion.Level,
                SelectedAnswer = selectedAnswer,
                CorrectAnswer = correctAnswer,
                IsCorrect = isCorrect
            });

            session.QuestionsAnswered++;

            if (isCorrect)
            {
                session.CorrectAnswers++;
            }

            if (session.QuestionsAnswered >= MaxQuestions)
            {
                _cache.Set(
                    session.AssessmentId,
                    session,
                    TimeSpan.FromMinutes(30));

                return new AnswerAssessmentResponseDto
                {
                    IsCorrect = isCorrect,
                    QuestionsAnswered = session.QuestionsAnswered,
                    IsFinished = true,
                    NextQuestion = null
                };
            }

            var levels = _levelService.GetAllLevels();

            var nextLevelId =
                await _assessmentService.ChooseNextLevelAsync(
                    levels,
                    session.AnswerHistory,
                    ct);

            var nextLevel = levels.First(
                l => l.LevelId == nextLevelId);

            var nextQuestion =
                await _assessmentService.GenerateQuestionAsync(
                    nextLevel.LevelName,
                    ct);

            session.CurrentLevelId = nextLevel.LevelId;
            session.CurrentQuestion = nextQuestion;

            _cache.Set(
                session.AssessmentId,
                session,
                TimeSpan.FromMinutes(30));

            return new AnswerAssessmentResponseDto
            {
                IsCorrect = isCorrect,
                QuestionsAnswered = session.QuestionsAnswered,
                IsFinished = false,
                NextQuestion = new AssessmentQuestionDto
                {
                    Question = nextQuestion.Question,
                    Level = nextLevel.LevelName,
                    Options = nextQuestion.Options.ToList()
                }
            };
        }

    }
}
