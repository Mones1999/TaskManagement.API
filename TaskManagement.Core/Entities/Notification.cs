using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; } 
        public int UserId { get; set; } 
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property
        public User User { get; set; } 
    }

}
