using ChildrenWithDisabilitiesAPI.Data;
using ChildrenWithDisabilitiesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChildrenWithDisabilitiesAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> Login(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
