using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Token> Tokens { get; set; }
        
        public DbSet<Message> Messages { get; set; }
        
        public DbSet<Conversation> Conversations { get; set; }
        
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