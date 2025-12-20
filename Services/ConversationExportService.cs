using KAIROS.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAIROS.Services
{
    public class ConversationExportService : IConversationExportService
    {
        public Task<string> ExportToMarkdownAsync(Conversation conversation)
        {
            if (conversation == null)
                throw new ArgumentNullException(nameof(conversation));

            var sb = new StringBuilder();
            
            // Title
            sb.AppendLine($"# {conversation.Title}");
            sb.AppendLine();
            
            // Metadata
            sb.AppendLine($"**Created:** {conversation.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"**Last Updated:** {conversation.LastUpdated:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"**Messages:** {conversation.Messages?.Count ?? 0}");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // Messages
            if (conversation.Messages != null)
            {
                foreach (var message in conversation.Messages.OrderBy(m => m.Timestamp))
                {
                    var role = message.Role == "user" ? "👤 **You**" : "🤖 **AI Assistant**";
                    sb.AppendLine($"### {role}");
                    sb.AppendLine($"*{message.Timestamp:yyyy-MM-dd HH:mm:ss}*");
                    sb.AppendLine();
                    sb.AppendLine(message.Content);
                    sb.AppendLine();
                    sb.AppendLine("---");
                    sb.AppendLine();
                }
            }

            // Footer
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine($"*Exported from KAIROS AI Chat Assistant on {DateTime.Now:yyyy-MM-dd HH:mm:ss}*");

            return Task.FromResult(sb.ToString());
        }

        public async Task ExportToFileAsync(Conversation conversation, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            var markdown = await ExportToMarkdownAsync(conversation);
            await File.WriteAllTextAsync(filePath, markdown);
        }
    }
}
