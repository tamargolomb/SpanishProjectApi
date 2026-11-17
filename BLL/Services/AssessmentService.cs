using System;
using System.Collections.Generic;
using System.Text;
using DTO.AIAgent;
using Microsoft.Extensions.AI;
using System.Text.Json;

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

        public async Task<string> GenerateQuestionAsync(
            string level,
            CancellationToken ct)
        {
            var messages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, SystemPrompt),
                new ChatMessage(
                    ChatRole.User,
                    $"Generate a Spanish placement question at CEFR level {level}.")
            };

            var response = await _chatClient.GetResponseAsync(
                messages,
                cancellationToken: ct);

            return response.Text ?? string.Empty;
        }
    }
}