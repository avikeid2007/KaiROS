using KAIROS.Models;
using System.Threading.Tasks;

namespace KAIROS.Services
{
    public interface IConversationExportService
    {
        Task<string> ExportToMarkdownAsync(Conversation conversation);
        Task ExportToFileAsync(Conversation conversation, string filePath);
    }
}
