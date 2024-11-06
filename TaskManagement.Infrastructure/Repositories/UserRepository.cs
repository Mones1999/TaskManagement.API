using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.DTOs;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Interfaces;
using TaskManagement.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.Include(u => u.UserLogin).FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.UserLogin).ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UsersTaskDTO>> getUsersByTopNumberOfTasks(int numberOfUser)
        {
            var query = await _context.Users
                .Where(u => u.Tasks.Any())
                .Select(u => new UsersTaskDTO
                {
                    UserId = u.UserId,
                    Username = u.FullName,
                    NumberOfTasks = u.Tasks.Count,
                    Status = u.Status
                })
                .OrderByDescending(dto => dto.NumberOfTasks)
                .Take(numberOfUser)
                .ToListAsync();
            return query;
        }

        public async Task<User> GetUserByUserLoginId(int userLoginId)
        {
            return await _context.Users.FirstOrDefaultAsync(u=> u.UserLoginId == userLoginId);
        }
    }
}
