using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Message> Messages { get; set; }
        
        public DbSet<Contact> Contacts { get; set; }
        
        public DbSet<UserContact> UserContacts { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserContact>()
                .HasKey(u => new {u.UserId, u.ContactId});
            
            modelBuilder.Entity<UserContact>()
                .HasOne(uc => uc.User)
                .WithMany(c => c.UserContacts)
                .HasForeignKey(uc => uc.UserId);
            
            modelBuilder.Entity<UserContact>()
                .HasOne(uc => uc.Contact)
                .WithMany(u => u.UserContacts)
                .HasForeignKey(uc => uc.ContactId);

            base.OnModelCreating(modelBuilder);
        }
    }
}