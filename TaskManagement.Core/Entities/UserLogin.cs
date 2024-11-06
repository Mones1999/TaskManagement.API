using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Entities
{
    public class UserLogin
    {
        public int UserLoginId { get; set; } 
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
