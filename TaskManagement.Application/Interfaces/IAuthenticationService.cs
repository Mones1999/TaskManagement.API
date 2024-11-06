using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.DTOs;

namespace TaskManagement.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string>? Login(LoginCredentails loginDetails);
    }
}
