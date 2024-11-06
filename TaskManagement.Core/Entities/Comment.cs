using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Entities
{
    public class Comment
    {
        public int CommentId { get; set; } 
        public int TaskId { get; set; } 
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime? Timestamp { get; set; } = DateTime.Now;

        // Navigation properties
        public Task? Task { get; set; } 
        public User? User { get; set; } 
    }

}
