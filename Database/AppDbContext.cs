
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

            modelBuilder.Entity<Card>().HasData(
         new Card()
         {
             Id = new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"),
             UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814"),
             CVC = "123",
             Pan = "4397185796979658",
             IsDefault = true,
             CardName = "First card",
             
         },
           new Card()
           {

               Id = new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"),
               UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814"),
               CVC = "666",
               Pan = "2367000000019234",
               IsDefault = false,
               CardName = "Second card",

           });
            modelBuilder.Entity<CardDateExpired>().HasData(
                new CardDateExpired
                {
                    CardId = new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"),
                    Month = 12,
                    Year = 2023

                },
                new CardDateExpired
                {
                    CardId = new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"),
                    Month = 5,
                    Year = 2030
                });

            modelBuilder.Entity<CardDateExpired>(a =>
            {
                a.HasOne(x => x.Card)
                    .WithOne(x => x.CardDateExpired);
               
                a.HasKey(x => x.CardId);
              
            });
        }
    }
}
