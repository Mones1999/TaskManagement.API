﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.DTOs
{
    public class UsersTaskDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int NumberOfTasks { get; set; }
        public string Status  { get; set; }
    }
}
