using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Interfaces;
using TaskManagement.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Repositories
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly AppDbContext _context;

        public UserLoginRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserLogin> GetUserLoginByIdAsync(int userLoginId)
        {
            return await _context.UserLogins.FindAsync(userLoginId);
        }

        public async Task<UserLogin> GetUserLoginByUsernameAsync(string username)
        {
            return await _context.UserLogins.FirstOrDefaultAsync(ul => ul.Username == username);
        }

        public async Task<IEnumerable<UserLogin>> GetAllUserLoginsAsync()
        {
            return await _context.UserLogins.ToListAsync();
        }

        public async Task AddUserLoginAsync(UserLogin userLogin)
        {
            await _context.UserLogins.AddAsync(userLogin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserLoginAsync(UserLogin userLogin)
        {
            _context.UserLogins.Update(userLogin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserLoginAsync(int userLoginId)
        {
            var userLogin = await GetUserLoginByIdAsync(userLoginId);
            if (userLogin != null)
            {
                _context.UserLogins.Remove(userLogin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangePassword(int userLoginId, string newPassword)
        {
            var userLogin = await GetUserLoginByIdAsync(userLoginId);
            if (userLogin != null)
            {
                userLogin.PasswordHash = newPassword;
                _context.UserLogins.Update(userLogin);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
