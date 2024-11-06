using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskManagement.Core.Entities
{
    public class User
    {
        public int UserId { get; set; } 
        public int UserLoginId { get; set; } 
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; } 

        // Navigation properties
        public UserLogin UserLogin { get; set; } 
        public ICollection<Task> Tasks { get; set; } 
        public ICollection<Comment> Comments { get; set; } 
        public ICollection<ActivityLog> ActivityLogs { get; set; } 
        public ICollection<Notification> Notifications { get; set; } 
    }
}
