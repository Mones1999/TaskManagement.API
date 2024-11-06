using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Entities
{
    public class ActivityLog
    {
        public int ActivityLogId { get; set; } 
        public int UserId { get; set; } 
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string Username { get; set; }

        // Navigation property
        public User User { get; set; } 
    }

}
