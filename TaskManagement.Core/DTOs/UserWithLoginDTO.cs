using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.DTOs
{
    public class UserWithLoginDTO
    {
        // UserLogin properties
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        // User properties
        public int UserLoginId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
    }

}
