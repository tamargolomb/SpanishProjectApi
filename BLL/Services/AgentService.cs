using DTO.AIAgent;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BLL.Services
{
    public class AgentService: IAgentService
    {
        private const string SystemPrompt = """
           אתה מורה פרטי חכם ומומחה ללימוד ספרדית באתר.
           כאשר המשתמשת מבקשת להשתפר בנושא מסוים (כמו פעלים):
           1. השתמש מיד בכלי GetUserProgress כדי לבדוק אילו תרגילים היא כבר פתרה ואיפה היא טועה.
           2. זהה את דפוס הטעויות שלה.
           3. בחר עבורה תרגילים מתאימים או הצע חומרי לימוד שהועלו למערכת, והנחה אותה צעד אחר צעד.
           היה מעודד, סבלני, ודבר בעברית נעימה.
           """;
        private readonly IChatClient _chatClient;

        public AgentService(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        private List<ChatMessage> BuildMessages(List<ChatMessageDto> history)
        {
            var messages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, SystemPrompt)
            };

            foreach (var m in history)
            {
                var role = m.Role == "assistant" ? ChatRole.Assistant : ChatRole.User;
                messages.Add(new ChatMessage(role, m.Text));
            }

            return messages;
        }

        public async Task<string> AskAsync(List<ChatMessageDto> history, CancellationToken ct)
        {
            var response = await _chatClient.GetResponseAsync(BuildMessages(history), cancellationToken: ct);
            return response.Text;
        }
        public async IAsyncEnumerable<string> AskStreamingAsync(List<ChatMessageDto> history,[EnumeratorCancellation] CancellationToken ct)
        {
            var updates = _chatClient.GetStreamingResponseAsync(BuildMessages(history), cancellationToken: ct);

            await foreach (var update in updates)
            {
                if (!string.IsNullOrEmpty(update.Text))
                    yield return update.Text;
            }
        }
    }
}
