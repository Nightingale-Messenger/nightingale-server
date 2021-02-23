using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nightingale.Core.Entities;
using Nightingale.Core.Identity;

namespace Nightingale.Infrastructure.Data
{
    public class NightingaleContext : IdentityDbContext<User>
    {
        public DbSet<Message> Messages { get; set; }
        
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public NightingaleContext(DbContextOptions<NightingaleContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.SentMessages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId);

            builder.Entity<User>()
                .HasMany(u => u.ReceivedMessages)
                .WithOne(m => m.Receiver)
                .HasForeignKey(m => m.ReceiverId);

            builder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(t => t.User);
        }
    }
}