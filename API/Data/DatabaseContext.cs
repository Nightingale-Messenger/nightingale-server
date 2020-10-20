using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Message> Messages { get; set; }
        
        public DbSet<Contact> Contacts { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}