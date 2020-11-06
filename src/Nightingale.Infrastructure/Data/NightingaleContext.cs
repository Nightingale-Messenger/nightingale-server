using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nightingale.Core.Entities;
using Nightingale.Core.Identity;

namespace Nightingale.Infrastructure.Data
{
    public class NightingaleContext : IdentityDbContext<User>
    {
        public DbSet<Message> Messages { get; set; }
        
        public NightingaleContext(DbContextOptions<NightingaleContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Message>()
                .HasKey(m => new {m.SenderId, m.ReceiverId});

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(s => s.Messages)
                .HasForeignKey(m => m.SenderId);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(r => r.Messages)
                .HasForeignKey(m => m.ReceiverId);
        }
    }
}