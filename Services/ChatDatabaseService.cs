using KAIROS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAIROS.Services
{
    public interface IChatDatabaseService
    {
        Task InitializeDatabaseAsync();
        Task<Conversation> CreateConversationAsync(string title);
        Task<List<Conversation>> GetAllConversationsAsync();
        Task<Conversation?> GetConversationAsync(int id);
        Task<ChatMessage> AddMessageAsync(int conversationId, string content, string role);
        Task DeleteConversationAsync(int id);
        Task UpdateConversationTitleAsync(int id, string newTitle);
    }

    public class ChatDatabaseService : IChatDatabaseService
    {
        public async Task InitializeDatabaseAsync()
        {
            using var db = new Data.ChatDbContext();
            await db.Database.EnsureCreatedAsync();
        }

        public async Task<Conversation> CreateConversationAsync(string title)
        {
            using var db = new Data.ChatDbContext();
            
            var conversation = new Conversation
            {
                Title = title,
                CreatedAt = DateTime.Now,
                LastUpdated = DateTime.Now
            };

            db.Conversations.Add(conversation);
            await db.SaveChangesAsync();

            return conversation;
        }

        public async Task<List<Conversation>> GetAllConversationsAsync()
        {
            using var db = new Data.ChatDbContext();
            return await db.Conversations
                .Include(c => c.Messages)
                .OrderByDescending(c => c.LastUpdated)
                .ToListAsync();
        }

        public async Task<Conversation?> GetConversationAsync(int id)
        {
            using var db = new Data.ChatDbContext();
            return await db.Conversations
                .Include(c => c.Messages.OrderBy(m => m.Timestamp))
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ChatMessage> AddMessageAsync(int conversationId, string content, string role)
        {
            using var db = new Data.ChatDbContext();
            
            var message = new ChatMessage
            {
                Content = content,
                Role = role,
                Timestamp = DateTime.Now,
                ConversationId = conversationId
            };

            db.ChatMessages.Add(message);

            var conversation = await db.Conversations.FindAsync(conversationId);
            if (conversation != null)
            {
                conversation.LastUpdated = DateTime.Now;
            }

            await db.SaveChangesAsync();

            return message;
        }

        public async Task DeleteConversationAsync(int id)
        {
            using var db = new Data.ChatDbContext();
            
            var conversation = await db.Conversations.FindAsync(id);
            if (conversation != null)
            {
                db.Conversations.Remove(conversation);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateConversationTitleAsync(int id, string newTitle)
        {
            using var db = new Data.ChatDbContext();
            
            var conversation = await db.Conversations.FindAsync(id);
            if (conversation != null)
            {
                conversation.Title = newTitle;
                conversation.LastUpdated = DateTime.Now;
                await db.SaveChangesAsync();
            }
        }
    }
}
