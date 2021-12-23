
using Microsoft.EntityFrameworkCore;
using System;
using TransactionService.Domain;

namespace TransactionService.Datalayer
{
    public class AppDbContext :DbContext
    {
       public DbSet<Transaction> Transactions { get; set; }
      
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<Transaction>(t =>
            {
                t.Property<Guid>("Id").IsRequired();

            });
        }
    }
}
