using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.DTOs;

namespace TaskManagement.Infrastructure.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<TokenPayload> Login(LoginCredentails loginDetails);
        Task<bool> IsUserNameExist(string userName);
    }
}
