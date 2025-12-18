using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KAIROS.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        
        public required string Title { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime LastUpdated { get; set; }
        
        public List<ChatMessage> Messages { get; set; } = new();
    }
}
