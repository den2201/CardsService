﻿// <auto-generated />
using System;
using CardService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CardService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CardService.Database.Entites.CardDateEntity", b =>
                {
                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("CardId");

                    b.ToTable("ExpiredDates");

                    b.HasData(
                        new
                        {
                            CardId = new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"),
                            Month = 12,
                            Year = 2023
                        },
                        new
                        {
                            CardId = new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"),
                            Month = 5,
                            Year = 2019
                        });
                });

            modelBuilder.Entity("CardService.Database.Entites.CardEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("CardId");

                    b.Property<string>("CVC")
                        .HasColumnType("text");

                    b.Property<string>("CardName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("Pan")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Cards");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"),
                            CVC = "123",
                            CardName = "First card",
                            IsDefault = true,
                            Pan = "4397185796979658",
                            UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814")
                        },
                        new
                        {
                            Id = new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"),
                            CVC = "666",
                            CardName = "Second card",
                            IsDefault = false,
                            Pan = "2367000000019234",
                            UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814")
                        });
                });

            modelBuilder.Entity("CardService.Database.Entites.CardDateEntity", b =>
                {
                    b.HasOne("CardService.Database.Entites.CardEntity", "Card")
                        .WithOne("Date")
                        .HasForeignKey("CardService.Database.Entites.CardDateEntity", "CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("CardService.Database.Entites.CardEntity", b =>
                {
                    b.Navigation("Date");
                });
#pragma warning restore 612, 618
        }
    }
}