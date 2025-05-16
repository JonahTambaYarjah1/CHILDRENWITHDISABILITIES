using Microsoft.EntityFrameworkCore;
using ChildrenWithDisabilitiesAPI.Models;

namespace ChildrenWithDisabilitiesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Disability> Disabilities { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
