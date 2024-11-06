using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.DTOs;
using TaskManagement.Infrastructure.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AppDbContext _context;

        public AuthenticationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUserNameExist(string userName)
        {
            return await _context.UserLogins.AnyAsync(ul => ul.Username.Equals(userName));
        }

        public async Task<TokenPayload> Login(LoginCredentails loginDetails)
        {
            var userLogin = await _context.UserLogins
                .Include(ul => ul.User)
                .FirstOrDefaultAsync(ul => ul.Username.Equals(loginDetails.Username) && ul.PasswordHash.Equals(loginDetails.Password));
            if (userLogin != null) 
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userLogin.User.UserId);
                return new TokenPayload { 
                    Username = userLogin.Username,
                    Userid = user.UserId,
                    Role = userLogin.Role,
                };
            }
            return null;

        }

        
    }
}
