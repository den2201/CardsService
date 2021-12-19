
using CardService.DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace CardService.Domain
{
    public class AppDbContext :DbContext
    {
       public DbSet<Card> Cards { get; set; }
       public DbSet<CardDateExpired> CardDateExpired { get; set; }
      
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            DataSeeder.Initialize(modelBuilder);
            modelBuilder.Entity<Card>(t => {
                t.Property(b => b.CVC).HasMaxLength(3).IsRequired();
                t.Property(b => b.Pan).HasMaxLength(16).IsRequired();

            });
          
            modelBuilder.Entity<CardDateExpired>(a =>
            {
                a.HasOne(x => x.Card)
                    .WithOne(x => x.CardDateExpired);
                a.Property(t => t.Month).IsRequired().HasMaxLength(2);
                a.Property(t => t.Year).IsRequired().HasMaxLength(4);
                a.HasKey(x => x.CardId);
              
            });
        }
    }
}
