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
            
            You are an expert Spanish placement test question writer
            for an English-language educational website.

            The website interface language is English.
            The language being assessed is Spanish.

            Generate exactly ONE multiple-choice question
            appropriate for the requested Spanish proficiency level.

            LANGUAGE RULES:
            - All instructions and question wording must be in English.
            - Spanish words, phrases, and sentences may appear as the
              language-learning content being tested.
            - Never write instructions or questions entirely in Spanish.
            - Never mix Spanish instructions with English sentences.
            - Translation questions must clearly specify the direction:
              English to Spanish or Spanish to English.
            - Never use Hebrew or any other interface language.

            DIFFICULTY RULES:

            BEGINNER:
            - Assume the student may have ZERO prior Spanish knowledge.
            - Focus on very basic vocabulary, greetings, numbers,
              colors, common objects, and simple expressions.
            - Use short and clear English questions.
            - Prefer recognition of individual Spanish words.
            - Avoid verb conjugation questions in early beginner questions.
            - Avoid long Spanish sentences, advanced grammar,
              and complicated vocabulary.
            - Make distractors simple and clearly distinguishable.

            INTERMEDIATE:
            - Use everyday conversations and common grammar.
            - Include basic past and future tenses.
            - Test sentence comprehension and common verb forms.
            - Keep the English instructions clear and concise.

            ADVANCED:
            - Test complex Spanish grammar and vocabulary.
            - Include nuanced sentence meanings and reading comprehension.
            - Use challenging but unambiguous questions.

            QUESTION QUALITY RULES:
            - Exactly four answer options.
            - Exactly one correct answer.
            - All options must be plausible and clearly written.
            - Avoid trick questions and ambiguous answers.
            - Avoid repeating the same question.
            - Match the difficulty to the requested level.
            - Do not include explanations or hints.

            OUTPUT RULES:
            Return ONLY a valid JSON object.
            Do not include Markdown or additional text.

            Required JSON format:
            {
              "question": "How do you say 'Hello' in Spanish?",
              "level": "Beginner",
              "options": [
                "Hola",
                "Gracias",
                "Adiós",
                "Por favor"
              ],
              "correctAnswerIndex": 0
            }
            
            """;

        public AssessmentService(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }



        public async Task<GeneratedQuestionDto> GenerateQuestionAsync(
            string level,
            CancellationToken ct,
            IEnumerable<string>? previousQuestions = null)
        {
            var askedQuestions = previousQuestions?
                .Where(q => !string.IsNullOrWhiteSpace(q))
                .ToList() ?? new List<string>();

            for (int attempt = 1; attempt <= 4; attempt++)
            {
                var previousQuestionsJson =
                    JsonSerializer.Serialize(askedQuestions);

                var messages = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, SystemPrompt),
            new ChatMessage(
                ChatRole.User,
                $"""
                Generate ONE new Spanish placement question
                at level {level}.

                Questions already asked:
                {previousQuestionsJson}

                IMPORTANT:
                - Never repeat any previously asked question.
                - Do not rephrase a previous question.
                - Do not test the exact same word or concept
                  in essentially the same way.
                - Choose a different vocabulary item or
                  grammar concept when possible.
                - Return only the required JSON.
                """)
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

                var normalizedNewQuestion =
                    NormalizeQuestion(question.Question);

                var isDuplicate = askedQuestions.Any(q =>
                    NormalizeQuestion(q) == normalizedNewQuestion);

                if (!isDuplicate)
                    return question;

                askedQuestions.Add(question.Question);
            }

            throw new InvalidOperationException(
                "Could not generate a unique question.");
        }

        private static string NormalizeQuestion(string question)
        {
            return new string(
                question
                    .Where(char.IsLetterOrDigit)
                    .Select(char.ToLowerInvariant)
                    .ToArray()
            );
        }



        public async Task<int> ChooseNextLevelAsync(
            List<ShowLevelDTO> levels,
            List<AssessmentAnswerHistoryDto> history,
            CancellationToken ct,
            bool isFinal = false)
        {
            if (levels == null || levels.Count == 0)
                throw new InvalidOperationException("No levels are available.");

            var levelsJson = JsonSerializer.Serialize(levels);
            var historyJson = JsonSerializer.Serialize(history);

            var taskInstruction = isFinal
                ? "The assessment is finished. Choose the level that best represents the student's demonstrated Spanish ability. Do not choose a harder level just to challenge the student."
                : history.Count == 0
                    ? "This is the first question. Choose an accessible starting level for a placement assessment."
                    : "Choose the most suitable difficulty level for the next question. Increase difficulty when the student demonstrates mastery and decrease difficulty when the student struggles.";

            var messages = new List<ChatMessage>
    {
        new ChatMessage(
            ChatRole.System,
            """
            
            You are an expert adaptive Spanish placement examiner.

            You receive available levels and student answer history.

            Evaluate performance across the entire history.

            ANSWER EVALUATION RULES:
            - A correct answer demonstrates knowledge.
            - An incorrect answer indicates difficulty with the material.
            - A selected answer of "I don't know" means the student
              explicitly indicated they do not know the answer.
            - Treat "I don't know" as incorrect for scoring purposes.
            - Distinguish between an attempted incorrect answer
              and an explicit "I don't know" response.
            - Multiple "I don't know" answers may indicate that
              the current difficulty is too high.
            - Do not decrease difficulty dramatically because
              of a single incorrect or unknown answer.
            - Increase difficulty when the student consistently
              demonstrates mastery.
            - Decrease difficulty when the student repeatedly
              struggles or selects "I don't know".
            - Evaluate the overall pattern, not just the last answer.

            LEVEL SELECTION RULES:
            - Consider the question difficulty and performance.
            - Use level names and descriptions to understand difficulty.
            - Do not assume LevelId represents difficulty order.
            - Select only a LevelId that exists in the provided levels.
            - Never invent a level.

            Return only valid JSON without Markdown or explanations.

            Required JSON format:
            {"levelId": 1}
            
            """
        ),
        new ChatMessage(
            ChatRole.User,
            $"""
            Available levels:
            {levelsJson}

            Answer history:
            {historyJson}

            Task:
            {taskInstruction}

            Return the selected level ID.
            """
        )
    };

            var response = await _chatClient.GetResponseAsync(
                messages,
                cancellationToken: ct);

            var json = response.Text?.Trim();

            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("AI returned an empty response.");

            if (json.StartsWith("```"))
            {
                var firstLineEnd = json.IndexOf('\n');
                var lastFence = json.LastIndexOf("```", StringComparison.Ordinal);

                if (firstLineEnd < 0 || lastFence <= firstLineEnd)
                    throw new InvalidOperationException("Invalid AI response format.");

                json = json.Substring(
                    firstLineEnd + 1,
                    lastFence - firstLineEnd - 1).Trim();
            }

            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty(
                "levelId",
                out var levelIdElement))
            {
                throw new InvalidOperationException("AI did not return a level ID.");
            }

            var levelId = levelIdElement.GetInt32();

            if (!levels.Any(l => l.LevelId == levelId))
                throw new InvalidOperationException(
                    "AI selected a level that does not exist.");

            return levelId;
        }



    }
}