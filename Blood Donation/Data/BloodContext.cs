using Blood_Donation.Models;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation.Data
{
    public class BloodContext : DbContext
    {
        public DbSet<Blood> Bloods { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public BloodContext(DbContextOptions<BloodContext> options) : base(options) { }
        public BloodContext() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the relationship between Account and Blood
            modelBuilder.Entity<Blood>()
                .HasOne(b => b.Account)
                .WithMany(a => a.BloodDonations)
                .HasForeignKey(b => b.AccountId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =YOUSSEFEBRAHEM\\SQLEXPRESS;Database=Blood;Integrated Security=True;Trust Server Certificate=True;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}