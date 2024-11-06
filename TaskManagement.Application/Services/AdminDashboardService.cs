using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.DTOs;
using TaskManagement.Infrastructure.Interfaces;

namespace TaskManagement.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        public AdminDashboardService(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }
        public async Task<int> getNumberOfCompletedTasks()
        {
            return await _taskRepository.getNumberOfCompletedTasks();
        }

        public async Task<int> getNumberOfPendingTasks()
        {
            return await _taskRepository.getNumberOfPendingTasks();
        }

        public async Task<int> getNumberOfTasks()
        {
            return await _taskRepository.getNumberOfTasks();
        }

        public async Task<IEnumerable<UsersTaskDTO>> getUsersByTopNumberOfTasks(int numberOfUser)
        {
            return await _userRepository.getUsersByTopNumberOfTasks(numberOfUser);
        }

        public async Task<int> getNumberOfUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Count();
        }
    }
}
