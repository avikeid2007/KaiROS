using System;
using System.ComponentModel.DataAnnotations;

namespace KAIROS.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        
        public required string Content { get; set; }
        
        public required string Role { get; set; } // "user" or "assistant"
        
        public DateTime Timestamp { get; set; }
        
        public int ConversationId { get; set; }
        
        public Conversation? Conversation { get; set; }
    }
}
