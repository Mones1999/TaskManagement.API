using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskManagement.Core.Entities
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedUserId { get; set; }
        public string Status { get; set; } 
        public DateTime DueDate { get; set; }

        public User? AssignedUser { get; set; } 
        public ICollection<Comment>? Comments { get; set; }
    }
}
