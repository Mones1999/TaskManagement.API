using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.DTOs
{
    public class TokenPayload
    {
        public string Username { get; set; } = null!;
        public int? Userid { get; set; }
        public string Role { get; set; }
    }
}
