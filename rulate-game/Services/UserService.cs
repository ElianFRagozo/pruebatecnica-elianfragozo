using Microsoft.EntityFrameworkCore;
using RouletteAPI.Data;
using RouletteAPI.Models;
using System;
using System.Threading.Tasks;

namespace RouletteAPI.Services
{
    public interface IUserService
    {
        Task<User> GetUserByNameAsync(string name);
        Task<User> SaveUserAsync(User user);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
        }

        public async Task<User> SaveUserAsync(User user)
        {
            var existingUser = await GetUserByNameAsync(user.Name);

            if (existingUser != null)
            {
                // Actualizar saldo del usuario existente
                existingUser.Balance = user.Balance;
                _context.Users.Update(existingUser);
            }
            else
            {
                // Crear nuevo usuario
                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync();
            return existingUser ?? user;
        }
    }
}