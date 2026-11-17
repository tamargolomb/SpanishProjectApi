using System;
using System.Collections.Generic;
using System.Text;
using DTO.AIAgent;
using Microsoft.Extensions.AI;
using System.Text.Json;
using DTO.Levels;

namespace BLL.Services
{
    public class AssessmentService
    {
        private readonly IChatClient _chatClient;

        private const string SystemPrompt = """
            You are a professional Spanish language examiner.

            Your task is to generate placement-test questions
            for students learning Spanish.

            Generate exactly one multiple-choice question.

            Requirements:
            - Use the requested CEFR difficulty level.
            - Test Spanish vocabulary or grammar.
            - Provide exactly four answer options.
            - Exactly one option must be correct.
            - Avoid ambiguous questions.
            - Return only valid JSON.
            - Do not include Markdown formatting.

            Use this JSON structure:
            {
              "question": "Question text",
              "level": "A1",
              "options": [
                "Option 1",
                "Option 2",
                "Option 3",
                "Option 4"
              ],
              "correctAnswerIndex": 0
            }

            The correctAnswerIndex must be an integer from 0 to 3.
            """;

        public AssessmentService(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }


        public async Task<GeneratedQuestionDto> GenerateQuestionAsync(
            string level,
            CancellationToken ct)
        {
            var messages = new List<ChatMessage>
    {
        new ChatMessage(ChatRole.System, SystemPrompt),
        new ChatMessage(
            ChatRole.User,
            $"Generate a Spanish placement question at level {level}.")
    };

            var response = await _chatClient.GetResponseAsync(
                messages,
                cancellationToken: ct);

            var json = response.Text?.Trim()
                ?? throw new InvalidOperationException(
                    "The AI returned an empty response.");

            if (json.StartsWith("```"))
            {
                var firstLineEnd = json.IndexOf('\n');
                var lastFence = json.LastIndexOf("```");

                if (firstLineEnd >= 0 && lastFence > firstLineEnd)
                {
                    json = json.Substring(
                        firstLineEnd + 1,
                        lastFence - firstLineEnd - 1
                    ).Trim();
                }
            }

            var question = JsonSerializer.Deserialize<GeneratedQuestionDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (question == null ||
                string.IsNullOrWhiteSpace(question.Question) ||
                question.Options == null ||
                question.Options.Count != 4 ||
                question.Options.Any(string.IsNullOrWhiteSpace) ||
                question.CorrectAnswerIndex < 0 ||
                question.CorrectAnswerIndex > 3)
            {
                throw new InvalidOperationException(
                    "The AI generated an invalid question.");
            }

            return question;
        }

        public async Task<int> ChooseNextLevelAsync(
            List<ShowLevelDTO> levels,
            List<AssessmentAnswerHistoryDto> history,
            CancellationToken ct)
        {
            if (levels.Count == 0)
            {
                throw new InvalidOperationException(
                    "No levels are available.");
            }

            var levelsJson = JsonSerializer.Serialize(levels);
            var historyJson = JsonSerializer.Serialize(history);

            var messages = new List<ChatMessage>
    {
        new ChatMessage(
            ChatRole.System,
            """
            You are an adaptive Spanish placement examiner.

            Your task is to select the most suitable difficulty
            level for the student's next question.

            You will receive:
            1. The available levels with their IDs, names and descriptions.
            2. The student's previous questions and answers.

            Rules:
            - Evaluate the student's performance across the full history.
            - Consider both correct and incorrect answers.
            - Use the level names and descriptions to understand difficulty.
            - Increase difficulty when the student demonstrates mastery.
            - Decrease difficulty when the student struggles.
            - Select only a LevelId from the provided levels.
            - Never invent new levels.
            - Return only a JSON object with a levelId integer.
            - Do not include explanations or Markdown.
            -If the answer history is empty, select an accessible starting level suitable for a new placement assessment.
            Example:
            {"levelId": 3}
            """
        ),
        new ChatMessage(
            ChatRole.User,
            $"""
            Available levels:
            {levelsJson}

            Student answer history:
            {historyJson}

            Select the level for the next question.
            """
        )
    };

            var response = await _chatClient.GetResponseAsync(
                messages,
                cancellationToken: ct);

            var json = response.Text?.Trim()
                ?? throw new InvalidOperationException(
                    "The AI returned an empty response.");

            using var document = JsonDocument.Parse(json);

            var levelId = document.RootElement
                .GetProperty("levelId")
                .GetInt32();

            if (!levels.Any(l => l.LevelId == levelId))
            {
                throw new InvalidOperationException(
                    "The AI selected a level that does not exist.");
            }

            return levelId;
        }


    }
}