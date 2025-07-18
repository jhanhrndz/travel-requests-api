using Microsoft.EntityFrameworkCore;
using TravelRequests.Api.Models;

namespace TravelRequests.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<TravelRequest> TravelRequests { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<TravelRequest>()
                .HasOne(tr => tr.User)
                .WithMany(u => u.TravelRequests)
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            
            modelBuilder.Entity<TravelRequest>()
                .Property(tr => tr.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}